using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace DATools
{
    public class UIDrawRaycast : ScriptableObject, IDevelopementTool
    {
        public string ToolName => "UIDrawRaycast";

        private int selectIndex;
        private static string[] colorArray;

        public void OnDestroy()
        {
            colorArray = null;
        }

        public void Awake()
        {
            isShow = false;

            selectIndex = 0;
            colorArray = new string[]
            {
                "Red", "Green", "Blue","white","black", "yellow", "cyan","magenta","gray", "grey", "clear"
            };
        }

        public void OnGUI()
        {
            isShow = GUILayout.Toggle(isShow, "绘制UI射线");
            selectIndex = EditorGUILayout.Popup(selectIndex, colorArray);

            switch (selectIndex)
            {
                case 0: drawColor = Color.red; break;
                case 1: drawColor = Color.green; break;
                case 2: drawColor = Color.blue; break;
                case 3: drawColor = Color.white; break;
                case 4: drawColor = Color.black; break;
                case 5: drawColor = Color.yellow; break;
                case 6: drawColor = Color.cyan; break;
                case 7: drawColor = Color.magenta; break;
                case 8: drawColor = Color.gray; break;
                case 9: drawColor = Color.grey; break;
                case 10: drawColor = Color.clear; break;
            }
        }


        public void OnEnable()
        {

        }

        public void OnDisable()
        {

        }

        private static bool isShow;
        private static Color drawColor;
        private static Vector3[] fourCorners = new Vector3[4];


        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected, typeof(Graphic))]
        private static void DrawRayRect(Graphic graphic, GizmoType gizmoType)
        {
            if (isShow)
            {
                var rect = graphic.GetComponent<RectTransform>();
                if (rect != null)
                {
                    rect.GetWorldCorners(fourCorners);
                    Gizmos.color = drawColor;
                    for (int i = 0; i < 4; i++)
                    {
                        Gizmos.DrawLine(fourCorners[i], fourCorners[(i + 1) % 4]);
                    }
                }
            }
        }
    }
}