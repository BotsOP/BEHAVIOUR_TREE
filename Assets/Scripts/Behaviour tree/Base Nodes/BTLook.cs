using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLook : BTBaseNode
{
    private FieldOfView fov;
    private BlackBoard blackBoard;
    private string valueName;
    public BTLook(GameObject _gameObject, LayerMask _targetMask, LayerMask _obstructionMask, float _radius, float _angle, BlackBoard _blackBoard, string _valueName)
    {
        blackBoard = _blackBoard;
        valueName = _valueName;

        fov = new FieldOfView(_gameObject, _targetMask, _obstructionMask, _radius, _angle);
    }
    
    public override TaskStatus Run()
    {
        
        
        fov.Update();
        if (fov.canSeeTarget)
        {
            blackBoard.SetValue(valueName, fov.target);
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
