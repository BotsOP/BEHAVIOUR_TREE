using UnityEngine;

namespace Behaviour_tree.Decorator_Nodes
{
    public class BTRunningToFailed : BTBaseNode
    {
        private BTBaseNode node;
        public BTRunningToFailed(BTBaseNode _node)
        {
            node = _node;
        }
        public override TaskStatus Run()
        {
            TaskStatus result = node.Run();
            switch (result)
            {
                case TaskStatus.Failed:
                    return TaskStatus.Failed;
                case TaskStatus.Success:
                    Debug.Log("in range");
                    return TaskStatus.Success;
                case TaskStatus.Running: 
                    return TaskStatus.Failed;
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
}
