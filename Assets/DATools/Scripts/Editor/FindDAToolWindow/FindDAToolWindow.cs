using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;
using System;
using System.Collections.Generic;

namespace DATools
{
    public class FindDAToolWindow : EditorWindow
    {
        [MenuItem("DATools/Find DATool Window", false, 0)]
        static void OpenWindow()
        {
            var window = GetWindow<FindDAToolWindow>();
            window.position = EditorMainWindow.GetMainWindowCenteredPosition(new Vector2(300, 600));
            window.Show();
        }

        private string searchFilter;
        private string lastSearchFilter;

        List<SearchData> allSearchDataList;
        List<SearchData> drawSearchDataList;

        private void OnEnable()
        {
            LoadAttributes();
        }
        private void OnGUI()
        {
            if (allSearchDataList == null) LoadAttributes();

            GUILayout.BeginHorizontal(GUI.skin.FindStyle("Toolbar"));
            {
                searchFilter = GUILayout.TextField(searchFilter, (GUIStyle)"ToolbarSeachTextField");

                if (GUILayout.Button(string.Empty, (GUIStyle)"ToolbarSeachCancelButton"))
                {
                    GUI.FocusControl(null);
                    if (searchFilter != string.Empty)
                    {
                    }
                    searchFilter = string.Empty;
                    FindTool();
                }
                if (GUILayout.Button("Search", GUILayout.Width(56)))
                {
                    FindTool();
                }

            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            EditorGUILayout.BeginVertical();
            {
                foreach (var searchData in drawSearchDataList)
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label(searchData.attribute.Name);
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Open"))
                        {
                            searchData.methodInfo.Invoke(null, null);
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void LoadAttributes()
        {
            allSearchDataList = new List<SearchData>();

            var assembly = typeof(SearchToolsAttribute).Assembly;

            Type[] types = assembly.GetTypes();

            foreach (var type in types)
            {
                MethodInfo[] methodInfos = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (MethodInfo methodInfo in methodInfos)
                {
                    var attribute = methodInfo.GetCustomAttribute<SearchToolsAttribute>(true);

                    if (attribute != null)
                    {
                        allSearchDataList.Add(new SearchData(attribute, methodInfo));
                    }
                }
            }

            drawSearchDataList = new List<SearchData>(allSearchDataList);
        }

        private void FindTool()
        {
            if (lastSearchFilter != searchFilter)
            {
                lastSearchFilter = searchFilter;
            }
            else return;

            if (string.IsNullOrWhiteSpace(searchFilter))
            {
                ShowAllTool();
                return;
            }
            string filter = searchFilter.ToLower();

            drawSearchDataList.Clear();

            foreach (var searchData in allSearchDataList)
            {
                foreach (var keyword in searchData.attribute.Keywords)
                {
                    if (keyword.Contains(filter))
                    {
                        drawSearchDataList.Add(searchData);
                        continue;
                    }
                }
            }
        }

        private void ShowAllTool()
        {
            drawSearchDataList.Clear();

            drawSearchDataList.AddRange(allSearchDataList);
        }


        class SearchData
        {
            public SearchToolsAttribute attribute;
            public MethodInfo methodInfo;

            public SearchData(SearchToolsAttribute attribute, MethodInfo methodInfo)
            {
                this.attribute = attribute;
                this.methodInfo = methodInfo;
            }
        }
    }
}