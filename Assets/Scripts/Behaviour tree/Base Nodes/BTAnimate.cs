using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTAnimate : BTBaseNode
{
    private Animator anim;
    private Vector2 moveDir;
    private float speedLerp = 1;
    
    public BTAnimate(Vector2 _moveDir, Animator _anim, float _speedLerp)
    {
        moveDir = _moveDir;
        anim = _anim;
        speedLerp = _speedLerp;
    }
    public BTAnimate(Vector2 _moveDir, Animator _anim)
    {
        moveDir = _moveDir;
        anim = _anim;
    }
    public override TaskStatus Run()
    {
        Vector2 currentDir = new Vector2(anim.GetFloat("moveX"), anim.GetFloat("moveY"));
        currentDir = Vector2.Lerp(currentDir, moveDir, Time.deltaTime * speedLerp);
        anim.SetFloat("moveX", currentDir.x);
        anim.SetFloat("moveY", currentDir.y);

        if (Vector2.Distance(currentDir, moveDir) < 0.01f)
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
