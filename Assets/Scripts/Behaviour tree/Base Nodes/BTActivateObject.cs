using UnityEngine;

namespace Behaviour_tree.Base_Nodes
{
    public class BTActivateObject : BTBaseNode
    {
        private GameObject gameObject;
        private bool setActive;
        public BTActivateObject(GameObject _gameObject, bool _setActive)
        {
            gameObject = _gameObject;
            setActive = _setActive;

        }
        public override TaskStatus Run()
        {
            gameObject.SetActive(setActive);
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