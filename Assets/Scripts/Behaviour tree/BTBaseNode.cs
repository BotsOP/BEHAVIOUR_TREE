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

public class BTAnimate : BTBaseNode
{
    private Animator anim;
    private Vector2 moveDir;
    
    public BTAnimate(Vector2 _moveDir, Animator _anim)
    {
        moveDir = _moveDir;
        anim = _anim;
    }
    public override TaskStatus Run()
    {
        Vector2 currentDir = new Vector2(anim.GetFloat("moveX"), anim.GetFloat("moveY"));
        currentDir = Vector2.Lerp(currentDir, moveDir, Time.deltaTime);
        anim.SetFloat("moveX", currentDir.x);
        anim.SetFloat("moveY", currentDir.y);

        if (Vector2.Distance(currentDir, moveDir) < 0.1f)
        {
            currentDir = moveDir;
            anim.SetFloat("moveX", currentDir.x);
            anim.SetFloat("moveY", currentDir.y);
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
    public override void OnEnter()
    {
        
    }
}

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
        return TaskStatus.Running;
    }
    public override void OnEnter()
    {
        target = blackBoard.GetValue<Transform>("target");
    }
}
public class BTLook : BTBaseNode
{
    private FieldOfView fov;
    private BlackBoard blackBoard;
    public BTLook(GameObject _gameObject, LayerMask _targetMask, LayerMask _obstructionMask, float _radius, float _angle, BlackBoard _blackBoard)
    {
        blackBoard = _blackBoard;
        fov = new FieldOfView(_gameObject, _targetMask, _obstructionMask, _radius, _angle);
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
            //Debug.Log(targetPos);
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
    public override void OnEnter()
    {
        Debug.Log(targetPos);
        agent.SetDestination(targetPos);
    }
}
