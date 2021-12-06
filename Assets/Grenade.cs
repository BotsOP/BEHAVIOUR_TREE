using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject smokePrefab;
    public float smokeExpansionRate;
    private bool exploded;
    private bool doneExpansion;
    private GameObject smoke;
    private Vector3 expansion;
    // Start is called before the first frame update
    void Start()
    {
        expansion = new Vector3(smokeExpansionRate, smokeExpansionRate, smokeExpansionRate);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y <= 0 && !exploded)
        {
            exploded = true;
            smoke = Instantiate(smokePrefab, transform.position, Quaternion.identity);
            smoke.transform.localScale = Vector3.zero;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        if (exploded && !doneExpansion)
        {
            if (smoke.transform.localScale.x >= 1)
            {
                doneExpansion = true;
            }
            smoke.transform.localScale += expansion;
        }
    }
}
