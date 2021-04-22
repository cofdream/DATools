using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace DATools
{
    /// <summary>
    /// 批量修改对象属性
    /// </summary>
    public class UIMuiltChangeVariable : ScriptableObject, IDevelopementTool
    {
        public string ToolName => "批量修改对象属性";

        public void Awake()
        {

        }

        public void OnDestroy()
        {

        }

        public void OnDisable()
        {

        }

        public void OnEnable()
        {

        }

        public void OnGUI()
        {
            var target = Selection.activeGameObject;
            if (target == null)
            {
                GUILayout.Label("请选择游戏对象");
            }
            else
            {
                GUILayout.Label(target.name);
            }

            if (GUILayout.Button("开启全部射线检测"))
            {
                var targets = target.GetComponentsInChildren<Graphic>();
                foreach (var item in targets)
                {
                    item.raycastTarget = true;
                }
            }
            if (GUILayout.Button("关闭全部射线检测"))
            {
                var targets = target.GetComponentsInChildren<Graphic>();
                foreach (var item in targets)
                {
                    item.raycastTarget = false;
                }
            }
        }
    }
}