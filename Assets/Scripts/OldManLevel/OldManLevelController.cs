using System;
using System.Collections;
using System.Collections.Generic;
using RhythmJam;
using UnityEngine;

public class OldManLevelController : MonoBehaviour
{
    [NonNullField]
    public GameObject DiscoLights;

    [NonNullField]
    public GameObject Crowd;
    
    public float DiscoLightsRotationSpeed = 0.1f;

    public List<GameObject> DiscoBallSprites = new();
    private int _currentDiscoBallFrame = 0;

    [NonNullField]
    public GameObject DjIdleSprite;
    
    [NonNullField]
    public GameObject DjPointingSprite;
    
    public List<GameObject> WalkerAnimationSprites = new();
    public List<GameObject> Dance1AnimationSprites = new();
    public List<GameObject> Dance2AnimationSprites = new();
    public List<GameObject> Dance3AnimationSprites = new();
    public GameObject VictorySprite;
    public GameObject MissSprite;

    private float _callgroupTimeoutSec = 0f;
    public float CallGroupTimeoutSec = 3.0f;
    private int _callGroupNumber = 0;
    private bool _newCallGroup = false;

    private float _djPointTimeoutSec = 0f;

    [NonNullField] public GameObject NoteIndicator;

    private enum OldManAnimation
    {
        Walker,
        Dance1,
        Dance2,
        Dance3,
        Victory,
        Miss,
        None
    }
    
    private OldManAnimation _oldManAnimation = OldManAnimation.None;
    private int _animationFrame = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        CallResponseGameplayManager.Instance.OnBeat += OnBeat;
        CallResponseGameplayManager.Instance.OnSongEvent += OnSongEvent;
        CallResponseGameplayManager.Instance.OnCallNote += OnCallNote;
        CallResponseGameplayManager.Instance.OnResponseNote += OnResponseNote;
    }

    public void Initialize()
    {
        _oldManAnimation = OldManAnimation.None;
        SwitchAnimationSet(OldManAnimation.Walker);
        NoteIndicator.SetActive(true);
    }

    private void OnResponseNote(object sender, CallResponseGameplayManager.Judgement judgment)
    {
        switch (judgment)
        {
            case CallResponseGameplayManager.Judgement.Perfect:
            case CallResponseGameplayManager.Judgement.Good:
                if (_callGroupNumber == 4)
                {
                    SwitchAnimationSet(OldManAnimation.Dance1);
                }
                
                if (_callGroupNumber == 6)
                {
                    SwitchAnimationSet(OldManAnimation.Dance2);
                }
                
                if (_callGroupNumber == 11)
                {
                    SwitchAnimationSet(OldManAnimation.Dance3);
                }
                
                AdvanceAnimationFrame(_oldManAnimation);
                break;
            case CallResponseGameplayManager.Judgement.Miss:
                break;
        }
        
        // If it's the last response note, do the Victory pose!
        if (CallResponseGameplayManager.Instance.IsLastResponseNote())
        {
            OnLastResponseNote();
        }
    }

    public void OnLastResponseNote()
    {
        SwitchAnimationSet(OldManAnimation.Victory);
        NoteIndicator.SetActive(false);
    }

    private void SwitchAnimationSet(OldManAnimation oldManAnimation)
    {
        if (_oldManAnimation == oldManAnimation)
        {
            // Don't do anything if the animation doesn't actually change.
            return;
        }
        
        _animationFrame = 0;
        _oldManAnimation = oldManAnimation;
        DisableSprites(WalkerAnimationSprites);
        DisableSprites(Dance1AnimationSprites);
        DisableSprites(Dance2AnimationSprites);
        DisableSprites(Dance3AnimationSprites);
        VictorySprite.SetActive(false);
        MissSprite.SetActive(false);
        
        switch (_oldManAnimation)
        {
            case OldManAnimation.Walker:
                WalkerAnimationSprites[_animationFrame].SetActive(true);
                break;
            case OldManAnimation.Dance1:
                Dance1AnimationSprites[_animationFrame].SetActive(true);
                break;
            case OldManAnimation.Dance2:
                Dance2AnimationSprites[_animationFrame].SetActive(true);
                break;
            case OldManAnimation.Dance3:
                Dance3AnimationSprites[_animationFrame].SetActive(true);
                break;
            case OldManAnimation.Victory:
                VictorySprite.SetActive(true);
                break;
            case OldManAnimation.Miss:
                MissSprite.SetActive(true);
                break;
        }
    }

    private void AdvanceAnimationFrame(OldManAnimation animation)
    {
        switch (_oldManAnimation)
        {
            case OldManAnimation.Walker:
                WalkerAnimationSprites[_animationFrame].SetActive(false);
                _animationFrame = (_animationFrame + 1) % WalkerAnimationSprites.Count;
                WalkerAnimationSprites[_animationFrame].SetActive(true);
                break;
            case OldManAnimation.Dance1:
                Dance1AnimationSprites[_animationFrame].SetActive(false);
                _animationFrame = (_animationFrame + 1) % Dance1AnimationSprites.Count;
                Dance1AnimationSprites[_animationFrame].SetActive(true);
                break;
            case OldManAnimation.Dance2:
                Dance2AnimationSprites[_animationFrame].SetActive(false);
                _animationFrame = (_animationFrame + 1) % Dance2AnimationSprites.Count;
                Dance2AnimationSprites[_animationFrame].SetActive(true);
                break;
            case OldManAnimation.Dance3:
                Dance3AnimationSprites[_animationFrame].SetActive(false);
                _animationFrame = (_animationFrame + 1) % Dance3AnimationSprites.Count;
                Dance3AnimationSprites[_animationFrame].SetActive(true);
                break;
            case OldManAnimation.Victory:
            case OldManAnimation.Miss:
                break;
        }
    }

    private void DisableSprites(List<GameObject> sprites)
    {
        foreach (GameObject sprite in sprites)
        {
            sprite.SetActive(false);
        }
    }
    
    private void OnCallNote()
    {
        if (_newCallGroup)
        {
            _newCallGroup = false;
            _callGroupNumber += 1;
            Debug.Log("New Call Group! " + _callGroupNumber);
        }
        
        // If it's been longer than X seconds since we've received a call note, we say it's the end of a call-group.
        _callgroupTimeoutSec = CallGroupTimeoutSec;
        
        // Switch the DJ to point finger. Set a timeout to swap back to idle a bit before the next call note.
        SetDjPointing(true);
        double timeUntilCallNote = CallResponseGameplayManager.Instance.TimeUntilCallNote();
        if (timeUntilCallNote < 0 || timeUntilCallNote > CallGroupTimeoutSec)
        {
            _djPointTimeoutSec = 1.0f;
        }
        else
        {
            _djPointTimeoutSec = (float)timeUntilCallNote - 0.25f;
        }
    }

    private void SetDjPointing(bool point)
    {
        if (point)
        {
            DjIdleSprite.SetActive(false);
            DjPointingSprite.SetActive(true);
        }
        else
        {
            DjIdleSprite.SetActive(true);
            DjPointingSprite.SetActive(false);
        }
    }
    
    private void OnSongEvent(object sender, SongEvent.EventType e)
    {
        if (e == SongEvent.EventType.Divergence)
        {
            Debug.Log("Song switch!");
        }
    }

    private void OnBeat(object sender, int e)
    {
        AdvanceDiscoBallFrame();
    }
    
    void Update()
    {
        // Get time to nearest beat. The closer to a beat, the bigger the scale
        var engine = CallResponseGameplayManager.Instance.RhythmEngine;
        var time = engine.GetCurrentAudioTime() - engine.Song.FirstBeatOffset;
        var timeToNearestBeat = CallResponseGameplayManager.Instance.CurrentSong.DistanceToNearestNBeat(time, 2);
        var t = (float)(timeToNearestBeat / CallResponseGameplayManager.Instance.CurrentSong.TimePerBeat);

        // Debug.Log("Time To Nearest Beat: " + t);
        if (t < 0.01666666666)
        {
            // Beat
        }

        float scale = Mathf.Lerp(0.9f, 1.0f, t);
        Crowd.transform.localScale = new Vector3(1, scale, 1);

        if (_callgroupTimeoutSec > 0.0f)
        {
            _callgroupTimeoutSec -= Time.deltaTime;
        }

        if (_callgroupTimeoutSec <= 0.0f && _callgroupTimeoutSec > -1.0f)
        {
            _callgroupTimeoutSec = -10.0f;
            _newCallGroup = true;
        }

        // DJ timer
        if (_djPointTimeoutSec > 0.0f)
        {
            _djPointTimeoutSec -= Time.deltaTime;
        }

        if (_djPointTimeoutSec <= 0.0f)
        {
            SetDjPointing(false);    
        }
    }
    
    void FixedUpdate()
    {
        DiscoLights.transform.Rotate(Vector3.forward, DiscoLightsRotationSpeed);
    }

    void AdvanceDiscoBallFrame()
    {
        DisableSprites(DiscoBallSprites);
        _currentDiscoBallFrame = (_currentDiscoBallFrame + 1) % DiscoBallSprites.Count;
        DiscoBallSprites[_currentDiscoBallFrame].SetActive(true);
    }
}
