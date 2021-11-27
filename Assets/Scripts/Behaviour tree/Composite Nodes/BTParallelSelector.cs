using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTParallelSelector : BTBaseNode
{
    private BTBaseNode[] nodes;
    int currentIndex = 0;
    public BTParallelSelector(params BTBaseNode[] inputNodes)
    {
        nodes = inputNodes;
    }
    public override TaskStatus Run()
    {
        for (; currentIndex < nodes.Length; currentIndex++)
        {
            TaskStatus result = nodes[currentIndex].Run();
            switch (result)
            {
                case TaskStatus.Failed:
                    continue;
                case TaskStatus.Success:
                    currentIndex = 0;
                    Debug.Log("found succes");
                    return TaskStatus.Success;
                case TaskStatus.Running: 
                    continue;
            }
        }

        currentIndex = 0;
        return TaskStatus.Running;
    }
    public override void OnEnter()
    {
        throw new System.NotImplementedException();
    }
}
