using UnityEngine;

namespace Behaviour_tree.Switch_Nodes
{
    public class BTFailIfBool : BTBaseNode
    {
        private bool boolean;
        private BlackBoard blackBoard;
        private string boolName;
        public BTFailIfBool(BlackBoard _blackBoard, string _boolName)
        {
            blackBoard = _blackBoard;
            boolName = _boolName;
        }
        public override TaskStatus Run()
        {
            boolean = blackBoard.GetValue<bool>(boolName);
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