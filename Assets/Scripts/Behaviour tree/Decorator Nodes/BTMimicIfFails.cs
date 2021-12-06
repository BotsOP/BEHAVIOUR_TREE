namespace Behaviour_tree.Decorator_Nodes
{
    public class BTMimicIfFails : BTBaseNode
    {
        private BTBaseNode nodeToMimic;
        private BTBaseNode mimicNode;
        public BTMimicIfFails(BTBaseNode _nodeToMimic, BTBaseNode _mimicNode)
        {
            nodeToMimic = _nodeToMimic;
            mimicNode = _mimicNode;
        }
        public override TaskStatus Run()
        {
            TaskStatus resultNodeToMimic = nodeToMimic.Run();
            TaskStatus resultMimicNode = mimicNode.Run();
            if (resultNodeToMimic == TaskStatus.Success)
            {
                return TaskStatus.Success;
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
