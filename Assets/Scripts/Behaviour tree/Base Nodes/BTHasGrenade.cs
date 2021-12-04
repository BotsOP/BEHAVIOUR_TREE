namespace Behaviour_tree.Base_Nodes
{
    public class BTHasGrenade : BTBaseNode
    {
        private float maxGrenades;
        private float amountGrenades;
        
        public BTHasGrenade(float _maxGrenades)
        {
            maxGrenades = _maxGrenades;
        }
        public override TaskStatus Run()
        {
            if (amountGrenades >= maxGrenades)
            {
                return TaskStatus.Failed;
            }
            amountGrenades++;
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