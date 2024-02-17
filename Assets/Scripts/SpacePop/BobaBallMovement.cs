using System;
using UnityEngine;

namespace RhythmJam
{

public class BobaBallMovement : MonoBehaviour
{
    [SerializeField] GameObject Spaceship;

    private Vector3 _direction;
    private float _speed;
    private double _t = 0;

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
    }

    void Update()
    {
        _t = _t < 2 * Math.PI ? _t + Time.deltaTime: 0;

        Vector3 translationVect = _direction * _speed * Time.deltaTime;
        translationVect.x += (float) (0.03 * Math.Cos(7 * _t));

        transform.Translate(translationVect);
    }
}

}
