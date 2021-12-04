using UnityEngine;

namespace Behaviour_tree.Base_Nodes
{
    public class BTActivateComponent : BTBaseNode
    {
        private GameObject gameObject;
        private object component;
        private bool setActive;
        public BTActivateComponent(GameObject _gameObject, object _component, bool _setActive)
        {
            gameObject = _gameObject;
            component = _component;
            setActive = _setActive;

        }
        public override TaskStatus Run()
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = setActive;
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