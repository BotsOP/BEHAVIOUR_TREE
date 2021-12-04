using UnityEngine;

namespace Behaviour_tree.Base_Nodes
{
    public class BTThrow : BTBaseNode
    {
        private Rigidbody objectToThrow;
        private Transform target;

        private float h = 2;
        private float gravity = -18;
        
        public BTThrow(Rigidbody _objectToThrow, Transform _target)
        {
            objectToThrow = _objectToThrow;
            target = _target;
        }
        public override TaskStatus Run()
        {
            Physics.gravity = Vector3.up * gravity;
            objectToThrow.useGravity = true;
            objectToThrow.velocity = CalculateLaunchVelocity();

            return TaskStatus.Success;
        }
        public override void OnEnter()
        {
        }
        public override void OnExit()
        {
        }
        
        private Vector3 CalculateLaunchVelocity()
        {
            float displacementY = target.position.y - objectToThrow.position.y;
            Vector3 displacementXZ = new Vector3(target.position.x - objectToThrow.position.x, 0, target.position.z - objectToThrow.position.z);

            float velocityY = Mathf.Sqrt(-2 * gravity * h);
            float velocityX = displacementXZ.x / (Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity));
            float velocityZ = displacementXZ.z/ (Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity));
            
            return new Vector3(velocityX,velocityY * -Mathf.Sign(gravity), velocityZ);
        }
    }
}