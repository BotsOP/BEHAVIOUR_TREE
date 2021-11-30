using UnityEngine;

namespace Behaviour_tree.Base_Nodes
{
    public class BTResetAnim : BTBaseNode
    {
        private Animator anim;
        private AnimatePackage animatePackage;
        public BTResetAnim(Animator _anim, AnimatePackage _animatePackage)
        {
            anim = _anim;
            animatePackage = _animatePackage;
        }
        public override TaskStatus Run()
        {
            return TaskStatus.Success;
        }
        public override void OnEnter()
        {
        }
        public override void OnExit()
        {
            anim.SetFloat(animatePackage.animationName, animatePackage.animationValue);
        }
    }
}
