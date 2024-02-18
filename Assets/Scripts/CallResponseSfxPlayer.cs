using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmJam
{

public class CallResponseSfxPlayer : MonoBehaviour
{
    public CallResponseGameplayManager GameManager;
    public AudioSource CallAudioSource;
    public AudioSource HitAudioSource;
    public AudioSource MissAudioSource;

    private float _delay = 0.1f;

    void OnEnable() {
        GameManager.OnCallNote += OnCallNote;
        GameManager.OnResponseNote += OnResponseNote;
    }

    void OnDisable() {
        GameManager.OnCallNote -= OnCallNote;
        GameManager.OnResponseNote -= OnResponseNote;
    }

    void OnCallNote() {
        CallAudioSource.PlayScheduled(_delay);
    }

    void OnResponseNote(object sender, CallResponseGameplayManager.Judgement judgement) {
        switch (judgement)
        {
            case CallResponseGameplayManager.Judgement.Miss:
                MissAudioSource.PlayScheduled(_delay);
                break;
            case CallResponseGameplayManager.Judgement.Good:
            case CallResponseGameplayManager.Judgement.Perfect:
                HitAudioSource.PlayScheduled(_delay);
                break;
            case CallResponseGameplayManager.Judgement.NoNote:
                // No nearby note, do nothing.
                break;
        }
    }

}

}
