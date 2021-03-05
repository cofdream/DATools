using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DATools
{
    /// <summary>
    /// 扇形排列
    /// </summary>
    public class UIRingLayoutGroupTool : ScriptableObject, IDevelopementTool
    {
        public string ToolName => "圆形排列UI位置";

        private bool ringSelectMode;

        [SerializeField] private GameObject[] ringData;

        private GUIContent ringDataGUIContent;

        private string messageRing;

        private SerializedObject serializedObjectRing;

        private SerializedProperty serializedPropertyRing;

        private float radius;
        private float startAngel;
        private float spacingAngel;

        private const float startLeftValue = 0f;
        private const float startRightValue = 360f;
        private const float spacingLeftValue = -360f;
        private const float spacingRightValue = 360f;



        public void Awake()
        {
            ringSelectMode = true;
            ringData = null;
            serializedObjectRing = new SerializedObject(this);
            ringDataGUIContent = new GUIContent("排列对象");
            serializedPropertyRing = null;
            radius = 50;
        }
        public void Enable()
        {
            serializedPropertyRing = serializedObjectRing.FindProperty("ringData");
        }
        public void Disable()
        {
        }
        public void Destroy()
        {
            ringData = null;
            ringDataGUIContent = null;
            serializedObjectRing = null;
            serializedPropertyRing = null;
        }

        public void OnGUI()
        {
            GameObject[] targets = null;

            if (ringSelectMode)
            {
                messageRing = "基于视图内选中的物体";
                targets = Selection.gameObjects.OrderBy(go => go.transform.GetSiblingIndex()).ToArray();
                ringData = null;
            }
            else
            {
                messageRing = "基于数组内的对象";
                targets = ringData;
            }

            ringSelectMode = GUILayout.Toggle(ringSelectMode, messageRing);

            if (ringSelectMode == false)
            {
                serializedObjectRing.Update();

                EditorGUI.BeginChangeCheck();

                if (serializedPropertyRing != null)
                {
                    EditorGUILayout.PropertyField(serializedPropertyRing, ringDataGUIContent, true);

                    if (EditorGUI.EndChangeCheck())
                    {
                        serializedObjectRing.ApplyModifiedProperties();
                    }
                }
            }

            radius = EditorGUILayout.FloatField("距离中心点半径", radius);

            startAngel = EditorGUILayout.Slider("起始角度（0度位置为右中）", startAngel, startLeftValue, startRightValue);

            spacingAngel = EditorGUILayout.Slider("间隔角度（两个物体之间的）", spacingAngel, spacingLeftValue, spacingRightValue);



            GUILayout.Label("排列UI位置");
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("开始设置相关物体位置"))
                {
                    RingLayoutGroup(targets, radius, spacingAngel, startAngel);
                }
            }
            GUILayout.EndHorizontal();
        }

        private void RingLayoutGroup(GameObject[] targets, float radius, float spacingAngel, float startAngel = 0, float maxAngle = 360)
        {

            if (targets != null && targets.Length > 0)
            {
                foreach (var target in targets)
                {
                    var RectTransform = target.GetComponent<RectTransform>();
                    if (RectTransform == null)
                    {
                        continue;
                    }
                    var temp = Mathf.PI / 180f;
                    float x = radius * Mathf.Cos(startAngel * temp);
                    float y = radius * Mathf.Sin(startAngel * temp);

                    RectTransform.anchoredPosition = new Vector3(x, y, 0);

                    startAngel += spacingAngel;
                }
            }
        }


    }
}