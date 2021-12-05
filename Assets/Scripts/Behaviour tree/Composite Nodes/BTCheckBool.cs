using UnityEngine;

namespace DefaultNamespace.Behaviour_tree.Composite_Nodes
{
    public class BTCheckBool : BTBaseNode
    {
        private BlackBoard blackBoard;
        private string valueName;
        public BTCheckBool(BlackBoard _blackBoard, string _valueName)
        {
            blackBoard = _blackBoard;
            valueName = _valueName;
        }

        public override TaskStatus Run()
        {
            bool boolean = blackBoard.GetValue<bool>(valueName);
            if (boolean)
            {
                return TaskStatus.Success;
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