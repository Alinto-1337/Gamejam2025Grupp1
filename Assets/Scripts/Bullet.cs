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


    private void OnCollisionEnter(Collision other)
    {
        print("GAYYY");
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemieBehavior enemyBehavior = other.gameObject.GetComponent<EnemieBehavior>();
            enemyBehavior.ApplyDamage(damage);
            Instantiate(enemyBehavior.bulletHitFxPrefab, other.GetContact(0).point, Quaternion.identity);
        }
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
