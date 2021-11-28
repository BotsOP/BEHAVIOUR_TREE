using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTurn : BTBaseNode
{
    private Transform transform;
    private Transform targetTransform;
    private float lerpValue;
    private float turnTime;
    private float currentLerp;
    public BTTurn(Transform _transform, Transform _targetTransform, float _turnTime)
    {
        transform = _transform;
        targetTransform = _targetTransform;
        turnTime = _turnTime;
    }
    public override TaskStatus Run()
    {
        currentLerp = currentLerp + lerpValue;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetTransform.rotation, currentLerp);

            Debug.Log(currentLerp);
        if (currentLerp >= 1)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }

    public override void OnEnter()
    {
        currentLerp = 0;
        lerpValue = 0.02f / turnTime;
    }
}
