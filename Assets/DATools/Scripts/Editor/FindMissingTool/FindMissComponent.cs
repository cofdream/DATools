using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace DATools
{
    public class FindMissComponent : EditorWindow
    {
        [SearchTools("寻找丢失脚本的游戏对象")]
        [MenuItem("DATools/Find Missing Tools/Find Miss Component")]
        static void OpenWindow()
        {
            var window = GetWindow<FindMissComponent>();
            window.position = EditorMainWindow.GetMainWindowCenteredPosition(new Vector2(300, 400));
            window.Show();
        }

        private GameObject needFindGameObject;
        private List<GameObject> missingGameObjects = new List<GameObject>();
        private Vector2 scroolViewPosition;

        private void OnGUI()
        {
            // 显示目标脚本
            GUI.enabled = false;
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty m_Script = serializedObject.FindProperty("m_Script");
            EditorGUILayout.PropertyField(m_Script);
            GUI.enabled = true;

            // 寻找指定对象的脚本
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(new GUIContent("Target"));
                needFindGameObject = EditorGUILayout.ObjectField(needFindGameObject, typeof(GameObject), true) as GameObject;

                GUI.enabled = needFindGameObject != null;
                if (GUILayout.Button("Find"))
                {
                    missingGameObjects.Clear();
                    EditorUtility.DisplayProgressBar("Find in project all prefabs", "Finding...", 0);
                    FindMissComponents(needFindGameObject, missingGameObjects);
                    foreach (Transform transform in needFindGameObject.transform)
                    {
                        FindMissComponents(transform.gameObject, missingGameObjects);
                    }
                    EditorUtility.ClearProgressBar();

                }
                GUI.enabled = true;
            }
            GUILayout.EndHorizontal();

            // 寻找所有预制体
            GUILayout.Space(5);
            if (GUILayout.Button("Find in project all prefabs"))
            {
                string[] guiIds = AssetDatabase.FindAssets("t:Prefab");

                int currentCount = 0;
                int count = guiIds.Length;

                missingGameObjects.Clear();

                foreach (var guiId in guiIds)
                {
                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guiId));
                    if (prefab != null)
                    {
                        FindMissComponents(prefab, missingGameObjects);
                        EditorUtility.DisplayProgressBar("Find in project all prefabs", "Finding...", currentCount++ / count);
                        foreach (Transform transform in prefab.transform)
                        {
                            FindMissComponents(transform.gameObject, missingGameObjects);
                            EditorUtility.DisplayProgressBar("Find in project all prefabs", "Finding...", currentCount++ / count);
                        }
                    }
                }
                Debug.Log("Find GameObject Count:" + count);
                EditorUtility.ClearProgressBar();
            }

            // 显示寻找结果
            if (missingGameObjects.Count > 0)
            {
                GUILayout.Space(3);
                scroolViewPosition = GUILayout.BeginScrollView(scroolViewPosition);
                {
                    GUILayout.Label("All missing Components");
                    GUI.enabled = false;
                    foreach (var missingGameObject in missingGameObjects)
                    {
                        EditorGUILayout.ObjectField(missingGameObject, typeof(GameObject), true);
                    }
                    GUI.enabled = true;
                }
                GUILayout.EndScrollView();
            }
        }

        static void FindMissComponents(GameObject target, List<GameObject> missingGameObjects)
        {
            var components = target.GetComponents<Component>();

            foreach (var component in components)
            {
                if (component == null)
                {
                    Debug.Log("Missing Commponent", target);
                    missingGameObjects.Add(target);
                    return;
                }
            }
        }
    }
}