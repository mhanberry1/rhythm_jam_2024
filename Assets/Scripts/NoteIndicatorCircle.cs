using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmJam
{

public class NoteIndicatorCircle : MonoBehaviour
{
    public SpriteRenderer CircleSprite;

    public double TimeToNote = 1;
    public float MaxScale = 5;
    
    private double _startTime;
    private double _expireTime;
    private Vector3 _originalScale;

    // Start is called before the first frame update
    void Start()
    {
        TimeToNote = CallResponseGameplayManager.Instance.CurrentSong.TimeUntilResponse();

        _startTime = CallResponseGameplayManager.Instance.RhythmEngine.GetCurrentAudioTime();
        _expireTime = _expireTime + TimeToNote;

        _originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        var time = CallResponseGameplayManager.Instance.RhythmEngine.GetCurrentAudioTime();
        // Normalized time between 0 to 1
        var tNormalized = (time - _startTime) / _expireTime;
        if (tNormalized > 1) {
            // destroy
        } else {
            // Interpolate between max and 1
            var scale = Mathf.Lerp(MaxScale, 1, (float)tNormalized);
            var alpha = tNormalized;
            
            transform.localScale = _originalScale * scale;

            var color = CircleSprite.color;
            color.a = (float)alpha;
            CircleSprite.color = color;
        }
    }
}

}