using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BombEnemyBehaviour : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] int health;

    [Header("Movement")]
    [SerializeField] AnimationCurve SpeedPattern = AnimationCurve.Linear(0, 1, 0, 1);
    [SerializeField] float walkingSpeed = 4f;

    [Header("Effects")]
    [SerializeField] List<GameObject> deathFxPrefabs;
    [SerializeField] public GameObject bulletHitFxPrefab;

    [Header("Animations")]
    [SerializeField] Animator animator;
    [SerializeField] string walkAnimName;
    [SerializeField] string jumpAnimName;

    bool active = true;

    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void OnEnable()
    {
        if (animator == null) Debug.LogError($"No animator assigned in enemy script, {name}");

        animator.CrossFade(walkAnimName, 0.2f);
    }

    void Update()
    {
        if (agent.enabled) { agent.SetDestination(target.position); }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        float t = stateInfo.normalizedTime % 1f;

        agent.speed = walkingSpeed * SpeedPattern.Evaluate(t);

        if (Vector3.Distance(transform.position, agent.destination) < 5 && active)
        {
            active = false;

            agent.enabled = false;

            transform.LookAt(target.position);

            animator.CrossFade(jumpAnimName, 0.1f);

            Invoke(nameof(Freeze), 0.2f);
            Invoke(nameof(Die), 0.2f);
        }
    }

    public void ApplyKnockBack(Vector3 direction, float force)
    {
        agent.velocity = direction * force;
    }

    public void SetTarget(Transform target)
    {
        if (!agent.enabled) { Debug.LogError($"Agent is disabled, {name}"); }

        this.target = target;
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

    void Freeze()
    {
        target.GetComponent<PlayerMovement>().StartFreeze(1);
        target.GetComponent<Gun>().StartFreeze(1);
    }

    private void Die()
    {
        foreach (GameObject _fx in deathFxPrefabs)
        {
            Instantiate(_fx, transform.position, transform.rotation);
        }
        Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
