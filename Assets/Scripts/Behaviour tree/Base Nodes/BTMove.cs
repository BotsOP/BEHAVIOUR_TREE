using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTMove : BTBaseNode
{
    private Vector3 targetPos;
    private NavMeshAgent agent;
    public BTMove(NavMeshAgent _agent, Vector3 _targetPos)
    {
        targetPos = _targetPos;
        agent = _agent;
    }
    public override TaskStatus Run()
    {
        agent.SetDestination(targetPos);
        return TaskStatus.Success;
    }
    public override void OnEnter()
    {
        
    }
}
