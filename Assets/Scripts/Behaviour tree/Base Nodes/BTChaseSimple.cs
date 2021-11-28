using UnityEngine;
using UnityEngine.AI;

namespace Behaviour_tree.Base_Nodes
{
    public class BTChaseSimple : BTBaseNode
    {
        private NavMeshAgent agent;
        private BlackBoard blackBoard;
        public BTChaseSimple(NavMeshAgent _agent, BlackBoard _blackBoard)
        {
            agent = _agent;
            blackBoard = _blackBoard;

        }
        public override TaskStatus Run()
        {
            agent.SetDestination(blackBoard.GetValue<Transform>("target").position);
            return TaskStatus.Success;
        }
        public override void OnEnter()
        {
        }
        public override void OnExit()
        {
        }
    }
}