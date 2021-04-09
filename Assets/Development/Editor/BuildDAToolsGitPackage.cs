using UnityEngine;
using UnityEditor;

namespace DATools.Development
{
    internal static class BuildDAToolsGitPackage
    {
        [MenuItem("Build Package/Run")]
        internal static void BuildDAToolsPackage()
        {
            Debug.Log(Utils.Cmd(new Utils.CmdInfo[]
            {
                new Utils.CmdInfo(){  CMDContent = "git tag -d v0.0.1"                                        ,Title = "Build" ,Info = "Build  in git package.Please Wait. cmd: git tag -d v0.0.1"                                      },
                new Utils.CmdInfo(){  CMDContent = "git push origin :refs/tags/v0.0.1"                        ,Title = "Build" ,Info = "Build  in git package.Please Wait. cmd: git push origin :refs/tags/v0.0.1"                      },
                new Utils.CmdInfo(){  CMDContent = "git subtree split --prefix=Assets/DATools --branch upm"   ,Title = "Build" ,Info = "Build  in git package.Please Wait. cmd: git subtree split --prefix=Assets/DATools --branch upm" },
                new Utils.CmdInfo(){  CMDContent = "git tag v0.0.1 upm"                                       ,Title = "Build" ,Info = "Build  in git package.Please Wait. cmd: git tag v0.0.1 upm"                                     },
                new Utils.CmdInfo(){  CMDContent = "git push origin upm --tags"                               ,Title = "Build" ,Info = "Build  in git package.Please Wait. cmd: git push origin upm --tags"                             },
            }));
        }

    }
}