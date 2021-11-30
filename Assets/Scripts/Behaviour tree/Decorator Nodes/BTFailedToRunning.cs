namespace Behaviour_tree.Decorator_Nodes
{
    public class BTFailedToRunning : BTBaseNode
    {
        private BTBaseNode node;
        public BTFailedToRunning(BTBaseNode _node)
        { 
            node = _node;
        }
        public override TaskStatus Run()
        {
            TaskStatus result = node.Run();
            switch (result)
            {
                case TaskStatus.Failed:
                    return TaskStatus.Running;
                case TaskStatus.Success:
                    return TaskStatus.Success;
                case TaskStatus.Running: 
                    return TaskStatus.Running;
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
}