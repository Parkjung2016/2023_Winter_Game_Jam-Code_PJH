using System;
using System.Collections;
using System.Collections.Generic;
using Michsky.LSS;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    private LSS_Manager lssManager;
    private static SceneManagement instance;

    public static SceneManagement Instance
    {
        get
        {
            if (instance == null)
            {
                SceneManagement sceneManagement = FindObjectOfType<SceneManagement>();
                if (sceneManagement == null)
                {
                    GameObject sceneManager = new GameObject("SceneManager");
                    sceneManagement = sceneManager.AddComponent<SceneManagement>();
                    sceneManagement.lssManager = sceneManager.AddComponent<LSS_Manager>();
                }

                instance = sceneManagement;
            }

            return instance;
        }
    }

    private SceneBase currentScene;
    private CameraManager cameraManager;
    
    
    public SceneBase CurrentScene => currentScene;
    public CameraManager CameraManager => cameraManager;

    private void Awake()
    {
        SceneManagement[] sceneManagements = FindObjectsOfType<SceneManagement>();
        if (sceneManagements.Length < 2)
        {
            lssManager = GetComponent<LSS_Manager>();
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnLoadScene;
        }
        else
        {
            Destroy(gameObject);
        }

        cameraManager = new CameraManager();
        if (currentScene == null) Init(SceneManager.GetActiveScene());
        // Init(SceneManager.GetActiveScene());
    }
    public void SetCursor(bool enable)
    {
        Cursor.lockState = enable ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = enable;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLoadScene;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLoadScene;
    }

    private void OnLoadScene(Scene arg0, LoadSceneMode arg1)
    {
        Init(arg0);
    }

    private void Init(Scene scene)
    {
        currentScene = FindObjectOfType<SceneBase>();
        currentScene.SceneName = scene.name;
        currentScene.SceneInfo = scene;
        cameraManager.Init();
    }


    public static void LoadScene(string sceneName)
    {
        Instance.lssManager.SetPreset("Futuristic");
        Instance.lssManager.LoadScene(sceneName);
        LSS_LoadingScreen.instance.titleObj.text = "Made by WINNER";

    }

    public static void LoadScene(string sceneName, string titleText)
    {
        LoadScene(sceneName);
        LSS_LoadingScreen.instance.titleObj.text = "Made by WINNER";
    }
}