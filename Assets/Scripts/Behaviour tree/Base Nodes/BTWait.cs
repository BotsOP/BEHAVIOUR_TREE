using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (Time.time - starTime > waitTime)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
    public override void OnEnter()
    {
        starTime = Time.time;
    } 
    public override void OnExit()
    {
        
    }
}
