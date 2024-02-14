using System;
using System.Collections;
using System.Collections.Generic;
using RhythmJam;
using UnityEngine;
using UnityEngine.UIElements;

public class VisualNoteManager : Singleton<VisualNoteManager>
{
    [NonNullField]
    public GameObject NoteObject;

    public List<Transform> NoteSpawnLocations;

    [NonNullField] public GameObject NotesContainer;

    // Effects References
    [NonNullField] public GameObject MissVfxPrefab;
    [NonNullField] public GameObject GoodVfxPrefab;
    [NonNullField] public GameObject PerfectVfxPrefab;

    // Notes in a measure. They are queue by OnCallNote and dequeued by OnResponseNote.
    private Queue<GameObject> _measureNotes = new();

    void Start()
    {
        CallResponseGameplayManager.Instance.OnCallNote += OnCallNote;
        CallResponseGameplayManager.Instance.OnResponseNote += OnResponseNote;
    }

    public void Reset()
    {
        _measureNotes.Clear();
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

        try
        {
            GameObject noteObj = _measureNotes.Dequeue();
            Destroy(noteObj);
        }
        catch (InvalidOperationException)
        {
        }
    }

    private void OnCallNote()
    {
        GameObject noteObj = Instantiate(NoteObject, NotesContainer.transform);
        int locationIndex = _measureNotes.Count % NoteSpawnLocations.Count;
        noteObj.transform.position = NoteSpawnLocations[locationIndex].position;
        _measureNotes.Enqueue(noteObj);
    }
}
