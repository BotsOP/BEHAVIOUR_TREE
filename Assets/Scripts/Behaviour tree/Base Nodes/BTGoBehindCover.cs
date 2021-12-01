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
        
        public BTGoBehindCover(NavMeshAgent _agent, Transform _transform, BlackBoard _blackBoard, string _targetName)
        {
            agent = _agent;
            transform = _transform;
            blackBoard = _blackBoard;
            targetName = _targetName;
        }
        public override TaskStatus Run()
        {
            Vector3 dir = (transform.position - enemyTransform.position).normalized;
            agent.SetDestination(agent.destination + dir * 5);
            return TaskStatus.Success;
        }
        public override void OnEnter()
        {
            enemyTransform = blackBoard.GetValue<Transform>(targetName);
        }
        public override void OnExit()
        {
        }
    }
}