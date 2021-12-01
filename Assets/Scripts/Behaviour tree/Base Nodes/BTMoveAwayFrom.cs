using UnityEngine;
using UnityEngine.AI;

namespace Behaviour_tree.Base_Nodes
{
    public class BTMoveAwayFrom : BTBaseNode
    {
        private NavMeshAgent agent;
        private Transform target;
        private BlackBoard blackBoard;
        private string targetName;
        private Transform transform;
        public BTMoveAwayFrom(NavMeshAgent _agent, Transform _runnerPos, BlackBoard _blackBoard, string _targetName)
        {
            agent = _agent;
            transform = _runnerPos;
            blackBoard = _blackBoard;
            targetName = _targetName;
        }
        public override TaskStatus Run()
        {
            Vector3 dir = (transform.position - target.position).normalized;
            agent.SetDestination(transform.position + dir);
            return TaskStatus.Success;
        }
        public override void OnEnter()
        {
            target = blackBoard.GetValue<Transform>(targetName);
        }
        public override void OnExit()
        {
        }
    }
}