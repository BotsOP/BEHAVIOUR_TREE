using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Behaviour_tree.Base_Nodes
{
    public class BTGoToClosestPos : BTBaseNode
    {
        private NavMeshAgent agent;
        private Transform transform;
        private BlackBoard blackBoard;
        private string targetName;
        private List<Transform> targetList = new List<Transform>();
        public BTGoToClosestPos(NavMeshAgent _agent, BlackBoard _blackBoard, string _targetName, Transform _transform)
        {
            agent = _agent;
            blackBoard = _blackBoard;
            targetName = _targetName;
            transform = _transform;

        }
        public override TaskStatus Run()
        {
            if (targetList.Count == 0)
            {
                return TaskStatus.Failed;
            }
            
            float closestDist = Mathf.Infinity;
            Vector3 targetPos = Vector3.zero;
            foreach (var target in targetList)
            {
                if (Vector3.Distance(transform.position, target.position) < closestDist)
                {
                    targetPos = target.position;
                }
            }
            agent.SetDestination(targetPos);
            return TaskStatus.Success;
        }
        public override void OnEnter()
        {
            targetList = blackBoard.GetValue<List<Transform>>(targetName);
        }
        public override void OnExit()
        {
        }
    }
}