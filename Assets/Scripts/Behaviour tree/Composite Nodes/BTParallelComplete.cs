using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTParallelComplete : BTBaseNode
{
    private BTBaseNode[] nodes;
    int currentIndex = 0;
    private bool onEnter = true;
    public BTParallelComplete(params BTBaseNode[] inputNodes)
    {
        nodes = inputNodes;
    }
    public override TaskStatus Run()
    {
        if (onEnter)
        {
            onEnter = false;
            foreach (var node in nodes)
            {
                node.OnEnter();
            }
        }
        
        int successCount = 0;
        for (; currentIndex < nodes.Length; currentIndex++)
        {
            TaskStatus result = nodes[currentIndex].Run();
            switch (result)
            {
                case TaskStatus.Failed:
                    continue;
                case TaskStatus.Success:
                    successCount++;
                    continue;
                case TaskStatus.Running: 
                    continue;
            }
        }

        if (successCount >= nodes.Length)
        {
            Debug.Log(successCount + "   " + nodes.Length);
            currentIndex = 0;
            onEnter = true;
            return TaskStatus.Success;
        }

        currentIndex = 0;
        return TaskStatus.Running;
    }
    public override void OnEnter()
    {
        
    }
}
