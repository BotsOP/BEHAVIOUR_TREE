using UnityEngine;

namespace Behaviour_tree.Decorator_Nodes
{
    public class BTMimicIfSucceeds :BTBaseNode
    {
        private BTBaseNode nodeToMimic;
        private BTBaseNode mimicNode;
        public BTMimicIfSucceeds(BTBaseNode _nodeToMimic, BTBaseNode _mimicNode)
        {
            nodeToMimic = _nodeToMimic;
            mimicNode = _mimicNode;
        }
        public override TaskStatus Run()
        {
            TaskStatus resultNodeToMimic = nodeToMimic.Run();
            TaskStatus resultMimicNode = mimicNode.Run();
            Debug.Log(resultNodeToMimic + "   " + resultMimicNode);
            if (resultNodeToMimic == TaskStatus.Failed)
            {
                return TaskStatus.Failed;
            }
            return resultMimicNode;
        }
        public override void OnEnter()
        {
        }
        public override void OnExit()
        {
        }
    }
}
