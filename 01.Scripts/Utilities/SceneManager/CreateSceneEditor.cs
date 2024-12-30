#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Parkjung2016.Library;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class CreateSceneEditor : EditorWindow
{
    private string sceneName = "씬 이름";
    public Scene targetScene;
    private string sceneSavePath;

    private void OnGUI()
    {

        // EditorGUIUtility.hotControl = 3;
        var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 15 };
        EditorGUILayout.LabelField("새 씬의 이름을 설정해주세요.", style);
        EditorGUILayout.Space(20);
        sceneName = EditorGUILayout.TextField("씬 이름", sceneName);
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("씬 저장 경로를 설정해주세요.", style);
        EditorGUILayout.LabelField($"씬 저장 경로 : {sceneSavePath}");
        if (GUILayout.Button("씬 저장 경로 선택"))
        {
            sceneSavePath =
                EditorUtility.OpenFolderPanel("씬 저장 경로를 설정해주세요.", "Assets", "");
        }

        EditorGUILayout.Space(50);
        if (GUILayout.Button("변경"))
        {
            targetScene.name = sceneName;
            if (sceneSavePath == "")
            {
                Debug.LogError("씬 저장 경로를 먼저 선택해주세요.");
                return;
            }

            if (!Directory.Exists($"{sceneSavePath}/{sceneName}"))
                Directory.CreateDirectory($"{sceneSavePath}/{sceneName}");
            if (File.Exists($"{sceneSavePath}/{sceneName}/{sceneName}.unity"))
            {
                Debug.LogError("이 경로에 같은 이름을 가진 씬이 이미 존재합니다.");
                return;
            }

            string strSceneName = sceneName.Replace(" ", "");
            bool isNumber = int.TryParse(strSceneName.ToCharArray()[0].ToString(), out int a);
            if (isNumber)
            {
                Debug.LogError("씬 이름은 숫자,특수문자로 시작 될 수 없습니다.");
                return;
            }


            EditorSceneManager.SaveScene(targetScene, $"{sceneSavePath}/{sceneName}/{sceneName}.unity");
            GenerateSceneScript(strSceneName);
            Close();
        }
    }


    void GenerateSceneScript(String strSceneName)
    {
        string copyPath = $"Assets/01.Scripts/Scene/{strSceneName}.cs";
        if (!Directory.Exists("Assets/01.Scripts")) Directory.CreateDirectory("Assets/01.Scripts");
        if (!Directory.Exists("Assets/01.Scripts/Scene")) Directory.CreateDirectory("Assets/01.Scripts/Scene");
        if (!File.Exists(copyPath))
        {
            using (StreamWriter outFile =
                   new StreamWriter(copyPath))
            {
                outFile.WriteLine("using UnityEngine;");
                outFile.WriteLine("using System.Collections;");
                outFile.WriteLine("");
                outFile.WriteLine($"public class {strSceneName} : SceneBase");
                outFile.WriteLine("{");
                outFile.WriteLine("     private void Start()");
                outFile.WriteLine("     {");
                outFile.WriteLine("     ");
                outFile.WriteLine("     }");
                outFile.WriteLine("");
                outFile.WriteLine("");
                outFile.WriteLine("     private void Update()");
                outFile.WriteLine("     {");
                outFile.WriteLine("     ");
                outFile.WriteLine("     }");
                outFile.WriteLine("}");
            }
        }
        AssetDatabase.Refresh();
    }
}
#endif
