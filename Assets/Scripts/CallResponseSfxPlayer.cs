using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmJam
{

public class CallResponseSfxPlayer : MonoBehaviour
{
    public AudioSource CallAudioSource;
    public AudioSource HitAudioSource;
    public AudioSource MissAudioSource;

    private float _delay = 0.1f;

    void OnEnable() {
        CallResponseGameplayManager.Instance.OnCallNote += OnCallNote;
        CallResponseGameplayManager.Instance.OnResponseNote += OnResponseNote;
    }

    void OnDisable() {
        CallResponseGameplayManager.Instance.OnCallNote -= OnCallNote;
        CallResponseGameplayManager.Instance.OnResponseNote -= OnResponseNote;
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
