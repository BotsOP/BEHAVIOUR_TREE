using UnityEngine;

namespace Behaviour_tree.Base_Nodes
{
    public class BTPickWeaponUp : BTBaseNode
    {
        private BlackBoard blackBoard;
        private Transform crowbar;
        private Transform crowbarInHand;

        public BTPickWeaponUp(BlackBoard _blackBoard, Transform _crowbarInHand)
        {
            blackBoard = _blackBoard;
            crowbarInHand = _crowbarInHand;
        }
        public override TaskStatus Run()
        {
            crowbar.gameObject.layer = 0;
            crowbar.transform.gameObject.SetActive(false);
            crowbarInHand.transform.gameObject.SetActive(true);
            
            return TaskStatus.Success;
        }
        public override void OnEnter()
        {
            crowbar = blackBoard.GetValue<Transform>("weapon");
            blackBoard.SetValue("hasWeapon", true);
        }
        public override void OnExit()
        {
            
        }
    }
}