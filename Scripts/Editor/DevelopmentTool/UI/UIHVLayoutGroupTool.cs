using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DATools
{
    /// <summary>
    /// — | 排列
    /// </summary>
    public class UIHVLayoutGroupTool : ScriptableObject, IDevelopementTool
    {
        private readonly string screenModeTip = "基于Hierarchy视图内选中的游戏物体";
        private readonly string arrayModeTip = "基于对象集合内的游戏物体";

        public string ToolName => "UI物体水平或垂直排列位置";
        private bool selectHVMode;

        [SerializeField] private GameObject[] dataHV = null;
        private SerializedObject serializedObject = null;
        private SerializedProperty serializedProperty = null;
        private GUIContent dataHVGUIContent = null;

        private float spacing;


        public void Awake()
        {
            selectHVMode = true;
            spacing = 0;
        }
        public void OnDestroy()
        {
            serializedObject = null;
            serializedProperty = null;
            dataHVGUIContent = null;
        }
        public void OnEnable()
        {
            serializedObject = new SerializedObject(this);
            dataHVGUIContent = new GUIContent("排列对象");
            serializedProperty = serializedObject.FindProperty("dataHV");
        }
        public void OnDisable()
        {
        }
        public void OnGUI()
        {
            GameObject[] targets = null;
            string message = string.Empty;

            if (selectHVMode)
            {
                message = screenModeTip;
                targets = Selection.gameObjects.OrderBy(go => go.transform.GetSiblingIndex()).ToArray();
                dataHV = null;
            }
            else
            {
                message = arrayModeTip;
                targets = dataHV;
            }

            selectHVMode = GUILayout.Toggle(selectHVMode, message);

            if (selectHVMode == false)
            {
                serializedObject.Update();

                EditorGUI.BeginChangeCheck();

                EditorGUILayout.PropertyField(serializedProperty, dataHVGUIContent, true);

                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                }
            }

            spacing = EditorGUILayout.FloatField("排列间距", spacing);

            GUILayout.Label("排列UI位置");
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("从左向右"))
                {
                    HorizontalLayoutGroup(targets, true, spacing);
                }
                if (GUILayout.Button("从右向左"))
                {
                    HorizontalLayoutGroup(targets, false, spacing);
                }

                if (GUILayout.Button("从上向下"))
                {
                    VerticalLayoutGroup(targets, true, spacing);
                }
                if (GUILayout.Button("从下向上"))
                {
                    VerticalLayoutGroup(targets, false, spacing);
                }
            }
            GUILayout.EndHorizontal();
        }


        private void HorizontalLayoutGroup(GameObject[] targets, bool right = true, float spacing = 0f)
        {
            if (targets != null && targets.Length > 1)
            {
                Undo.RecordObjects(targets, "Horizontal Position");

                bool init = true;
                Vector3 lastPos = Vector3.zero;
                float lastOffestX = 0;
                int direction = right ? 1 : -1;

                int length = targets.Length;
                for (int i = 0; i < length; i++)
                {
                    var gameObject = targets[i];
                    if (gameObject == null) continue;
                    var rectTransform = gameObject.GetComponent<RectTransform>();
                    if (rectTransform == null) continue;

                    var value = Vector2.one * 0.5f;
                    rectTransform.anchorMin = value;
                    rectTransform.anchorMax = value;
                    rectTransform.pivot = value;

                    if (init)
                    {
                        init = false;
                        lastPos = rectTransform.localPosition;
                        lastOffestX = rectTransform.rect.width * 0.5f;
                        continue;
                    }

                    //获取坐标偏移
                    float slefOffestX = rectTransform.rect.width * 0.5f;
                    lastPos.x += (slefOffestX + lastOffestX) * direction + spacing;

                    rectTransform.localPosition = lastPos;
                    //保留下次坐标偏移距离
                    lastOffestX = slefOffestX;
                }
            }
        }
        private void VerticalLayoutGroup(GameObject[] targets, bool down = true, float spacing = 0)
        {
            if (targets != null && targets.Length > 1)
            {
                Undo.RecordObjects(targets, "Vertical Position");

                bool init = true;
                Vector3 lastPos = Vector3.zero;
                float lastOffestY = 0;
                int direction = down ? -1 : 1;

                int length = targets.Length;
                for (int i = 0; i < length; i++)
                {
                    var gameObject = targets[i];
                    if (gameObject == null) continue;
                    var element = gameObject.GetComponent<RectTransform>();
                    if (element == null) continue;

                    var value = Vector2.one * 0.5f;
                    element.anchorMin = value;
                    element.anchorMax = value;
                    element.pivot = value;

                    if (init)
                    {
                        init = false;
                        lastPos = element.localPosition;
                        lastOffestY = element.rect.height * 0.5f;
                        continue;
                    }

                    //获取坐标偏移
                    float slefOffestY = element.rect.height * 0.5f;
                    lastPos.y += (slefOffestY + lastOffestY) * direction - spacing;

                    element.localPosition = lastPos;
                    //保留下次坐标偏移距离
                    lastOffestY = slefOffestY;
                }
            }
        }
    }
}