using UnityEngine;
using UnityEditor;

namespace DATools.Development
{
    internal static class BuildDAToolsGitPackage
    {
        [MenuItem("Build Package/Run")]
        internal static void BuildDAToolsPackage()
        {
            EditorUtility.DisplayProgressBar("Build", "Build  in git package.Please Wait.", 0);
            Debug.Log(Utils.CMD(new string[]
            {
                "git tag -d v0.0.1",
                "git push origin :refs/tags/v0.0.1",
                "git subtree split --prefix=Assets/DATools --branch upm",
                "git tag v0.0.1 upm",
                "git push origin upm --tags",
            }));
            EditorUtility.ClearProgressBar();
        }
    }
}