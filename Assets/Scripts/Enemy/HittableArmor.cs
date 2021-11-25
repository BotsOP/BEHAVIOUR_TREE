using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HittableArmor : MonoBehaviour, IDamagable
{
    private bool hasBeenhit;
    private MeshCollider ms;
    private Rigidbody rb;
    private Vector3 randomRot;

    private void Awake()
    {
        ms = gameObject.GetComponent<MeshCollider>();
    }
    public void TakeDamage(float damage, GameObject attacker)
    {
        if (!hasBeenhit)
        {
            Debug.Log("hit armor");
            transform.SetParent(null);
            rb = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
            
            ms.enabled = false;
            randomRot = new Vector3(Random.Range(-10,10), Random.Range(-10,10), Random.Range(-10,10));
            StartCoroutine("enableMS");

            Vector3 impact = transform.position - attacker.transform.position;
            impact = impact.normalized;
            impact = new Vector3(impact.x, impact.y * 1.5f, impact.z);
            rb.AddForce(impact * 15, ForceMode.VelocityChange);

            hasBeenhit = true;
        }
    }

    private void FixedUpdate()
    {
        if (!ms.enabled)
        {
            rb.AddTorque(randomRot * 100);
        }
    }

    private IEnumerator enableMS()
    {
        yield return new WaitForSeconds(0.1f);
        ms.enabled = true;
    }
}
