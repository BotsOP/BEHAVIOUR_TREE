namespace Behaviour_tree.Base_Nodes
{
    public class BTChangeText : BTBaseNode
    {
        private AgentUI enemyUI;
        private TextDisplay display;
        public BTChangeText(AgentUI _enemyUI, TextDisplay _display)
        {
            enemyUI = _enemyUI;
            display = _display;
        }
        public override TaskStatus Run()
        {
            enemyUI.UpdateText(display);
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
