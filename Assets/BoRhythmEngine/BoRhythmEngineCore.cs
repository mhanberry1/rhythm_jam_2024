using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmJam
{

public class BoRhythmEngineCore : MonoBehaviour
{
    [SerializeField] private Song SongToPlay;
    public AudioSource MusicSource;


    public bool IsPaused;
    public double SourceStartTime;

    public bool ManualMode;

    public bool HasStarted => _isPlaying;
    public Song Song => SongToPlay;

    public event Action OnStartPlaying;


    private double _startDspTime = 0;
    private bool _isPlaying = false;

    private double _sourceStartTime;

    private void Awake()
    {
        if (ManualMode) return;

        SetClip();
        InitTime();
        Play();
    }


    public void SetSong(Song song, bool setClip = true)
    {
        SongToPlay = song;
        if (setClip) SetClip();
    }

    public void InitTime()
    {

    }

    public void SetStartTime(double startTime) 
    {
        _sourceStartTime = startTime;
    }
    
    public double GetCurrentAudioTime() {
        if (!_isPlaying) return _sourceStartTime;

        return AudioSettings.dspTime - _startDspTime;
    }

    public void Play() {
        MusicSource.time = (float)_sourceStartTime;

        double playTime = AudioSettings.dspTime + 1;
        MusicSource.PlayScheduled(playTime);
        _startDspTime = playTime;

        _isPlaying = true;
        OnStartPlaying?.Invoke();
    }

    public void Pause() {
        _sourceStartTime = GetCurrentAudioTime();
        MusicSource.Stop();
        _isPlaying = false;
    }

    public void Unpause() {
        Play();
    }

    private void SetClip()
    {
        if (Song.Clip == null)
        {
            Debug.LogError("No clip set for the song, set it in the song's inspector", gameObject);
            return;
        }

        MusicSource.clip = Song.Clip;
    }
}

}