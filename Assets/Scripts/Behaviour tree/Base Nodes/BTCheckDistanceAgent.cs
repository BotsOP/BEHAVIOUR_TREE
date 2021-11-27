using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTCheckDistanceAgent : BTBaseNode
{
    private NavMeshAgent agent;
    private float distance;
    public BTCheckDistanceAgent(NavMeshAgent _agent, float _distance)
    {
        agent = _agent;
        distance = _distance;
    }
    public override TaskStatus Run()
    {
        if (agent.remainingDistance < distance && !agent.pathPending)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
    public override void OnEnter()
    {
        
    }
}
