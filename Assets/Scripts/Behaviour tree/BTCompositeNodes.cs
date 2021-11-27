using System;
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
}

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
