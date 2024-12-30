using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
public class EditorCoroutine
{
    public static EditorCoroutine StartCoroutine(IEnumerator _routine)
    {
        EditorCoroutine coroutine = new EditorCoroutine(_routine);
        coroutine.Start();
        return coroutine;
    }

    private readonly IEnumerator routine;
    private EditorCoroutine(IEnumerator _routine) => routine = _routine;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode == LoadSceneMode.Single)
            UnityEditor.EditorApplication.update -= Update;
    }

    private void Start()
    {
        EditorApplication.update += Update;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Stop()
    {
        EditorApplication.update -= Update;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (!routine.MoveNext()) Stop();
    }
}
#endif