using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Behaviour_tree.Base_Nodes
{
    public class BTGoBehindCover : BTBaseNode
    {
        private NavMeshAgent agent;
        private Transform transform;
        private Transform enemyTransform;
        private BlackBoard blackBoard;
        private string targetName;
        private string coverName;
        private List<Transform> coverList = new List<Transform>();
        public BTGoBehindCover(NavMeshAgent _agent, Transform _transform, BlackBoard _blackBoard, string _targetName, string _coverName)
        {
            agent = _agent;
            transform = _transform;
            blackBoard = _blackBoard;
            targetName = _targetName;
            coverName = _coverName;
        }
        public override TaskStatus Run()
        {
            if (coverList.Count == 0)
            {
                return TaskStatus.Failed;
            }

                float closestDist = Mathf.Infinity;
                Transform targetCover = null;
                foreach (var cover in coverList)
                {
                    if (Vector3.Distance(transform.position, cover.position) < closestDist)
                    {
                        closestDist = Vector3.Distance(transform.position, cover.position);
                        targetCover = cover;
                        blackBoard.SetValue("targetCover", cover);
                    }
                }

                Vector3 enemyDir = (transform.position - enemyTransform.position).normalized;
                List<Vector3> potentialCoverSpots = targetCover.GetComponent<CoverSpotsGenerator>().posAroundCube;
                float closestAngle = 0.2f;
                Vector3 closestPos = Vector3.zero;
                foreach (var pos in potentialCoverSpots)
                {
                    Vector3 localPos = (pos - targetCover.position).normalized;
                    localPos.y = 0;
                    enemyDir.y = 0;
                    if (Vector3.Dot(localPos, enemyDir) > closestAngle)
                    {
                        closestAngle = Vector3.Dot(localPos, enemyDir);
                        closestPos = pos;
                    }
                }

                agent.SetDestination(closestPos);

                return TaskStatus.Success;
        }
        public override void OnEnter()
        {
            enemyTransform = blackBoard.GetValue<Transform>(targetName);
            coverList = blackBoard.GetValue<List<Transform>>(coverName);
        }
        public override void OnExit()
        {
        }
    }
}