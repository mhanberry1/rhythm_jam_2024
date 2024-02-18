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
    
    // Start is called before the first frame update
    void Start()
    {
        CallResponseGameplayManager.Instance.OnBeat += OnBeat;
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

        Debug.Log("Time To Nearest Beat: " + t);
        if (t < 0.01666666666)
        {
            // Beat
        }

        float scale = Mathf.Lerp(0.9f, 1.0f, t);
        Crowd.transform.localScale = new Vector3(1, scale, 1);
    }
    
    void FixedUpdate()
    {
        DiscoLights.transform.Rotate(Vector3.forward, DiscoLightsRotationSpeed);
    }

    void AdvanceDiscoBallFrame()
    {
        foreach (GameObject discoBallSprite in DiscoBallSprites)
        {
            discoBallSprite.SetActive(false);
        }

        _currentDiscoBallFrame = (_currentDiscoBallFrame + 1) % DiscoBallSprites.Count;
        DiscoBallSprites[_currentDiscoBallFrame].SetActive(true);
    }
}
