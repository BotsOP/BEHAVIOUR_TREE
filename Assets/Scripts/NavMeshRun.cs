using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshRun : MonoBehaviour {
 
     public Transform player;
     private NavMeshAgent myNMagent;
     private float nextTurnTime;
     private Transform startTransform;
 
     public float multiplyBy;
 
 
     // Use this for initialization
     void Start () {
         myNMagent = GetComponent<NavMeshAgent>();
 
         RunFrom ();
     }
     
     // Update is called once per frame
     void Update () {
         RunFrom ();
     }
 
     public void RunFrom()
     {
         Vector3 dir = (transform.position - player.position).normalized;
         myNMagent.SetDestination(transform.position + dir);
     }
 }
