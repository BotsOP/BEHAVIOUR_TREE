using UnityEngine;

namespace Behaviour_tree.Base_Nodes
{
    public class BTLookAt : BTBaseNode
    {
        private Transform transform;
        private Transform lookAtTransform;
        private float time;
        private float incrementalTime;
        private float offsetDegress = 0;

        private Vector3 lookAt;
        private Vector3 pos;
        private Vector3 lookDir;
        public BTLookAt(Transform _transform, Transform _lookAtTransform, float _time, float _offsetDegress)
        {
            transform = _transform;
            lookAtTransform = _lookAtTransform;
            time = _time;
            offsetDegress = _offsetDegress;
        }
        
        public BTLookAt(Transform _transform, Transform _lookAtTransform, float _time)
        {
            transform = _transform;
            lookAtTransform = _lookAtTransform;
            time = _time;
        }
        public override TaskStatus Run()
        {
            Debug.Log("turning");
            Vector3 currentDir = transform.rotation * Vector3.forward;
            currentDir = Quaternion.Euler(0, incrementalTime, 0) * currentDir;
            transform.rotation = Quaternion.LookRotation(currentDir, Vector3.up);
            
            if (Vector3.Dot(lookDir.normalized, (transform.rotation * Vector3.forward).normalized) > 0.95f)
            {
                transform.rotation = Quaternion.LookRotation(lookAtTransform.position - transform.position, Vector3.up);
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }
        public override void OnEnter()
        {
            lookAt = new Vector3(lookAtTransform.position.x, 0, lookAtTransform.position.z);
            pos = new Vector3(transform.position.x, 0, transform.position.z);
            lookDir = lookAt - pos;
            lookDir = Quaternion.Euler(0, 0, 0) * lookDir;

            //Debug.Log(Vector3.Angle(transform.rotation * Vector3.forward, lookDir));
            incrementalTime = Vector3.Angle(transform.rotation * Vector3.forward, lookDir) / 50 * time;
        }
        public override void OnExit()
        {
        }
    }
}