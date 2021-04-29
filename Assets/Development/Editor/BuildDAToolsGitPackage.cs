using UnityEngine;
using UnityEditor;

namespace DATools.Development
{
    internal static class BuildDAToolsGitPackage
    {
        [MenuItem("Build Package/Run")]
        internal static void BuildDAToolsPackage()
        {
            var cmds = new CMD[]
            {
                new CMD(){  CMDContent = "git tag -d v0.0.1"                                        ,Title = "Build" ,Info = "Build  in git package.Please Wait. cmd: git tag -d v0.0.1"                                      },
                new CMD(){  CMDContent = "git push origin :refs/tags/v0.0.1"                        ,Title = "Build" ,Info = "Build  in git package.Please Wait. cmd: git push origin :refs/tags/v0.0.1"                      },
                new CMD(){  CMDContent = "git subtree split --prefix=Assets/DATools --branch upm"   ,Title = "Build" ,Info = "Build  in git package.Please Wait. cmd: git subtree split --prefix=Assets/DATools --branch upm" },
                new CMD(){  CMDContent = "git tag v0.0.1 upm"                                       ,Title = "Build" ,Info = "Build  in git package.Please Wait. cmd: git tag v0.0.1 upm"                                     },
                new CMD(){  CMDContent = "git push origin upm --tags"                               ,Title = "Build" ,Info = "Build  in git package.Please Wait. cmd: git push origin upm --tags"                             },
            };

            Debug.Log(CMD.Run(cmds, cmds.Length));
        }
    }
}