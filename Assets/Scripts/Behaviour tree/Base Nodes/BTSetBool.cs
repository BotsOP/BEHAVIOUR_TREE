using UnityEngine;

namespace Behaviour_tree.Base_Nodes
{
    public class BTSetBool : BTBaseNode
    {
        private BlackBoard blackBoard;
        private string valueName;
        private bool value;
        public BTSetBool(BlackBoard _blackBoard, string _valueName, bool _value)
        {
            blackBoard = _blackBoard;
            valueName = _valueName;
            value = _value;
        }

        public override TaskStatus Run()
        {
            blackBoard.SetValue(valueName, value);
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