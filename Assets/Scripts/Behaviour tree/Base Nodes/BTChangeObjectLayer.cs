using UnityEngine;

namespace Behaviour_tree.Base_Nodes
{
    public class BTChangeObjectLayer : BTBaseNode
    {
        private GameObject objectToChange;
        private int layerInt;
        public BTChangeObjectLayer(GameObject _objectToChange, int _layerInt)
        {
            objectToChange = _objectToChange;
            layerInt = _layerInt;

        }
        public override TaskStatus Run()
        {
            objectToChange.layer = layerInt;
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
