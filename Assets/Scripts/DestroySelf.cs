using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    [SerializeField] float lifetime;

    private void Start()
    {
        Invoke(nameof(Death), lifetime);
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
