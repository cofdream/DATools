using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace DATools
{
    public class CMDWindow : EditorWindow
    {
        //[MenuItem("CMD/Run", false, 0)]
        static void BuildDAToolsPackage()
        {
            var cmds = new CMD[]
            {
                new CMD(){  Command = "git tag -d v0.0.1"                                        ,Title = "Build" ,Info = "Build in git package.Please Wait. cmd: git tag -d v0.0.1"                                      },
                new CMD(){  Command = "git push origin :refs/tags/v0.0.1"                        ,Title = "Build" ,Info = "Build in git package.Please Wait. cmd: git push origin :refs/tags/v0.0.1"                      },
                new CMD(){  Command = "git subtree split --prefix=Assets/DATools --branch upm"   ,Title = "Build" ,Info = "Build in git package.Please Wait. cmd: git subtree split --prefix=Assets/DATools --branch upm" },
                new CMD(){  Command = "git tag v0.0.1 upm"                                       ,Title = "Build" ,Info = "Build in git package.Please Wait. cmd: git tag v0.0.1 upm"                                     },
                new CMD(){  Command = "git push origin upm --tags"                               ,Title = "Build" ,Info = "Build in git package.Please Wait. cmd: git push origin upm --tags"                             },
            };

            Debug.Log(CMD.Run(cmds, cmds.Length));
        }


        [MenuItem("CMD/Open", false, 11)]
        static void OpenWindow()
        {
            EditorMainWindow.GetWindowInCenter<CMDWindow>(new Vector2(600, 700)).Show();
        }


        private GitCommand cmdData;
        private string createDataPath = "Assets/NewCMDData.asset";

        private SerializedObject serializedObject;
        private SerializedProperty serializedProperty;

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();

            createDataPath = GUILayout.TextField(createDataPath);
            if (GUILayout.Button("Create"))
            {
                var _cmdData = ScriptableObject.CreateInstance<GitCommand>();

                AssetDatabase.CreateAsset(_cmdData, createDataPath);
                AssetDatabase.ImportAsset(createDataPath);
            }


            GUILayout.EndHorizontal();

            if (cmdData != null)
            {
                if (GUILayout.Button("Run Cmd"))
                {
                    List<CMD> cmds = new List<CMD>(cmdData.cmds.Length);
                    foreach (var item in cmdData.cmds)
                    {
                        cmds.Add(item);
                    }

                    Debug.Log(CMD.Run(cmds, cmds.Count));
                }
            }

            GUILayout.Space(10);

            {
                var _cmdData = EditorGUILayout.ObjectField(cmdData, typeof(GitCommand), false) as GitCommand;

                if (_cmdData != cmdData)
                {
                    cmdData = _cmdData;
                    if (cmdData != null)
                    {
                        serializedObject = new SerializedObject(cmdData);
                        serializedProperty = serializedObject.FindProperty("cmds");
                    }
                }
            }

            if (cmdData != null)
            {
                if (serializedObject == null)
                {
                    serializedObject = new SerializedObject(cmdData);
                    serializedProperty = serializedObject.FindProperty("cmds");
                }
                serializedObject.Update();

                EditorGUILayout.PropertyField(serializedProperty);

                serializedObject.ApplyModifiedProperties();
            }






        }
    }
}