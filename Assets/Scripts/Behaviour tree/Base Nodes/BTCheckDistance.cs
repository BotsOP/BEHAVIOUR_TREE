using UnityEngine;

namespace Behaviour_tree.Base_Nodes
{
    public class BTCheckDistance : BTBaseNode
    {
        private Transform pos1;
        private Transform pos2;
        private float distance;
        public BTCheckDistance(float _distance, Transform _pos2, Transform _pos1)
        {
            distance = _distance;
            pos2 = _pos2;
            pos1 = _pos1;
        }
        public override TaskStatus Run()
        {
            if(Vector3.Distance(pos1.position, pos2.position) < distance)
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