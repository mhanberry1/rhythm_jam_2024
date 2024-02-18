using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmJam
{

public class BobToBeat : MonoBehaviour
{
    public float MaxScale = 1.05f;
    
    private Vector3 _originalScale;

    // Start is called before the first frame update
    void Start()
    {
        _originalScale = gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Get time to nearest beat. The closer to a beat, the bigger the scale
        var engine = CallResponseGameplayManager.Instance.RhythmEngine;
        var time = engine.GetCurrentAudioTime() - engine.Song.FirstBeatOffset;
        var timeToNearestBeat = CallResponseGameplayManager.Instance.CurrentSong.DistanceToNearestBeat(time);
        var t = timeToNearestBeat / CallResponseGameplayManager.Instance.CurrentSong.TimePerBeat;
        var scale = Mathf.Lerp(1f, MaxScale, (float)(t*t));
        gameObject.transform.localScale = new Vector3(_originalScale.x, _originalScale.y * scale, _originalScale.z);
    }
}

}
