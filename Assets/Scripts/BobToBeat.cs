using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmJam
{

public class BobToBeat : MonoBehaviour
{
    public int bobInterval = 1;
    public float MaxYScale = 1.05f;
    public float MaxXScale = 1.0f;
    public float MinRotation = 0f;
    public float MaxRotation = 0f;

    private Vector3 _originalScale;
    private Quaternion _originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        _originalScale = gameObject.transform.localScale;
        _originalRotation = gameObject.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Get time to nearest beat. The closer to a beat, the bigger the scale
        var engine = CallResponseGameplayManager.Instance.RhythmEngine;
        var time = engine.GetCurrentAudioTime() - engine.Song.FirstBeatOffset;
        var timeToNearestBeat = CallResponseGameplayManager.Instance.CurrentSong.DistanceToNearestNBeat(time, bobInterval);
        Debug.Log(timeToNearestBeat);
        var t = (float)(timeToNearestBeat / CallResponseGameplayManager.Instance.CurrentSong.TimePerBeat / bobInterval);

        var scaleX = Mathf.Lerp(1f, MaxXScale, t*t);
        var scaleY = Mathf.Lerp(1f, MaxYScale, t*t);
        gameObject.transform.localScale = new Vector3(_originalScale.x * scaleX, _originalScale.y * scaleY, _originalScale.z);

        var rotationRad = Mathf.Lerp(MinRotation, MaxRotation, t);
        var rotation = Quaternion.Euler(0, 0, rotationRad);
        gameObject.transform.rotation = _originalRotation * rotation;
    }
}

}
