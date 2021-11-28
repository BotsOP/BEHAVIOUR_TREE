using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTMove : BTBaseNode
{
    private Transform targetTransform;
    private NavMeshAgent agent;
    public BTMove(NavMeshAgent _agent, Transform _targetTransform)
    {
        agent = _agent;
        targetTransform = _targetTransform;
    }
    public override TaskStatus Run()
    {
        agent.SetDestination(targetTransform.position);
        return TaskStatus.Success;
    }
    public override void OnEnter()
    {
        
    }
}
