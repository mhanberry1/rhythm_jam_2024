using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmJam
{

public class EditorSfxPlayer : MonoBehaviour
{
    public CallResponseAnnouncer Announcer;
    public AudioSource AudioSource;

    void OnEnable() {
        Announcer.OnCallNotePlayed += OnCallNotePlayed;
    }

    void OnDisable() {
        Announcer.OnCallNotePlayed -= OnCallNotePlayed;
    }

    void OnCallNotePlayed(CallResponseNote note) {
        AudioSource.PlayScheduled(0.1);
    }

}

}
