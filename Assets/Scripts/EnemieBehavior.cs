using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemieBehavior : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] int health;

    [Header("Movement")]
    [SerializeField] AnimationCurve SpeedPattern = AnimationCurve.Linear(0, 1, 0, 1);
    [SerializeField] float walkingSpeed = 4f;

    [Header("Effects")]
    [SerializeField] List<GameObject> deathFxPrefabs;

    [Header("Animations")]
    [SerializeField] Animator animator;
    [SerializeField] string walkAnimName;
    [SerializeField] string pushButtonAnimname;

    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        if (animator == null) Debug.LogError($"No animator assigned in enemy script, {name}");

        animator.CrossFade(walkAnimName, 0.2f);


    }

    void Update()
    {
        agent.SetDestination(target.position);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        float t = stateInfo.normalizedTime % 1f;

        agent.speed = walkingSpeed * SpeedPattern.Evaluate(t);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void OnCollisionEnter(Collision other)
    {
        Bullet bullet = other.gameObject.GetComponent<Bullet>();

        if (bullet == null) return;

        Destroy(other.gameObject);

        ApplyDamage(bullet.damage);
    }

    public void ApplyDamage(int _dmg)
    {
        if (_dmg <= 0) return;

        health -= _dmg;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        foreach (GameObject _fx in deathFxPrefabs)
        {
            Instantiate(_fx, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
