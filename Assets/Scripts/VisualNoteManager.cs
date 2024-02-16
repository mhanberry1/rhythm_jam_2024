using System;
using System.Collections;
using System.Collections.Generic;
using RhythmJam;
using UnityEngine;
using UnityEngine.UIElements;

public class VisualNoteManager : Singleton<VisualNoteManager>
{
    // Effects References
    [NonNullField] public GameObject MissVfxPrefab;
    [NonNullField] public GameObject GoodVfxPrefab;
    [NonNullField] public GameObject PerfectVfxPrefab;

    void Start()
    {
        CallResponseGameplayManager.Instance.OnResponseNote += OnResponseNote;
    }

    private void OnResponseNote(object sender, CallResponseGameplayManager.Judgement judgement)
    {
        switch (judgement)
        {
            case CallResponseGameplayManager.Judgement.Miss:
                Instantiate(MissVfxPrefab);
                break;
            case CallResponseGameplayManager.Judgement.Good:
                Instantiate(GoodVfxPrefab);
                break;
            case CallResponseGameplayManager.Judgement.Perfect:
                Instantiate(PerfectVfxPrefab);
                break;
        }
    }
}
