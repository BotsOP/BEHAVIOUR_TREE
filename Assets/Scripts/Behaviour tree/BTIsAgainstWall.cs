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
                if (HandleUtility.DistancePointLine(transform.position, coverScript.worldVerts[i], coverScript.worldVerts[secondVert]) < distanceToWall)
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
    }
}
