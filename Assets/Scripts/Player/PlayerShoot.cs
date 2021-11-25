using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public float damage = 10f;
    private Camera camera;
    void Awake()
    {
        camera = GetComponentInChildren<Camera>();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.gameObject.GetComponent<IDamagable>() != null)
                {
                    hit.collider.gameObject.GetComponent<IDamagable>().TakeDamage(damage, gameObject);
                }
            }
        }
    }
}
