using UnityEditor;
using UnityEngine;

namespace DefaultNamespace.Behaviour_tree
{
    public class BTIsAgainstWall : BTBaseNode
    {
        private BlackBoard blackBoard;
        private float distanceToWall;
        private Transform transform;
        public BTIsAgainstWall(BlackBoard _blackBoard, float _distanceToWall, Transform _transform)
        {
            blackBoard = _blackBoard;
            distanceToWall = _distanceToWall;
            transform = _transform;
        }
        public override TaskStatus Run()
        {
            Transform cover = blackBoard.GetValue<Transform>("targetCover");
            CoverSpotsGenerator coverScript = cover.GetComponent<CoverSpotsGenerator>();
            for (int i = 0; i < 4; i++)
            {
                int secondVert = (i + 1) % 4;
                if (DistancePointLine(transform.position, coverScript.worldVerts[i], coverScript.worldVerts[secondVert]) < distanceToWall)
                {
                    return TaskStatus.Success;
                }
            }
            return TaskStatus.Failed;
        }
        public override void OnEnter()
        {
        }
        public override void OnExit()
        {
        }
        
        //code from HandleUtility
        public static Vector3 ProjectPointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
        {
            Vector3 relativePoint = point - lineStart;
            Vector3 lineDirection = lineEnd - lineStart;
            float length = lineDirection.magnitude;
            Vector3 normalizedLineDirection = lineDirection;
            if (length > .000001f)
                normalizedLineDirection /= length;

            float dot = Vector3.Dot(normalizedLineDirection, relativePoint);
            dot = Mathf.Clamp(dot, 0.0F, length);

            return lineStart + normalizedLineDirection * dot;
        }

        // Calculate distance between a point and a line.
        public static float DistancePointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
        {
            return Vector3.Magnitude(ProjectPointLine(point, lineStart, lineEnd) - point);
        }
    }
}
