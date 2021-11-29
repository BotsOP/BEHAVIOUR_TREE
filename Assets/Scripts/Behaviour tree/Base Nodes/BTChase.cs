using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTChase : BTBaseNode
{
    private BlackBoard blackBoard;
    private NavMeshAgent agent;
    private GameObject gameObject;
    private Transform target;
    private float inRange;
    private float stoppingDistance;
    public BTChase(BlackBoard _blackBoard, float _inRange, float _stoppingDistance)
    {
        blackBoard = _blackBoard;
        agent = blackBoard.GetValue<NavMeshAgent>("agent");
        gameObject = blackBoard.GetValue<GameObject>("gameObject");
        
        inRange = _inRange;
        stoppingDistance = _stoppingDistance;
    }
    public override TaskStatus Run()
    {
        Vector3 targetPos = target.position;
        Vector3 lookAt = new Vector3(targetPos.x, gameObject.transform.position.y, targetPos.z);
        gameObject.transform.LookAt(lookAt);
        
        agent.SetDestination(targetPos);
        
        if (agent.remainingDistance < inRange && !agent.pathPending)
        {
            return TaskStatus.Success;
        }
        if (agent.remainingDistance > stoppingDistance && !agent.pathPending)
        {
            return TaskStatus.Failed;
        }
        //Debug.Log((blackBoard.GetValue<GameObject>("gameObject").transform.position - targetPos).normalized);
        Debug.Log(agent.velocity);
        return TaskStatus.Running;
    }
    public override void OnEnter()
    {
        target = blackBoard.GetValue<Transform>("target");
    }
    public override void OnExit()
    {
        
    }
}
