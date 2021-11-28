using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSelector : BTBaseNode
{
    private int currentIndex = 0;
    private BTBaseNode[] nodes;
    private bool startNode = true;
    public BTSelector(params BTBaseNode[] inputNodes)
    {
        nodes = inputNodes;
    }

    public override TaskStatus Run()
    {
        for (; currentIndex < nodes.Length; currentIndex++)
        {
            if (startNode)
            {
                nodes[currentIndex].OnEnter();
                startNode = false;
            }
            TaskStatus result = nodes[currentIndex].Run();
            switch (result)
            {
                case TaskStatus.Failed:
                    startNode = true;
                    continue;
                case TaskStatus.Success:
                    currentIndex = 0;
                    startNode = true;
                    return TaskStatus.Success;
                case TaskStatus.Running: return TaskStatus.Running;
            }
        }
        currentIndex = 0;
        return TaskStatus.Failed;

    }
    public override void OnEnter()
    {
        nodes[currentIndex].OnEnter();
    }
    public override void OnExit()
    {
        
    }
}
