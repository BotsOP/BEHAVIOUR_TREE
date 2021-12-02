using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class BallLauncher : MonoBehaviour
    {
        public Rigidbody ball;
        public Transform target;

        public float h = 25;
        public float gravity = -18;

        private void Start()
        {
            ball.useGravity = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Launch();
            }
        }

        public void Launch()
        {
            Physics.gravity = Vector3.up * gravity;
            ball.useGravity = true;
            ball.velocity = CalculateLaunchVelocity();
        }

        private Vector3 CalculateLaunchVelocity()
        {
            float displacementY = target.position.y - ball.position.y;
            Debug.Log(displacementY);
            Vector3 displacementXZ = new Vector3(target.position.x - ball.position.x, 0, target.position.z - ball.position.z);

            
            float velocityY = Mathf.Sqrt(-2 * gravity * h);
            float velocityX = displacementXZ.x / (Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity));
            float velocityZ = displacementXZ.z/ (Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity));
            
            Debug.Log(Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity));
            return new Vector3(velocityX,velocityY * -Mathf.Sign(gravity), velocityZ);
        }
    }
}
