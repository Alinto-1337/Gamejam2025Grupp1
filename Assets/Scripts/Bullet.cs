using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    [SerializeField] float lifetime = 5f;
    [Tooltip("Delay before the bullet is destroyed after hitting something")]
    [SerializeField] float bulletDieDelay = 0.1f;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.LookAt(transform.position + rb.linearVelocity.normalized);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemieBehavior>().ApplyDamage(damage);
        }

        Destroy(gameObject, bulletDieDelay);
    }
}
