using UnityEngine;

namespace RhythmJam
{

public class AnimateHit : MonoBehaviour
{
    [SerializeField] Sprite HitSprite;
    [SerializeField] float Duration = 0.5f;

    private Sprite _originalSprite;
    private float _remainingDuration = 0;

    private void OnEnable()
    {
        CallResponseGameplayManager.Instance.OnResponseNote += OnResponseNote;
    }

    private void Start()
    {
        _originalSprite = GetComponent<SpriteRenderer>().sprite;
    }

    private void Update()
    {
        if (_remainingDuration <= 0 ) return;

        _remainingDuration -= Time.deltaTime;

        if (_remainingDuration > 0) return;

        GetComponent<SpriteRenderer>().sprite = _originalSprite;
    }

    private void OnResponseNote(object sender, CallResponseGameplayManager.Judgement judgement)
    {
        if (judgement != CallResponseGameplayManager.Judgement.Miss) return;

        GetComponent<SpriteRenderer>().sprite = HitSprite;
        _remainingDuration = Duration;
    }
}

}
