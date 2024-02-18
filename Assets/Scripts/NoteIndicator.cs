using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmJam
{

public class NoteIndicator : MonoBehaviour
{

    public GameObject OuterCirclePrefab;
    public SpriteRenderer InnerCircle;
    public SpriteRenderer FilledCircle;

    void OnEnable() {
        CallResponseGameplayManager.Instance.OnCallNote += OnCallNote;
        CallResponseGameplayManager.Instance.OnResponseNote += OnResponseNote;
    }

    void OnDisable() {
        CallResponseGameplayManager.Instance.OnCallNote -= OnCallNote;
        CallResponseGameplayManager.Instance.OnResponseNote -= OnResponseNote;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCallNote() {
        // Spawn indicator circle
        var circle = Instantiate(OuterCirclePrefab, this.gameObject.transform);
    }

    void OnResponseNote(object sender, CallResponseGameplayManager.Judgement judgement) {
        switch (judgement) {
            case CallResponseGameplayManager.Judgement.Miss:
                // Change color to red
                break;
            case CallResponseGameplayManager.Judgement.Good:
                // 
                break;
            case CallResponseGameplayManager.Judgement.Perfect:
                break;
        }
    }
}

}