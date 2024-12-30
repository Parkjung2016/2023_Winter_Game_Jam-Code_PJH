#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


[InitializeOnLoad]
public class InitializeOnLoadScene
{
    static InitializeOnLoadScene()
    {
        GameObject scene = GameObject.Find("Scene");
        string sceneName = SceneManager.GetActiveScene().name;
        if (scene.GetComponent(Type.GetType(sceneName))) return;
        GameObject.Find("Scene").AddComponent(Type.GetType(SceneManager.GetActiveScene().name));
    }
}
#endif