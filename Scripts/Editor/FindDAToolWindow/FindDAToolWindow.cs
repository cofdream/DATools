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
            window.position = EditorWindowExtension.GetMainWindowCenteredPosition(new Vector2(300, 600));
            window.Show();
        }

        private string searchFilter;
        private string lastSearchFilter;

        List<SearchData> allSearchDataList;
        List<SearchData> drawSearchDataList;

        private Vector2 scroolViewPosition;

        private void OnEnable()
        {
            LoadAttributes();
        }

        private void OnGUI()
        {
            if (allSearchDataList == null) LoadAttributes();

            GUILayout.BeginHorizontal(GUI.skin.FindStyle("Toolbar"));
            {
                Rect rect = GUILayoutUtility.GetRect(0, this.maxSize.x, 18, 18, EditorStyles.toolbarSearchField, GUILayout.MinWidth(60));

                var temp_SearchFilter = searchFilter;
                searchFilter = GUIExpand.DrawSearchField(rect, searchFilter);

                if (temp_SearchFilter != searchFilter)
                {
                    FindTool();
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            //Rect rect = EditorGUILayout.GetControlRect();

            //rect.position = Vector2.zero;
            //rect.size = new Vector2(position.width, position.height);

            //Rect viewRect = new Rect(new Vector2(0, 0), new Vector2(position.width - 15, position.height * 2));

            //scroolViewPosition = GUI.BeginScrollView(rect, scroolViewPosition, viewRect, false, true);
            //{
            //    Handles.color = Color.black;

            //    Vector2 contentPostion = new Vector2(0, 4 - 20);
            //    Vector2 contentSize = new Vector2(viewRect.size.x, 20);

            //    if (drawSearchDataList.Count > 0) Handles.DrawLine(new Vector3(contentPostion.x, contentPostion.y + 18), new Vector3(contentSize.x, contentPostion.y + 18));

            //    foreach (var searchData in drawSearchDataList)
            //    {
            //        contentPostion += new Vector2(0, 20);

            //        GUI.Label(new Rect(contentPostion, new Vector2(80, 20)), searchData.attribute.Name);

            //        if (GUI.Button(new Rect(new Vector2(contentPostion.x + 180, contentPostion.y + 1), new Vector2(40, 20)), "Open"))
            //        {
            //            searchData.methodInfo.Invoke(null, null);
            //        }

            //        Handles.DrawLine(new Vector3(contentPostion.x, contentPostion.y + 22), new Vector3(contentSize.x, contentPostion.y + 22));
            //        contentPostion += new Vector2(0, 4);
            //    }
            //}
            //GUI.EndScrollView();
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

            if (GUI.changed)
            {
                base.Repaint();
            }
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