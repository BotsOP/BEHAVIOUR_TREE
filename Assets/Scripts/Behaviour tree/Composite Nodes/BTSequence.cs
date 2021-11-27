using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSequence : BTBaseNode
{
    private int currentIndex = 0;
    private BTBaseNode[] nodes;
    private bool startNode = true;
    public BTSequence(params BTBaseNode[] inputNodes)
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
                    currentIndex = 0;
                    startNode = true;
                    return TaskStatus.Failed;
                case TaskStatus.Success: 
                    startNode = true;
                    continue;
                case TaskStatus.Running: return TaskStatus.Running;
            }
        }
        currentIndex = 0;
        return TaskStatus.Success;

    }
    public override void OnEnter()
    {
        nodes[currentIndex].OnEnter();
    }
}
