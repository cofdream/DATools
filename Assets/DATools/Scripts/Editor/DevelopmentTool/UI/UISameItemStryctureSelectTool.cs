﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DATools
{
    public class UISameItemStryctureSelectTool : ScriptableObject, IDevelopementTool
    {
        public string ToolName => "同类Item选中工具";

        protected Vector2 scrollViewPosition;

        protected SerializedObject serializedObject = null;
        protected SerializedProperty serializedPropertySameItem = null;
        protected SerializedProperty serializedPropertyIsPriorityRootPosition = null;
        protected SerializedProperty serializedPropertyRootData = null;
        protected SerializedProperty serializedPropertyIsShowRootData = null;

        [SerializeField] protected List<GameObject> sameItems = null;
        [SerializeField] protected bool isPriorityRootPosition;//是否优先节点位置 
        [SerializeField] protected List<RootData> rootDatas = null;
        [SerializeField] protected bool isShowRootData;

        protected GUIContent smaeItemGUIContent = null;
        protected GUIContent rootDataGUIContent = null;
        protected GUIContent isPriorityRootPositionGUIContent = null;
        protected GUIContent isShowRootDataGUIContent = null;

        protected GUIContent selectSameItemGUIContent = null;

        public virtual void Awake()
        {
            scrollViewPosition = Vector2.zero;

            serializedObject = new SerializedObject(this);

            sameItems = null;
            isPriorityRootPosition = true;
            rootDatas = null;
            isShowRootData = false;

            smaeItemGUIContent = new GUIContent("所有的同类物体");
            rootDataGUIContent = new GUIContent("当前的节点信息   (谨慎修改)");
            isPriorityRootPositionGUIContent = new GUIContent("优先基于节点位置获取节点", "True：优先基于每个节点位置去获取节点,获取失败会尝试通过节点对象的名字去寻找。Flase:优先节点名字寻找，获取失败通过节点位置寻找");
            isShowRootDataGUIContent = new GUIContent("显示当前的节点信息");
            selectSameItemGUIContent = new GUIContent("选中同类对象", "基于当前选中物体的节点信息去获取同类物体");
        }
        public virtual void OnEnable()
        {
            serializedPropertySameItem = serializedObject.FindProperty("sameItems");
            serializedPropertyIsPriorityRootPosition = serializedObject.FindProperty("rootDatas");
            serializedPropertyRootData = serializedObject.FindProperty("isPriorityRootPosition");
            serializedPropertyIsShowRootData = serializedObject.FindProperty("isShowRootData");
        }
        public virtual void OnDisable()
        {

        }
        public virtual void OnDestroy()
        {
            serializedObject = null;

            serializedPropertySameItem = null;
            serializedPropertyIsPriorityRootPosition = null;
            serializedPropertyRootData = null;
            serializedPropertyIsShowRootData = null;

            sameItems = null;
            rootDatas = null;

            smaeItemGUIContent = null;
            rootDataGUIContent = null;
            isPriorityRootPositionGUIContent = null;
            isShowRootDataGUIContent = null;
            selectSameItemGUIContent = null;
        }
        public virtual void OnGUI()
        {
            if (serializedObject == null) return;

            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedPropertySameItem, smaeItemGUIContent, true);

            if (isShowRootData)
            {
                EditorGUILayout.PropertyField(serializedPropertyRootData, rootDataGUIContent, true);
            }


            scrollViewPosition = GUILayout.BeginScrollView(scrollViewPosition);
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("获取当前选中物体下的所有子物体，保存到同类节点集合内", EditorStyles.boldLabel);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("覆盖之前的全部数据"))
                    {
                        if (sameItems == null)
                        {
                            sameItems = new List<GameObject>(10);
                        }
                        else
                        {
                            sameItems.Clear();
                        }
                        AddDataToSameItems(Selection.gameObjects);
                    }
                    if (GUILayout.Button("添加到之前的数据后"))
                    {
                        if (sameItems == null)
                        {
                            sameItems = new List<GameObject>();
                        }
                        AddDataToSameItems(Selection.gameObjects);
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(3);
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("保存节点"))
                    {
                        SaveRootDatas(Selection.activeGameObject);

                    }
                    if (GUILayout.Button(selectSameItemGUIContent))
                    {
                        Selection.objects = GetSameItemsByRootDatas(sameItems, rootDatas);
                        EditorWindow.FocusWindowIfItsOpen<SearchableEditorWindow>();
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(3);
                if (GUILayout.Button("获取节点数据并选中同类对象"))
                {
                    SaveRootDatas(Selection.activeGameObject);

                    Selection.objects = GetSameItemsByRootDatas(sameItems, rootDatas);
                    EditorWindow.FocusWindowIfItsOpen<SearchableEditorWindow>();
                }
            }
            GUILayout.EndScrollView();


            serializedObject.ApplyModifiedProperties();
        }


        /// <summary>
        /// 保存当前数据到到同类节点内
        /// </summary>
        /// <param name="gameObjects"></param>
        protected void AddDataToSameItems(GameObject[] gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                var transform = gameObject.transform;
                foreach (Transform child in transform)
                {
                    sameItems.Add(child.gameObject);
                }
            }
        }

        /// <summary>
        /// 保存节点数据
        /// </summary>
        /// <param name="gameObject"></param>
        protected void SaveRootDatas(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return;
            }

            if (TryGetRootData(gameObject.transform, ref rootDatas) == false)
            {
                Debug.LogWarning("当前选中的物体不属于当前同类物体的子物体");

            }
        }

        /// <summary>
        /// 尝试获取节点数据
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="rootDatas"></param>
        /// <returns></returns>
        protected bool TryGetRootData(Transform transform, ref List<RootData> rootDatas)
        {
            GameObject sameItem;
            if (TryGetSameItem(transform, out sameItem))
            {
                //获取节点
                rootDatas = new List<RootData>();
                if (sameItem.Equals(transform.gameObject) == false)
                {
                    GetRootData(sameItem.transform, transform, ref rootDatas);
                }
                else
                {
                    Debug.Log("选中物体时节点");
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否在当前同类物体内
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        protected bool IsInSameItems(Transform transform)
        {
            foreach (var sameItem in sameItems)
            {
                if (transform.IsChildOf(sameItem.transform))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 尝试获取是否在在当前同类物体内
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        protected bool TryGetSameItem(Transform transform, out GameObject gameObject)
        {
            foreach (var sameItem in sameItems)
            {
                if (transform.IsChildOf(sameItem.transform))
                {
                    gameObject = sameItem;
                    return true;
                }
            }
            gameObject = null;
            return false;
        }

        /// <summary>
        /// 获取子物体在父物体的位置数据
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <param name="rootDatas"></param>
        protected void GetRootData(Transform parent, Transform child, ref List<RootData> rootDatas)
        {
            Transform tempParent = child.parent;
            if (tempParent.Equals(parent) == false)
            {
                GetRootData(parent, tempParent, ref rootDatas);
            }

            rootDatas.Add(
                new RootData
                {
                    Index = child.GetSiblingIndex(),
                    Name = child.name,
                }
            );

        }

        /// <summary>
        /// 基于节点数据获取目标对象集合
        /// </summary>
        /// <param name="targets"></param>
        /// <param name="rootDatas"></param>
        /// <returns></returns>
        protected GameObject[] GetSameItemsByRootDatas(List<GameObject> targets, List<RootData> rootDatas)
        {
            GameObject[] sameItems = new GameObject[targets.Count];
            int index = 0;
            foreach (var sameItem in targets)
            {

                if (sameItem == null)
                {
                    Debug.LogWarning("下标为" + index.ToString() + "的节点为空");
                    index++;
                }
                else
                {
                    Transform temp = GetTransformByRootDatas(sameItem.transform, rootDatas);
                    if (temp != null)
                    {
                        sameItems[index] = temp.gameObject;
                    }
                    index++;
                }
            }

            return sameItems;
        }

        /// <summary>
        /// 基于节点信息集合获取对象
        /// </summary>
        /// <param name="rootTransform"></param>
        /// <param name="rootDatas"></param>
        /// <returns></returns>
        protected Transform GetTransformByRootDatas(Transform rootTransform, List<RootData> rootDatas)
        {
            Transform temp = rootTransform;
            foreach (var rootData in rootDatas)
            {
                temp = GetTransformByRootData(temp, rootData);
                if (temp == null)
                {
                    Debug.LogErrorFormat("节点：{0}获取失败，失败对象为{1}", rootData.ToString(), rootTransform == null ? rootTransform.name : temp.name);
                    continue;
                }
            }

            return temp;
        }

        /// <summary>
        /// 基于节点信息获取对象
        /// </summary>
        /// <param name="rootTransform"></param>
        /// <param name="rootData"></param>
        /// <returns></returns>
        protected Transform GetTransformByRootData(Transform rootTransform, RootData rootData)
        {
            Transform temp = rootTransform;
            if (isPriorityRootPosition)
            {
                temp = GetTransformByRootDataInNode(temp, rootData.Index);
                if (temp == null)
                {
                    temp = GetTransformByRootDataInName(temp, rootData.Name);
                }
            }
            else
            {
                temp = GetTransformByRootDataInName(temp, rootData.Name);
                if (temp == null)
                {
                    temp = GetTransformByRootDataInNode(temp, rootData.Index);
                }
            }

            return temp;
        }

        /// <summary>
        /// 基于节点下标获取对象的子物体
        /// </summary>
        /// <param name="rootTransform"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        protected Transform GetTransformByRootDataInNode(Transform rootTransform, int node)
        {
            return rootTransform.GetChild(node);
        }

        /// <summary>
        /// 基于节点名字获取对象的子物体
        /// </summary>
        /// <param name="rootTransform"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected Transform GetTransformByRootDataInName(Transform rootTransform, string name)
        {
            return rootTransform.Find(name);
        }


        /// <summary>
        /// 节点数据
        /// </summary>
        [Serializable]
        protected struct RootData
        {
            public int Index;
            public string Name;

            public override string ToString()
            {
                return string.Format("下标：{0}，名字：{1}", Index, Name);
            }
        }
    }
}