using System;
using UnityEngine;

public class BobaMovement : MonoBehaviour
{
    [SerializeField] private double Modifier = 0.2;

    private double _t = 0;
    private Vector3 _originalPos;

    void Start()
    {
        _originalPos = transform.position;
    }

    void Update()
    {
        _t = _t < 2 * Math.PI ? _t + Time.deltaTime : 0;

        transform.position = new Vector3(
            (float) (Modifier * Math.Cos(_t) + _originalPos.x),
            (float) (Modifier * Math.Sin(_t) + _originalPos.y),
            0
        );
    }
}
