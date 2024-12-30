using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager
{

    public static TimeManager Instance;

    private CoroutineHandler coroutineHandler;

    public static void SetFreeze(float time, float duration)
    {
        if (Instance.coroutineHandler != null) Instance.coroutineHandler.Stop();
        Instance.coroutineHandler = CoroutineHandler.Start_Coroutine(Instance.Freeze(time, duration));
    }

    private IEnumerator Freeze(float time, float duration)
    {
        Time.timeScale = time;
        yield return YieldCache.WaitForSeconds(duration);
        Time.timeScale = 1;
    }
}