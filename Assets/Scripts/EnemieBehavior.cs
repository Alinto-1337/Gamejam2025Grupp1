using UnityEngine;
using UnityEngine.AI;

public class EnemieBehavior : MonoBehaviour
{
    [SerializeField] Transform Target;

    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {

    }


    void Update()
    {
        agent.SetDestination(Target.position);
    }
}
