using UnityEngine;
using UnityEngine.AI;

namespace Behaviour_tree.Base_Nodes
{
    public class BTMoveTo : BTBaseNode
    {
        private NavMeshAgent agent;
        private BlackBoard blackBoard;
        private string blackBoardName;
        public BTMoveTo(NavMeshAgent _agent, BlackBoard _blackBoard, string _blackBoardName)
        {
            agent = _agent;
            blackBoard = _blackBoard;
            blackBoardName = _blackBoardName;
        }
        public override TaskStatus Run()
        {
            agent.SetDestination(blackBoard.GetValue<Transform>(blackBoardName).position);
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