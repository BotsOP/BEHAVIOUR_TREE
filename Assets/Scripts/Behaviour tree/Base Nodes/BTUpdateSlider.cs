namespace Behaviour_tree.Base_Nodes
{
    public class BTUpdateSlider : BTBaseNode
    {
        private float min;
        private float max;
        private BlackBoard blackBoard;
        private string valueName;
        private AgentUI enemyUI;
        public BTUpdateSlider(float _min, float _max, BlackBoard _blackBoard, string _valueName, AgentUI _enemyUI)
        {
            min = _min;
            max = _max;
            blackBoard = _blackBoard;
            valueName = _valueName;
            enemyUI = _enemyUI;
        }
        public override TaskStatus Run()
        {
            enemyUI.UpdateSlider(min, max, blackBoard.GetValue<float>(valueName));
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
