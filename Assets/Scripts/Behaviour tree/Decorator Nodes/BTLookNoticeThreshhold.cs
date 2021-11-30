using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLookNoticeThreshhold : BTBaseNode
{
    private BTLook lookNode;
    private BlackBoard blackBoard;
    private float noticeTargetIncrease;
    private float noticeThreshHold;
    public BTLookNoticeThreshhold(BTLook _lookNode, BlackBoard _blackBoard, float _noticeTargetIncrease)
    {
        lookNode = _lookNode;
        blackBoard = _blackBoard;
        noticeTargetIncrease = _noticeTargetIncrease;
        noticeThreshHold = blackBoard.GetValue<float>("noticeThreshhold");
    }
    public override TaskStatus Run()
    {
        TaskStatus result = lookNode.Run();
        float currentNotice = blackBoard.GetValue<float>("currentNotice");
        if (result == TaskStatus.Success)
        {
            float currentNoticeValue = currentNotice + noticeTargetIncrease;
            currentNoticeValue = Mathf.Clamp(currentNoticeValue, 0, noticeThreshHold);
            blackBoard.SetValue("currentNotice", currentNoticeValue);
        }
        if (result == TaskStatus.Failed)
        {
            float currentNoticeValue = currentNotice - noticeTargetIncrease / 5;
            currentNoticeValue = Mathf.Clamp(currentNoticeValue, 0, noticeThreshHold);
            blackBoard.SetValue("currentNotice", currentNoticeValue);
        }
        Debug.Log(currentNotice);

        if (noticeThreshHold == currentNotice)
        {
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
