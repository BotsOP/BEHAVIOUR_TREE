using UnityEngine;

namespace Behaviour_tree.Base_Nodes
{
    public class BTDebug : BTBaseNode
    {
        private string debug;
        public BTDebug(string _debug)
        {
            debug = _debug;

        }
        public override TaskStatus Run()
        {
            Debug.Log(debug);
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