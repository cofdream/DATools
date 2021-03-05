using UnityEngine;

namespace DATools
{
    public class UIResolutionExchangeTool :  IDevelopementTool
    {
        //TODO 制作一个能快速切换分辨率的工具，（缘由：每次查看特点机型分辨率切换的时候寻找麻烦，有按钮快速切换相对方便）

        public string ToolName => "临时工具";

        public void Awake()
        {

        }

        public void Destroy()
        {

        }

        public void Disable()
        {
        }

        public void Enable()
        {
        }

        public void OnGUI()
        {
            GUILayout.Label("开发中...");
        }

        //static object gameViewSizesInstance;
        //static MethodInfo getGroup;
        //public enum GameViewSizeType
        //{
        //    AspectRatio, FixedResolution
        //}

        //// [Button("切换下一个屏幕分辨率")]

        //public static void ChangedViewResolution()
        //{
        //    var gameSizeType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizes");
        //    var singleType = typeof(ScriptableSingleton<>).MakeGenericType(gameSizeType);
        //    var instanceProp = singleType.GetProperty("instance");
        //    getGroup = gameSizeType.GetMethod("GetGroup");
        //    gameViewSizesInstance = instanceProp.GetValue(null, null);

        //    var temp = getGroup.Invoke(gameViewSizesInstance, null);
        //    Debug.Log(1);
        //}

        //// [MenuItem("Test/AddSize")]
        //public static void AddTestSize()
        //{
        //    AddCustomSize(GameViewSizeType.AspectRatio, GameViewSizeGroupType.Standalone, 123, 456, "Test size");
        //}

        //// [MenuItem("Test/SizeTextQuery")]
        //public static void SizeTextQueryTest()
        //{
        //    Debug.Log(SizeExists(GameViewSizeGroupType.Standalone, "Test size"));
        //}

        //// [MenuItem("Test/Query16:9Test")]
        //public static void WidescreenQueryTest()
        //{
        //    Debug.Log(SizeExists(GameViewSizeGroupType.Standalone, "16:9"));
        //}

        ////[MenuItem("Test/Set16:9")]
        //public static void SetWidescreenTest()
        //{
        //    SetSize(FindSize(GameViewSizeGroupType.Standalone, "16:9"));
        //}

        //// [MenuItem("Test/SetTestSize")]
        //public static void SetTestSize()
        //{
        //    int idx = FindSize(GameViewSizeGroupType.Standalone, 123, 456);
        //    if (idx != -1)
        //        SetSize(idx);
        //}

        //public static void SetSize(int index)
        //{
        //    var gvWndType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
        //    var selectedSizeIndexProp = gvWndType.GetProperty("selectedSizeIndex",
        //            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        //    var gvWnd = EditorWindow.GetWindow(gvWndType);
        //    selectedSizeIndexProp.SetValue(gvWnd, index, null);
        //}

        ////[MenuItem("Test/SizeDimensionsQuery")]
        //public static void SizeDimensionsQueryTest()
        //{
        //    Debug.Log(SizeExists(GameViewSizeGroupType.Standalone, 123, 456));
        //}

        //public static void AddCustomSize(GameViewSizeType viewSizeType, GameViewSizeGroupType sizeGroupType, int width, int height, string text)
        //{
        //    // GameViewSizes group = gameViewSizesInstance.GetGroup(sizeGroupTyge);
        //    // group.AddCustomSize(new GameViewSize(viewSizeType, width, height, text);

        //    var group = GetGroup(sizeGroupType);
        //    var addCustomSize = getGroup.ReturnType.GetMethod("AddCustomSize"); // or group.GetType().
        //    var gvsType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSize");
        //    var ctor = gvsType.GetConstructor(new Type[] { typeof(int), typeof(int), typeof(int), typeof(string) });
        //    var newSize = ctor.Invoke(new object[] { (int)viewSizeType, width, height, text });
        //    addCustomSize.Invoke(group, new object[] { newSize });
        //}

        //public static bool SizeExists(GameViewSizeGroupType sizeGroupType, string text)
        //{
        //    return FindSize(sizeGroupType, text) != -1;
        //}

        //public static int FindSize(GameViewSizeGroupType sizeGroupType, string text)
        //{
        //    // GameViewSizes group = gameViewSizesInstance.GetGroup(sizeGroupType);
        //    // string[] texts = group.GetDisplayTexts();
        //    // for loop...

        //    var group = GetGroup(sizeGroupType);
        //    var getDisplayTexts = group.GetType().GetMethod("GetDisplayTexts");
        //    var displayTexts = getDisplayTexts.Invoke(group, null) as string[];
        //    for (int i = 0; i < displayTexts.Length; i++)
        //    {
        //        string display = displayTexts[i];
        //        // the text we get is "Name (W:H)" if the size has a name, or just "W:H" e.g. 16:9
        //        // so if we're querying a custom size text we substring to only get the name
        //        // You could see the outputs by just logging
        //        // Debug.Log(display);
        //        int pren = display.IndexOf('(');
        //        if (pren != -1)
        //            display = display.Substring(0, pren - 1); // -1 to remove the space that's before the prens. This is very implementation-depdenent
        //        if (display == text)
        //            return i;
        //    }
        //    return -1;
        //}

        //public static bool SizeExists(GameViewSizeGroupType sizeGroupType, int width, int height)
        //{
        //    return FindSize(sizeGroupType, width, height) != -1;
        //}

        //public static int FindSize(GameViewSizeGroupType sizeGroupType, int width, int height)
        //{
        //    // goal:
        //    // GameViewSizes group = gameViewSizesInstance.GetGroup(sizeGroupType);
        //    // int sizesCount = group.GetBuiltinCount() + group.GetCustomCount();
        //    // iterate through the sizes via group.GetGameViewSize(int index)

        //    var group = GetGroup(sizeGroupType);
        //    var groupType = group.GetType();
        //    var getBuiltinCount = groupType.GetMethod("GetBuiltinCount");
        //    var getCustomCount = groupType.GetMethod("GetCustomCount");
        //    int sizesCount = (int)getBuiltinCount.Invoke(group, null) + (int)getCustomCount.Invoke(group, null);
        //    var getGameViewSize = groupType.GetMethod("GetGameViewSize");
        //    var gvsType = getGameViewSize.ReturnType;
        //    var widthProp = gvsType.GetProperty("width");
        //    var heightProp = gvsType.GetProperty("height");
        //    var indexValue = new object[1];
        //    for (int i = 0; i < sizesCount; i++)
        //    {
        //        indexValue[0] = i;
        //        var size = getGameViewSize.Invoke(group, indexValue);
        //        int sizeWidth = (int)widthProp.GetValue(size, null);
        //        int sizeHeight = (int)heightProp.GetValue(size, null);
        //        if (sizeWidth == width && sizeHeight == height)
        //            return i;
        //    }
        //    return -1;
        //}

        //static object GetGroup(GameViewSizeGroupType type)
        //{
        //    return getGroup.Invoke(gameViewSizesInstance, new object[] { (int)type });
        //}

        ////[MenuItem("Test/LogCurrentGroupType")]
        ////public static void LogCurrentGroupType()
        ////{
        ////    Debug.Log(GetCurrentGroupType());
        ////}
        //public static GameViewSizeGroupType GetCurrentGroupType()
        //{
        //    var getCurrentGroupTypeProp = gameViewSizesInstance.GetType().GetProperty("currentGroupType");
        //    return (GameViewSizeGroupType)(int)getCurrentGroupTypeProp.GetValue(gameViewSizesInstance, null);
        //}
        //public static void switchOrientation()
        //{
        //    int width = Screen.height;
        //    int height = Screen.width;
        //    int index = FindSize(GetCurrentGroupType(), width, height);
        //    if (index == -1)
        //    {
        //        AddCustomSize(GameViewSizeType.FixedResolution, GetCurrentGroupType(), width, height, "");
        //        index = FindSize(GetCurrentGroupType(), width, height);
        //    }
        //    if (index != -1)
        //    {
        //        SetSize(index);
        //    }
        //    else
        //    {
        //        Debug.LogError("switchOrientation failed, can not find or add resoulution for " + width.ToString() + "*" + height.ToString());
        //    }
        //}

    }
}
