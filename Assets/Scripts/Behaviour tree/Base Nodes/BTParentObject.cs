using UnityEngine;

namespace Behaviour_tree.Base_Nodes
{
    public class BTParentObject : BTBaseNode
    {
        private GameObject gameObject;
        private Transform newParent;
        private BlackBoard blackBoard;
        private string valueName;
        public BTParentObject(GameObject _gameObject, Transform _newParent)
        {
            newParent = _newParent;
            gameObject = _gameObject;
        }
        public BTParentObject(BlackBoard _blackBoard, string _valueName, Transform _newParent)
        {
            blackBoard = _blackBoard;
            valueName = _valueName;
            newParent = _newParent;
        }
        public override TaskStatus Run()
        {
            gameObject.transform.SetParent(newParent);
            return TaskStatus.Success;
        }
        public override void OnEnter()
        {
            gameObject = blackBoard.GetValue<GameObject>(valueName);
        }
        public override void OnExit()
        {
            
        }
    }
}