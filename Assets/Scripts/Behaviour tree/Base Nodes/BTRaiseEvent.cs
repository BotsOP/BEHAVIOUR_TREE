namespace Behaviour_tree.Base_Nodes
{
    public class BTRaiseEvent : BTBaseNode
    {
        private EventType evt;
        public BTRaiseEvent(EventType _evt)
        {
            evt = _evt;
        }
        public override TaskStatus Run()
        {
            EventSystem.RaiseEvent(evt);
            return TaskStatus.Success;
        }
        public override void OnEnter()
        {
        }
        public override void OnExit()
        {
        }
    }
}