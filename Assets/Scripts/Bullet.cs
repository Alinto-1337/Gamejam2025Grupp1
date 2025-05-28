using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    [SerializeField] float lifetime = 5f;
    [Tooltip("Delay before the bullet is destroyed after hitting something")]
    [SerializeField] float bulletDieDelay = 0.1f;
    [SerializeField] float knockbackForce = 5f;
    [SerializeField] GameObject defaultHitFXPrefab;

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
            if (enemyBehavior == null)
            {
                BombEnemyBehaviour bombEnemyBehavior = other.gameObject.GetComponent<BombEnemyBehaviour>();
                bombEnemyBehavior.ApplyDamage(damage);
                Instantiate(bombEnemyBehavior.bulletHitFxPrefab, other.GetContact(0).point, Quaternion.identity);
                Vector3 direction = (other.transform.position - transform.position).normalized;
                bombEnemyBehavior.ApplyKnockBack(direction, knockbackForce);
            }
            else
            {
                enemyBehavior.ApplyDamage(damage);
                Instantiate(enemyBehavior.bulletHitFxPrefab, other.GetContact(0).point, Quaternion.identity);
                Vector3 direction = (other.transform.position - transform.position).normalized;
                enemyBehavior.ApplyKnockBack(direction, knockbackForce);
            }
        }
        else
        {
            Instantiate(defaultHitFXPrefab, other.GetContact(0).point, Quaternion.identity);
        }

        Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();
        if (otherRb != null)
        {
            Vector3 direction = -(other.transform.position - transform.position).normalized;
            otherRb.AddForce(direction * knockbackForce, ForceMode.Impulse);
        }

        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
