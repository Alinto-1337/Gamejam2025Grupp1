using UnityEngine;
using UnityEngine.InputSystem;

public class WorldSpaceGursor : Singleton<WorldSpaceGursor>
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200, LayerMask.GetMask("ShootingPlane")))
        {
            transform.position = hit.point;
        }
    }
}
