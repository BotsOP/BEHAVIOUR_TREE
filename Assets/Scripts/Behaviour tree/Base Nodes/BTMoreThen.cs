using UnityEngine;

namespace Behaviour_tree.Base_Nodes
{
    public class BTMoreThen : BTBaseNode
    {
        private BlackBoard blackBoard;
        private string valueName;
        private float moreThen;

        private float value;
        public BTMoreThen(BlackBoard _blackBoard, string _valueName, float _moreThen)
        {
            blackBoard = _blackBoard;
            valueName = _valueName;
            moreThen = _moreThen;

        }
        public override TaskStatus Run()
        {
            value = blackBoard.GetValue<float>(valueName);
            if (value < moreThen)
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