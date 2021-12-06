namespace Behaviour_tree.Base_Nodes
{
    public class BTNoticeText : BTBaseNode
    {
        private AgentUI enemyUI;
        private BlackBoard blackBoard;
        private string valueName;
        
        public BTNoticeText(AgentUI _enemyUI, BlackBoard _blackBoard, string _valueName)
        {
            enemyUI = _enemyUI;
            blackBoard = _blackBoard;
            valueName = _valueName;
        }
        public override TaskStatus Run()
        {
            if (blackBoard.GetValue<float>(valueName) > 0)
            {
                enemyUI.UpdateText(TextDisplay.Noticing);
            }
            else
            {
                enemyUI.UpdateText(TextDisplay.Patrolling);
            }
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
