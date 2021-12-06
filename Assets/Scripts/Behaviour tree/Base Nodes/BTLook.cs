using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLook : BTBaseNode
{
    private FieldOfView fov;
    private BlackBoard blackBoard;
    private string valueName;
    private bool returnList;
    public BTLook(GameObject _gameObject, LayerMask _targetMask, LayerMask _obstructionMask, float _radius, float _angle, BlackBoard _blackBoard, string _valueName)
    {
        blackBoard = _blackBoard;
        valueName = _valueName;

        fov = new FieldOfView(_gameObject, _targetMask, _obstructionMask, _radius, _angle);
    }
    
    public BTLook(GameObject _gameObject, LayerMask _targetMask, float _radius, float _angle, BlackBoard _blackBoard, string _valueName)
    {
        blackBoard = _blackBoard;
        valueName = _valueName;

        fov = new FieldOfView(_gameObject, _targetMask, _radius, _angle);
    }
    
    public BTLook(GameObject _gameObject, LayerMask _targetMask, float _radius, float _angle, BlackBoard _blackBoard, string _valueName, bool _returnList)
    {
        blackBoard = _blackBoard;
        valueName = _valueName;
        returnList = _returnList;

        fov = new FieldOfView(_gameObject, _targetMask, _radius, _angle, returnList);
    }
    
    public override TaskStatus Run()
    {
        fov.Update();
        
        if (fov.canSeeTarget)
        {
            if (returnList)
            {
                blackBoard.SetValue(valueName, fov.targetList);
            }
            else
            {
                blackBoard.SetValue(valueName, fov.target);
            }
            return TaskStatus.Success;
        }
        return TaskStatus.Failed;
    }
    public override void OnEnter()
    {
        
    }
    public override void OnExit()
    {
        
    }
}
