using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView
{
    private float radius = 100f;
    private float angle = 130;
    
    public bool canSeeTarget;
    public Transform target;
    public List<Transform> targetList = new List<Transform>();

    private GameObject gameObject;
    private int frameCount;
    private LayerMask targetMask;
    private LayerMask obstructionMask;

    private bool returnList;

    public FieldOfView(GameObject _gameObject, LayerMask _targetMask, LayerMask _obstructionMask, float _radius, float _angle)
    {
        gameObject = _gameObject;
        targetMask = _targetMask;
        obstructionMask = _obstructionMask;
        radius = _radius;
        angle = _angle;
    }
    
    public FieldOfView(GameObject _gameObject, LayerMask _targetMask, float _radius, float _angle)
    {
        gameObject = _gameObject;
        targetMask = _targetMask;
        radius = _radius;
        angle = _angle;
    }
    
    public FieldOfView(GameObject _gameObject, LayerMask _targetMask, LayerMask _obstructionMask, float _radius, float _angle, bool _returnList)
    {
        gameObject = _gameObject;
        targetMask = _targetMask;
        obstructionMask = _obstructionMask;
        radius = _radius;
        angle = _angle;
        returnList = _returnList;
    }
    
    public FieldOfView(GameObject _gameObject, LayerMask _targetMask, float _radius, float _angle, bool _returnList)
    {
        gameObject = _gameObject;
        targetMask = _targetMask;
        radius = _radius;
        angle = _angle;
        returnList = _returnList;
    }

    public void Update()
    {
        if(frameCount % 20 == 0)
        {
            if (!returnList)
            {
                FieldofViewCheck();
            }
            else
            {
                FieldofViewCheckList();
            }
        }
        frameCount++;
    }

    private void FieldofViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(gameObject.transform.position, radius, targetMask);

        //If there are multiple targets change this into a foreach loop
        if (rangeChecks.Length != 0)
        {
            target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - gameObject.transform.position).normalized;

            if (Vector3.Angle(gameObject.transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(gameObject.transform.position, target.position);

                if (!Physics.Raycast(gameObject.transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeeTarget = true;
                }
                else
                {
                    canSeeTarget = false;
                }
            }
            else
            {
                canSeeTarget = false;
            }
        }
        else if (canSeeTarget)
        {
            canSeeTarget = false;
        }
    }
    
    private void FieldofViewCheckList()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(gameObject.transform.position, radius, targetMask);

        //If there are multiple targets change this into a foreach loop
        if (rangeChecks.Length != 0)
        {
            foreach (var collider in rangeChecks)
            {
                Transform target = collider.transform;
                Vector3 directionToTarget = (target.position - gameObject.transform.position).normalized;

                if (Vector3.Angle(gameObject.transform.forward, directionToTarget) < angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(gameObject.transform.position, target.position);

                    if (!Physics.Raycast(gameObject.transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    {
                        targetList.Add(target);
                        canSeeTarget = true;
                    }
                    else
                    {
                        targetList.Remove(target);
                        canSeeTarget = false;
                    }
                }
                else
                {
                    targetList.Remove(target);
                    canSeeTarget = false;
                }
            }
        }
        else if (canSeeTarget)
        {
            targetList.Remove(target);
            canSeeTarget = false;
        }
    }
}

