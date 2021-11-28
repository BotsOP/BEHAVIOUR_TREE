using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTParallelSelector : BTBaseNode
{
    private BTBaseNode[] nodes;
    int currentIndex = 0;
    private bool exitOut;
    public BTParallelSelector(params BTBaseNode[] inputNodes)
    {
        nodes = inputNodes;
    }
    public override TaskStatus Run()
    {
        for (; currentIndex < nodes.Length; currentIndex++)
        {
            TaskStatus result = TaskStatus.Running;
            if (!exitOut)
            {
                result = nodes[currentIndex].Run();
            }
            else
            {
                nodes[currentIndex].OnExit();
            }
            switch (result)
            {
                case TaskStatus.Failed:
                    continue;
                case TaskStatus.Success:
                    currentIndex = 0;
                    exitOut = true;
                    continue;
                case TaskStatus.Running: 
                    continue;
            }
        }
        
        currentIndex = 0;
        if (exitOut)
        {
            exitOut = false;
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
    public override void OnEnter()
    {
        
    }
    public override void OnExit()
    {
        
    }
}
