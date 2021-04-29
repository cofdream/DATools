using System.Collections.Generic;
using System.Text;
using UnityEditor;

namespace DATools
{
    [System.Serializable]
    public struct CMD
    {
        public string CMDContent;
        public string Title;
        public string Info;

        public static string Run(IEnumerable<CMD> cmdInfos, int length)
        {
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.StandardOutputEncoding = System.Text.Encoding.GetEncoding("GB2312");


            StringBuilder output = new StringBuilder("cmd:\n");

            float index = 0;
            float progress;
            foreach (var cmdInfo in cmdInfos)
            {
                process.Start();

                progress = (float)index / length;
                EditorUtility.DisplayProgressBar(cmdInfo.Title, cmdInfo.Info, progress);

                process.StandardInput.WriteLine(cmdInfo.CMDContent);
                process.StandardInput.AutoFlush = true;

                output.AppendLine(cmdInfo.CMDContent);
                process.StandardInput.WriteLine("exit");
                process.WaitForExit();

                output.AppendLine();
                output.AppendLine(process.StandardOutput.ReadToEnd());

                index++;
            }

            process.Close();

            EditorUtility.ClearProgressBar();


            return output.ToString();
        }
    }
}