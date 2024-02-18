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

    private bool _isDeflected;
    private Vector3 _deflectionVector;
    public float DeflectionSpeed = 5f;

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

        CallResponseGameplayManager.Instance.OnResponseNote += OnResponseNote;
    }

    void OnDisable()
    {
        CallResponseGameplayManager.Instance.OnResponseNote -= OnResponseNote;
    }

    void Update()
    {
        _timePassed += Time.deltaTime;
        _t = _t < 2 * Math.PI ? _t + Time.deltaTime: 0;

        if (!_isDeflected) {
            Vector3 newPos = _direction * _speed * _timePassed + _originalPos;
            newPos.x += (float) (0.3 * Math.Cos(7 * _t));

            transform.position = newPos;
        } else {
            transform.position += _deflectionVector * Time.deltaTime;
        }
    }

    void OnResponseNote(object sender, CallResponseGameplayManager.Judgement judgement) {
        // Check if it is for this note
        var timeFromImpact = _timePassed - CallResponseGameplayManager.Instance.CurrentSong.TimeUntilResponse();
        if (Mathf.Abs((float)timeFromImpact) > CallResponseGameplayManager.Instance.CurrentSong.MissTimeMs / 1000) {
            // Not this note. Ignore
            return;
        }

        switch (judgement) {
            case CallResponseGameplayManager.Judgement.Good:
            case CallResponseGameplayManager.Judgement.Perfect:
                // Successfully deflected
                _isDeflected = true;
                // Pick random direction
                var direction = UnityEngine.Random.insideUnitCircle.normalized;
                if (direction.y < 0) {
                    direction.y = -direction.y;
                }
                _deflectionVector = direction * DeflectionSpeed;
                break; 
            case CallResponseGameplayManager.Judgement.Miss:
                // ouch
                Destroy(this.gameObject);
                break;
        }
    }
}

}
