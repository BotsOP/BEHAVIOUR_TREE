using UnityEngine;

namespace Behaviour_tree.Base_Nodes
{
    public class BTHasGrenade : BTBaseNode
    {
        private BlackBoard blackBoard;
        private string valueName;
        
        public BTHasGrenade(BlackBoard _blackBoard, string _valueName)
        {
            blackBoard = _blackBoard;
            valueName = _valueName;
            blackBoard.SetValue(valueName, 0f);
        }
        public override TaskStatus Run()
        {
            float amountGrenades = blackBoard.GetValue<float>(valueName);
            amountGrenades++;
            blackBoard.SetValue(valueName, amountGrenades);
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