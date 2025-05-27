using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemieBehavior : MonoBehaviour
{
    [SerializeField] Transform Target;
    [SerializeField] int health;

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
        agent.SetDestination(Target.position);
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
            Instantiate(_fx);
        }
        Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
