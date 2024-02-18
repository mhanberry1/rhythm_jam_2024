using UnityEngine;

public class Expire : MonoBehaviour
{
    [SerializeField] private float Ttl = 10;

    void Update()
    {
        if (Ttl < 0) Destroy(gameObject);

        Ttl -= Time.deltaTime;
    }
}
