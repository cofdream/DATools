using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace DATools
{
    [System.Serializable]
    public class CMD
    {
        public string Title;
        public string Command;
        public string Info;

        public static string Run(IEnumerable<CMD> cmdInfos, int length)
        {
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.StandardOutputEncoding = Encoding.GetEncoding("GB2312");

            StringBuilder output = new StringBuilder();


            float index = 0;
            float progress;
            foreach (var cmdInfo in cmdInfos)
            {
                process.Start();

                progress = (float)index / length;
                EditorUtility.DisplayProgressBar
                    (cmdInfo.Title, string.IsNullOrWhiteSpace(cmdInfo.Info) ? cmdInfo.Command : cmdInfo.Info, progress);

                process.StandardInput.WriteLine(cmdInfo.Command);
                process.StandardInput.AutoFlush = true;
                process.StandardInput.WriteLine("exit");
                process.WaitForExit();

                output.AppendLine(process.StandardOutput.ReadToEnd());

                index++;
            }

            process.Close();

            EditorUtility.ClearProgressBar();

            return output.ToString();
        }
    }
}