using System.Collections.Generic;
using UnityEngine;

namespace DATools
{
    [System.Serializable]
    public class MissingInfo
    {
        public Object MissingObject;
        public string Path;
        public List<MissingData> MissingDatas;
    }
}