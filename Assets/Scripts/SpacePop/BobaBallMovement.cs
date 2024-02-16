using UnityEngine;

public class BobaBallMovement : MonoBehaviour
{
    [SerializeField] GameObject Spaceship;
    [SerializeField] float Speed = 1f;

    private Vector3 _direction;

    void OnEnable()
    {
        _direction = (Spaceship.transform.position - transform.position).normalized;
    }

    void Update()
    {
        transform.Translate(_direction * Speed * Time.deltaTime);
    }
}
