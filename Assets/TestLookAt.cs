using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLookAt : MonoBehaviour
{
    public Transform lookAt;
    public float time;
    private float incrementalAngle;
    void Start()
    {
        incrementalAngle = Vector3.Angle(transform.rotation * Vector3.forward, lookAt.position - transform.position) / 50 * time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time < 1)
        {
            Vector3 currentDir = transform.rotation * Vector3.forward;
            currentDir = Quaternion.Euler(0, incrementalAngle, 0) * currentDir;
            transform.rotation = Quaternion.LookRotation(currentDir, Vector3.up);
        }
    }
}
