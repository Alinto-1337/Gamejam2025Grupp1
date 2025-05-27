using UnityEngine;
using UnityEngine.AI;

public class EnemieBehavior : MonoBehaviour
{
    [SerializeField] Transform Target;

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
}
