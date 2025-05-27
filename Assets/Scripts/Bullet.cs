using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.LookAt(transform.position + rb.linearVelocity.normalized);
    }
}
