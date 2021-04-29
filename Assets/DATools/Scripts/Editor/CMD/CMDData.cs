using UnityEngine;
using UnityEditor;

namespace DATools
{
    public class CMDData : ScriptableObject
    {
        public CMDElement[] cmds;
    }

    [System.Serializable]
    public class CMDElement
    {
        public string caption;
        public bool State;
        public CMD CMD;
       
    }
}