using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace DATools
{
    public class UIDrawRaycast : ScriptableObject, IDevelopementTool
    {
        public string ToolName => "UIDrawRaycast";

        private DrawGizmosUIRaycast drawGameObject;
        private bool isShow;
        private bool IsShow
        {
            get { return isShow; }
            set
            {
                if (isShow == value) return;

                isShow = value;
                drawGameObject?.gameObject.SetActive(value);
            }
        }

        private int selectIndex;
        private string[] colorArray;

        public void OnDestroy()
        {
            colorArray = null;

            if (drawGameObject != null)
            {
                DestroyImmediate(drawGameObject.gameObject);
                drawGameObject = null;
            }
            else
            {
                ClearAllInScreenDrawGizmosObject();
            }
        }

        public void Awake()
        {
            isShow = false;
            CreateDrawGameObject();

            selectIndex = 5;
            colorArray = new string[]
            {
                "Red", "Green", "Blue","white","black", "yellow", "cyan","magenta","gray", "grey", "clear"
            };
        }

        public void OnGUI()
        {
            if (drawGameObject == null)
            {
                CreateDrawGameObject();
            }

            IsShow = GUILayout.Toggle(IsShow, "绘制UI射线");
            selectIndex = EditorGUILayout.Popup(selectIndex, colorArray);

            switch (selectIndex)
            {
                case 0: drawGameObject.DrawColor = Color.red; break;
                case 1: drawGameObject.DrawColor = Color.green; break;
                case 2: drawGameObject.DrawColor = Color.blue; break;
                case 3: drawGameObject.DrawColor = Color.white; break;
                case 4: drawGameObject.DrawColor = Color.black; break;
                case 5: drawGameObject.DrawColor = Color.yellow; break;
                case 6: drawGameObject.DrawColor = Color.cyan; break;
                case 7: drawGameObject.DrawColor = Color.magenta; break;
                case 8: drawGameObject.DrawColor = Color.gray; break;
                case 9: drawGameObject.DrawColor = Color.grey; break;
                case 10: drawGameObject.DrawColor = Color.clear; break;
            }
        }

        private void CreateDrawGameObject()
        {
            ClearAllInScreenDrawGizmosObject();

            drawGameObject = new GameObject("[Draw Gizmos UI Raycast]").AddComponent<DrawGizmosUIRaycast>();
            drawGameObject.gameObject.SetActive(isShow);

            if (Application.isPlaying)
            {
                DontDestroyOnLoad(drawGameObject.gameObject);
            }
        }
        private void ClearAllInScreenDrawGizmosObject()
        {
            var drawObjs = GameObject.FindObjectsOfType<DrawGizmosUIRaycast>();
            foreach (var drawObj in drawObjs)
            {
                DestroyImmediate(drawObj);
            }
        }

        public void OnEnable()
        {

        }

        public void OnDisable()
        {

        }

        private sealed class DrawGizmosUIRaycast : MonoBehaviour
        {
            public Color DrawColor;
            static Vector3[] fourCorners = new Vector3[4];
            void OnDrawGizmos()
            {
                foreach (Graphic graphic in GameObject.FindObjectsOfType<Graphic>())
                {
                    if (graphic.raycastTarget)
                    {
                        RectTransform rectTransform = graphic.transform as RectTransform;
                        DrawRect(rectTransform);
                    }
                }
            }

            void DrawRect(RectTransform rectTransform)
            {
                if (rectTransform == null) return;
                rectTransform.GetWorldCorners(fourCorners);
                Gizmos.color = DrawColor;
                for (int i = 0; i < 4; i++)
                {
                    Gizmos.DrawLine(fourCorners[i], fourCorners[(i + 1) % 4]);
                }
            }
        }
    }
}