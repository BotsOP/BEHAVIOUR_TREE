using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLookNoticeThreshhold : BTBaseNode
{
    private BTLook lookNode;
    private BlackBoard blackBoard;
    private float noticeTargetIncrease;
    private float noticeThreshHold;
    private float noticeBuffer;
    public BTLookNoticeThreshhold(BTLook _lookNode, BlackBoard _blackBoard, float _noticeTargetIncrease, float _noticeBuffer)
    {
        lookNode = _lookNode;
        blackBoard = _blackBoard;
        noticeTargetIncrease = _noticeTargetIncrease;
        noticeBuffer = _noticeBuffer;
        noticeThreshHold = blackBoard.GetValue<float>("noticeThreshhold");
    }
    public override TaskStatus Run()
    {
        TaskStatus result = lookNode.Run();
        float currentNotice = blackBoard.GetValue<float>("currentNotice");
        if (result == TaskStatus.Success)
        {
            float currentNoticeValue = currentNotice + noticeTargetIncrease;
            currentNoticeValue = Mathf.Clamp(currentNoticeValue, 0, noticeThreshHold + noticeBuffer);
            blackBoard.SetValue("currentNotice", currentNoticeValue);
        }
        if (result == TaskStatus.Failed)
        {
            float currentNoticeValue = currentNotice - noticeTargetIncrease / 5;
            currentNoticeValue = Mathf.Clamp(currentNoticeValue, 0, noticeThreshHold + noticeBuffer);
            blackBoard.SetValue("currentNotice", currentNoticeValue);
        }

        if (noticeThreshHold <= currentNotice)
        {
            //so enemy doesnt instantly ignores when you quickly go out of his radius
            blackBoard.SetValue("currentNotice", currentNotice + noticeBuffer);
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
