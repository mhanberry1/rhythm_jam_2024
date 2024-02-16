using System;
using UnityEngine;

public class SpaceshipMovement : MonoBehaviour
{
    [SerializeField] private double Modifier = 0.005;

    private double _t = 0;
    private Vector3 _originalPos;

    void Start()
    {
        _originalPos = transform.position;
    }

    void Update()
    {
        _t = _t < 2 * Math.PI ? _t + Time.deltaTime: 0;

        transform.Translate(
            (float) (Modifier * Math.Cos(_t + _originalPos.x)),
            0,
            0
        );
    }
}
