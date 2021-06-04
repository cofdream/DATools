using UnityEngine;
using UnityEditor;

namespace DATools
{
    [CreateAssetMenu(menuName = "Asset File", fileName = "NewAsset")]
    internal sealed class CreateAsset : ScriptableObject
    {
    }

    [CustomEditor(typeof(CreateAsset), false)]
    internal class CreateAssetInspector : Editor
    {

        public override void OnInspectorGUI()
        {
            SerializedObject serializedObject = new SerializedObject(target);
            serializedObject.Update();

            SerializedProperty m_Script = serializedObject.FindProperty("m_Script");
            EditorGUILayout.PropertyField(m_Script);

            serializedObject.ApplyModifiedProperties();
        }
    }
}