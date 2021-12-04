using UnityEngine;

namespace Behaviour_tree.Base_Nodes
{
    public class BTParentObject : BTBaseNode
    {
        private GameObject gameObject;
        private Transform newParent;
        public BTParentObject(GameObject _gameObject, Transform _newParent)
        {
            newParent = _newParent;
            gameObject = _gameObject;

        }
        public override TaskStatus Run()
        {
            gameObject.transform.SetParent(newParent);
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