using UnityEngine;

namespace Behaviour_tree.Base_Nodes
{
    public class BTInstantiate : BTBaseNode
    {
        private GameObject prefab;
        private Vector3 pos;
        private Quaternion rotation;
        private Vector3 scale;
        private float uniformScale;
        private Transform parent;
        private BlackBoard blackBoard;
        private string valueName;
        
        public BTInstantiate(GameObject _prefab, Vector3 _pos, Quaternion _rotation, Vector3 _scale, Transform _parent)
        {
            prefab = _prefab;
            pos = _pos;
            rotation = _rotation;
            scale = _scale;
            parent = _parent;
        }
        public BTInstantiate(GameObject _prefab, Vector3 _pos, Quaternion _rotation, float _uniformScale, Transform _parent)
        {
            prefab = _prefab;
            pos = _pos;
            rotation = _rotation;
            uniformScale = _uniformScale;
            parent = _parent;
        }
        public BTInstantiate(GameObject _prefab, Vector3 _pos, Quaternion _rotation, Vector3 _scale, Transform _parent, BlackBoard _blackBoard, string _valueName)
        {
            prefab = _prefab;
            pos = _pos;
            rotation = _rotation;
            scale = _scale;
            parent = _parent;
            blackBoard = _blackBoard;
            valueName = _valueName;
        }
        public BTInstantiate(GameObject _prefab, Vector3 _pos, Quaternion _rotation, float _uniformScale, Transform _parent, BlackBoard _blackBoard, string _valueName)
        {
            prefab = _prefab;
            pos = _pos;
            rotation = _rotation;
            uniformScale = _uniformScale;
            parent = _parent;
            blackBoard = _blackBoard;
            valueName = _valueName;
        }
        public override TaskStatus Run()
        {
            GameObject gameObject = GameObject.Instantiate(prefab, pos, rotation, parent);
            if (uniformScale != 0)
            {
                gameObject.transform.localScale *= uniformScale;
                gameObject.transform.localPosition = pos;
                if (valueName != null)
                {
                    blackBoard.SetValue(valueName, gameObject);
                }
                return TaskStatus.Success;
            }
            gameObject.transform.localScale = scale;
            if (valueName != null)
            {
                blackBoard.SetValue(valueName, gameObject);
            }
            Debug.Break();
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