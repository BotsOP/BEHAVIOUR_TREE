using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLookAround : BTBaseNode
{
    private Animator anim;
    private float lookSpeed = 0.01f;
    private float startTime;
    private float timeForLooking = 2f;
    private bool hasTurnedhead;
    private bool turnedHead;
    public BTLookAround(Animator _anim)
    {
        anim = _anim;
    }
    public override TaskStatus Run()
    {
        float lookX = anim.GetFloat("lookX");
        if (lookX > -1 && !turnedHead)
        {
            float currentX = lookX - lookSpeed;
            anim.SetFloat("lookX", currentX);
            startTime = Time.time;
        }
        else
        {
            turnedHead = true;
            if (Time.time - startTime > timeForLooking && !hasTurnedhead)
            {
                float currentX = lookX + lookSpeed * 1.5f;
                anim.SetFloat("lookX", currentX);
                
                if (lookX > 1 && !hasTurnedhead)
                {
                    startTime = Time.time;
                    hasTurnedhead = true;
                }
            }
            
            //Debug.Log(Time.time + "   " + startTime + "  " + hasTurnedhead);
            if (Time.time - startTime > timeForLooking && hasTurnedhead)
            {
                float currentX = lookX - lookSpeed;
                anim.SetFloat("lookX", currentX);
                
                if (lookX < 0)
                {
                    //import look center and have that as starting anim in blend tree
                    //anim.SetLayerWeight(1, 0);
                    return TaskStatus.Success;
                }
            }
        }

        return TaskStatus.Running;
    }
    public override void OnEnter()
    {
        anim.SetLayerWeight(1, 1);
        turnedHead = false;
        hasTurnedhead = false;
        startTime = 0;
    }
}
