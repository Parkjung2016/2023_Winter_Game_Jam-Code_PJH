using System;
using System.Collections;
using System.Collections.Generic;
using PlayerStat;
using UGS;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class SceneBase : MonoBehaviour
{
    internal string SceneName;

    internal Scene SceneInfo;


    protected virtual void Awake()
    {
        UnityGoogleSheet.LoadAllData();

    }
}