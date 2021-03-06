﻿using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DATools
{
    public class DevelopmentTools : EditorWindow
    {
        private Vector2 scrollViewPosition;
        private UIDevelopmentToolCell[] developementTools;

        [MenuItem("DATools/DevelopmentTool")]
        public static void OpenWindow()
        {
            GetWindow<DevelopmentTools>().Show();
        }

        private void Awake()
        {
            scrollViewPosition = Vector2.zero;
            developementTools = null;
        }
        private void OnEnable()
        {
            LoadTool();

            UnityEditor.Compilation.CompilationPipeline.compilationStarted += CompilationStartedCallBack;
        }
        private void OnDisable()
        {
            UnityEditor.Compilation.CompilationPipeline.compilationStarted -= CompilationStartedCallBack;
            if (developementTools != null)
            {
                foreach (var tool in developementTools)
                {
                    tool.Tool.OnDisable();
                }
            }
        }

        private void OnDestroy()
        {
            if (developementTools != null)
            {
                foreach (var tool in developementTools)
                {
                    tool.Tool.OnDestroy();
                }

                developementTools = null;
            }
        }

        private void OnGUI()
        {
            //保证重新编译以后界面处于打开时能重新获取对象
            if (developementTools == null)
            {
                if (GUILayout.Button("加载工具集"))
                {
                    LoadTool();
                }
                else
                {
                    return;
                }
            }


            if (GUILayout.Button("清除工具集  （Tip：编译之前最好清除一下工具集）"))
            {
                ClearTool();
            }

            if (developementTools == null) return;

            scrollViewPosition = GUILayout.BeginScrollView(scrollViewPosition);
            {
                foreach (var tool in developementTools)
                {
                    GUILayout.BeginVertical("box");
                    {
                        GUILayout.BeginHorizontal();
                        {
                            tool.OpenState = GUILayout.Toggle(tool.OpenState, tool.Tool.ToolName, EditorStyles.foldout);

                            GUILayout.FlexibleSpace();

                            if (GUILayout.Button(EditorGUIUtility.FindTexture("d__Help"), (GUIStyle)"IconButton"))
                            {
                                // todo URL
                                //System.Diagnostics.Process.Start("");
                                // Application.OpenURL
                            }
                        }
                        GUILayout.EndHorizontal();


                        if (tool.OpenState)
                        {
                            GUILayout.Space(5);
                            tool.Tool.OnGUI();
                        }
                    }
                    GUILayout.EndVertical();

                }
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndScrollView();
        }

        private void LoadTool()
        {
            //var assmblys = System.AppDomain.CurrentDomain.GetAssemblies();
            var toolType = typeof(IDevelopementTool);
            var assmbly = toolType.Assembly;
            System.Collections.Generic.List<IDevelopementTool> tools = new System.Collections.Generic.List<IDevelopementTool>();

            var unityObj = typeof(UnityEngine.ScriptableObject);
            //foreach (var assembly in assmblys)
            //{
            System.Collections.Generic.IEnumerable<System.Type> types = assmbly.GetTypes().Where(t => toolType.IsAssignableFrom(t) && t.IsAbstract == false);

            foreach (var type in types)
            {
                IDevelopementTool tool = null;
                if (unityObj.IsAssignableFrom(type))
                {
                    var toolTypes = Resources.FindObjectsOfTypeAll(type);
                    if (toolTypes.Length != 0)
                    {
                        tool = toolTypes[0] as IDevelopementTool;
                    }
                    else
                    {
                        tool = ScriptableObject.CreateInstance(type) as IDevelopementTool;
                    }
                }
                else
                {
                    tool = assmbly.CreateInstance(type.Namespace + "." + type.Name) as IDevelopementTool;
                }
                tool.Awake();
                tool.OnEnable();
                tools.Add(tool);
            }
            //}

            developementTools = new UIDevelopmentToolCell[tools.Count];
            int index = 0;
            foreach (var tool in tools)
            {
                developementTools[index] = new UIDevelopmentToolCell() { Tool = tool, };
                index++;
            }
        }
        private void ClearTool()
        {
            foreach (var tool in developementTools)
            {
                tool.Tool.OnDisable();
            }
            foreach (var tool in developementTools)
            {
                tool.Tool.OnDestroy();
            }

            developementTools = null;
        }
        private void CompilationStartedCallBack(object @object)
        {
            if (developementTools != null)
            {
                ClearTool();
            }
        }

        public class UIDevelopmentToolCell
        {
            public IDevelopementTool Tool;
            public bool OpenState;
        }
    }
}