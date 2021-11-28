using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TaskStatus { Success, Failed, Running }

public abstract class BTBaseNode
{
    public abstract TaskStatus Run();
    public abstract void OnEnter();
    public abstract void OnExit();
}