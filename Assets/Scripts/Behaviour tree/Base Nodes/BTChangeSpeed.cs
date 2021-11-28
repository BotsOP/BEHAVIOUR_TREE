using System;
using UnityEngine.AI;

namespace Behaviour_tree.Base_Nodes
{
    public class BTChangeSpeed : BTBaseNode
    {
        private NavMeshAgent agent;
        private float newSpeed;
        private float speedIncrease;
        private float time;
        
        public BTChangeSpeed(NavMeshAgent _agent, float _newSpeed, float _time)
        {
            agent = _agent;
            newSpeed = _newSpeed;
            time = _time;
        }
        public override TaskStatus Run()
        {
            if (Math.Abs(newSpeed - agent.speed) < 0.01f)
            {
                return TaskStatus.Success;
            }
            
            agent.speed = agent.speed + speedIncrease;

            return TaskStatus.Running;
        }
        public override void OnEnter()
        {
            speedIncrease = (newSpeed - agent.speed) / (50 * time);
        }
        public override void OnExit()
        {
            
        }
    }
}