using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField] private List<Transform> targets = new List<Transform>();

    Vector2 shakeOffset; // set by the Effect manager.

    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, 0);



    private void LateUpdate()
    {
        if (targets == null || targets.Count == 0)
            return;

        Vector3 midpoint = GetMidpoint();
        Vector3 desiredPosition = midpoint + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime) + (Vector3)shakeOffset;
    }

    private Vector3 GetMidpoint()
    {
        if (targets.Count == 1)
            return targets[0].position;

        Vector3 sum = Vector3.zero;
        foreach (var t in targets)
            sum += t.position;
        return sum / targets.Count;
    }

    // Optional: Call this from your Effect manager to set shake offset
    public void SetShakeOffset(Vector2 offset)
    {
        shakeOffset = offset;
    }

    

}
