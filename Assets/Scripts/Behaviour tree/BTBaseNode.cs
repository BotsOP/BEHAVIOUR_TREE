using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TaskStatus { Success, Failed, Running }

public abstract class BTBaseNode
{
    public abstract TaskStatus Run();
    public abstract void OnEnter();
}
public class BTLook : BTBaseNode
{
    private FieldOfView fov;
    private BlackBoard blackBoard;
    public BTLook(GameObject _gameObject, LayerMask _targetMask, LayerMask _obstructionMask, float radius, float angle, BlackBoard _blackBoard)
    {
        blackBoard = _blackBoard;
        fov = new FieldOfView(_gameObject, _targetMask, _obstructionMask, radius, angle);
    }
    
    public override TaskStatus Run()
    {
        fov.Update();
        if (fov.canSeeTarget)
        {
            Debug.Log("seeing target!!!!!!!!!!!!!!");
            blackBoard.SetValue("target", fov.target);
            return TaskStatus.Success;
        }
        return TaskStatus.Failed;
    }
    public override void OnEnter()
    {
        
    }
}

public class BTWait : BTBaseNode
{
    private float waitTime;
    private float starTime;
    public BTWait(float _waitTime)
    {
        waitTime = _waitTime;
    }
    public override TaskStatus Run()
    {
        //Debug.Log(Time.time + "   " + starTime + "   " + waitTime);
        if (Time.time - starTime > waitTime)
        {
            Debug.Log("done waiting");
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
    public override void OnEnter()
    {
        starTime = Time.time;
    }
}

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
        if (agent.remainingDistance < 0.1f && !agent.pathPending)
        {
            Debug.Log("reached pos");
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
    public override void OnEnter()
    {
        //Debug.Log(targetPos);
        agent.SetDestination(targetPos);
    }
}
