using System;
using UnityEngine;

namespace RhythmJam
{

public class BobaBallMovement : MonoBehaviour
{
    [SerializeField] GameObject Spaceship;

    private Vector3 _direction;
    private float _speed;
    private float _timePassed = 0;
    private double _t = 0;
    private Vector3 _originalPos;

    void OnEnable()
    {
        Vector3 target = new Vector3 (
            Spaceship.transform.position.x,
            Spaceship.GetComponent<SpriteRenderer>().bounds.min.y,
            0
        );

        Vector3 differenceVect = (target - transform.position);
        _direction = differenceVect.normalized;
        _speed = 0.65f * differenceVect.magnitude / (float) CallResponseGameplayManager.Instance.CurrentSong.TimeUntilResponse();
        _originalPos = transform.position;
        Debug.Log(CallResponseGameplayManager.Instance.CurrentSong.TimeUntilResponse());
    }

    void Update()
    {
        _timePassed += Time.deltaTime;
        _t = _t < 2 * Math.PI ? _t + Time.deltaTime: 0;

        Vector3 newPos = _direction * _speed * _timePassed + _originalPos;
        newPos.x += (float) (0.3 * Math.Cos(7 * _t));

        transform.position = newPos;
    }
}

}
