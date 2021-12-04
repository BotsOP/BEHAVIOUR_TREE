using UnityEngine;

namespace Behaviour_tree.Base_Nodes
{
    public class BTThrow : BTBaseNode
    {
        private Rigidbody objectToThrow;
        private Transform target;
        private BlackBoard blackBoard;
        private string valueName;

        private float h = 2;
        private float gravity = -18;
        
        public BTThrow(GameObject _objectToThrow, Transform _target)
        {
            objectToThrow = _objectToThrow.GetComponent<Rigidbody>();
            target = _target;
        }
        public BTThrow(BlackBoard _blackBoard, string _valueName, Transform _target)
        {
            blackBoard = _blackBoard;
            valueName = _valueName;
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
            if (valueName != null)
            {
                objectToThrow = blackBoard.GetValue<GameObject>(valueName).GetComponent<Rigidbody>();
            }
        }
        public override void OnExit()
        {
        }
        
        private Vector3 CalculateLaunchVelocity()
        {
            float displacementY = target.position.y - objectToThrow.position.y;
            Vector3 displacementXZ = new Vector3(target.position.x - objectToThrow.gameObject.transform.position.x, 0, target.position.z - objectToThrow.gameObject.transform.position.z);

            float velocityY = Mathf.Sqrt(-2 * gravity * h);
            float velocityX = displacementXZ.x / (Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity));
            float velocityZ = displacementXZ.z/ (Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity));
            
            return new Vector3(velocityX,velocityY * -Mathf.Sign(gravity), velocityZ);
        }
    }
}