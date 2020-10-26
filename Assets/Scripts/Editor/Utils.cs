
namespace DevilAngel.EditorTool
{
    public static class Utils
    {
        /// <summary>
        /// 获取Project目录下选中对象的 文件夹 路径
        /// </summary>
        /// <returns></returns>
        public static string[] GetSelectionPath()
        {
            var guids = UnityEditor.Selection.assetGUIDs;
            if (guids == null) return null;

            for (int i = 0; i < guids.Length; i++)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
                if (System.IO.Directory.Exists(path) == false)
                {
                    int index = path.LastIndexOf('/');
                    path = path.Substring(0, index);
                }
                guids[i] = path;
            }
            return guids;
        }

        public static void OpenDirectory(string path, bool useCMD = true)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                UnityEngine.Debug.Log($"{path} 是Null or WhiteSpace");
            }
            else
            {
                path = path.Replace("/", "\\");//反转 防止找不到文件
                if (System.IO.Directory.Exists(path))
                {
                    path = $"{path}";//路径存在空格会打开错误,加双引号可以解决这个问题
                    if (useCMD)
                        OpenDirectoryByCMD(path);
                    else
                        OpenDirectoryByEXPER(path); //Window10 无法打开不清楚问题
                }
                else
                {
                    UnityEngine.Debug.LogWarning($"{path} 不是文件夹夹路径");
                }
            }
        }
        private static void OpenDirectoryByEXPER(string path)
        {
            System.Diagnostics.Process.Start("exper.exe", path);
        }
        private static void OpenDirectoryByCMD(string path)
        {
            // 新开线程防止锁死
            var newThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(CmdOpenDirectory));
            newThread.Start(path);
        }
        private static void CmdOpenDirectory(object obj)
        {
            var p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c start " + obj.ToString();

            UnityEngine.Debug.Log(p.StartInfo.Arguments);

            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();

            p.WaitForExit();
            p.Close();
        }


    }
}