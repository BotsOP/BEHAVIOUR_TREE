namespace Behaviour_tree.Decorator_Nodes
{
    public class BTInvert : BTBaseNode
    {
        private BTBaseNode node;
        public BTInvert(BTBaseNode _node)
        {
            node = _node;
        }
        public override TaskStatus Run()
        {
            TaskStatus result = node.Run();
            switch (result)
            {
                case TaskStatus.Failed:
                    return TaskStatus.Success;
                case TaskStatus.Success:
                    return TaskStatus.Failed;
                case TaskStatus.Running: 
                    return TaskStatus.Running;
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
