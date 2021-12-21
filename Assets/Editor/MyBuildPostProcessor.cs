/*
 *  @CreatedTime 2021年7月1日 18:12:12
 *  @FileName MyBuildPostProcessor.cs
 *  @Version 1.00
 *  @Author  Jacob.zhang
 *
*/

using UnityEngine;
using System.Collections;
using UnityEditor.Callbacks;
using UnityEditor;
using System.IO;
using System;

public class MyBuildPostprocessor  
{
        [PostProcessBuildAttribute()]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
                if (target == BuildTarget.Android)
    		        PostProcessAndroidBuild(pathToBuiltProject);
        }

        public static void PostProcessAndroidBuild(string pathToBuiltProject)
        {
                UnityEditor.ScriptingImplementation backend = UnityEditor.PlayerSettings.GetScriptingBackend(UnityEditor.BuildTargetGroup.Android);

                if (backend == UnityEditor.ScriptingImplementation.IL2CPP)
                {
                        CopyAndroidIL2CPPSymbols(pathToBuiltProject, PlayerSettings.Android.targetArchitectures);
                }
        }

        public static void CopyAndroidIL2CPPSymbols(string pathToBuiltProject, AndroidArchitecture targetDevice)
        {
                string buildName = Path.GetFileNameWithoutExtension(pathToBuiltProject);
                FileInfo fileInfo = new FileInfo(pathToBuiltProject);
                string symbolsDir = fileInfo.Directory.Name;
                symbolsDir = symbolsDir + "/"+buildName+"_IL2CPPSymbols";

                CreateDir(symbolsDir);

                switch (PlayerSettings.Android.targetArchitectures)
                {
                      case AndroidArchitecture.All:
                        {
                            CopyARMSymbols(symbolsDir);
                            break;
                        }
                      case AndroidArchitecture.ARMv7:
                        {
                            CopyARMSymbols(symbolsDir);
                            break;
                        }
                      // case AndroidTargetDevice.:
                      //   {
                      //       CopyX86Symbols(symbolsDir);
                      //       break;
                      //   }
                      default:
                      break;
                }
        }


        const string libpath = "/../Temp/StagingArea/libs/";
        const string libFilename = "libil2cpp.so";
        private static void CopyARMSymbols(string symbolsDir)
        {
                // string sourcefileARM = Application.dataPath + libpath + "armeabi-v7a/" + libFilename;
                // CreateDir(symbolsDir + "/armeabi-v7a/");
                // File.Copy(sourcefileARM, symbolsDir + "/armeabi-v7a/libil2cpp.so");
        }

        // private static void CopyX86Symbols(string symbolsDir)
        // {
        //         string sourcefileX86 = Application.dataPath + libpath + "x86/libil2cpp.so.debug";
        //         File.Copy(sourcefileX86, symbolsDir + "/x86/libil2cpp.so.debug");
        // }

        public static void CreateDir(string path)
        {
                if (Directory.Exists(path))
                    return;

                Directory.CreateDirectory(path);
        }
}