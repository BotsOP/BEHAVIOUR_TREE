namespace Behaviour_tree.Switch_Nodes
{
    public class BTSwitchNode : BTBaseNode
    {
        private BTBaseNode switchNode;
        private BTBaseNode nodeIfFails;
        private BTBaseNode nodeIfSucceeds;
        public BTSwitchNode(BTBaseNode _switchNode, BTBaseNode _nodeIfFails, BTBaseNode _nodeIfSucceeds)
        {
            switchNode = _switchNode;
            nodeIfFails = _nodeIfFails;
            nodeIfSucceeds = _nodeIfSucceeds;
        }
        public override TaskStatus Run()
        {
            TaskStatus result = switchNode.Run();
            if (result == TaskStatus.Failed)
            {
                nodeIfFails.Run();
                nodeIfSucceeds.OnExit();
            }
            if (result == TaskStatus.Success)
            {
                nodeIfSucceeds.Run();
                nodeIfFails.OnExit();
            }
            return TaskStatus.Success;
        }
        public override void OnEnter()
        {
            throw new System.NotImplementedException();
        }
        public override void OnExit()
        {
            throw new System.NotImplementedException();
        }
    }
}
