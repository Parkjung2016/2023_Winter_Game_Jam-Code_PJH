using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

public class CameraManager
{
    private Dictionary<string, CinemachineVirtualCameraBase> cams = new();

    private CinemachineVirtualCameraBase currentCam;


    private CoroutineHandler coroutineHandler;
    private Sequence noiseSequence;

    public void Init()
    {
        CinemachineVirtualCameraBase[] cameras = Object.FindObjectsOfType<CinemachineVirtualCameraBase>();

        for (int i = 0; i < cameras.Length; i++)
        {
            cams.Add(cameras[i].name, cameras[i]);
        }
    }

    public void ChangeCamera(string cameraName)
    {
        if (!cams.ContainsKey(cameraName)) return;
        foreach (var cam in cams)
        {
            cam.Value.Priority = 10;
        }

        CinemachineVirtualCameraBase camera = cams[cameraName];
        currentCam = camera;
        camera.Priority = 15;
    }

    public void SetNoise(float time, float amplitudeGain, float frequencyGain, float duration,
        CinemachineVirtualCameraBase cam = null)
    {
        if (currentCam == null) return;
        if (coroutineHandler != null)
        {
            coroutineHandler.Stop();
        }

        if (noiseSequence != null && noiseSequence.IsActive()) noiseSequence.Kill();


        coroutineHandler = CoroutineHandler.Start_Coroutine(Noise(time, amplitudeGain, frequencyGain, duration,
            cam == null ? currentCam : cam));
    }

    private IEnumerator Noise(float time, float amplitudeGain, float frequencyGain, float duration,
        CinemachineVirtualCameraBase cam)
    {
        if (cam is CinemachineVirtualCamera virtualCam)
        {
            CinemachineBasicMultiChannelPerlin perlin =
                virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            noiseSequence = DOTween.Sequence();
            SetPerlinValue(perlin, amplitudeGain, frequencyGain, duration);
            yield return YieldCache.WaitForSeconds(time);
            SetPerlinValue(perlin, 0, 0, duration);
        }
        else if (cam is CinemachineFreeLook)
        {
            CinemachineBasicMultiChannelPerlin[] perlins = new[]
            {
                FreeLookGetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>(),
                FreeLookGetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>(),
                FreeLookGetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>()
            };

            noiseSequence = DOTween.Sequence();
            for (int i = 0; i < perlins.Length; i++)
            {
                SetPerlinValue(perlins[i], amplitudeGain, frequencyGain, duration);
            }

            yield return YieldCache.WaitForSeconds(time);
            for (int i = 0; i < perlins.Length; i++)
            {
                SetPerlinValue(perlins[i], 0, 0, duration);
            }
        }
    }

    private void SetPerlinValue(CinemachineBasicMultiChannelPerlin perlin, float amplitudeGain, float frequencyGain,
        float duration)
    {
        noiseSequence.Append(DOTween.To(
            () => perlin.m_FrequencyGain,
            x => perlin.m_FrequencyGain = x,
            frequencyGain,
            duration
        ));
        noiseSequence.Append(DOTween.To(
            () => perlin.m_AmplitudeGain,
            x => perlin.m_AmplitudeGain = x,
            amplitudeGain,
            duration
        ));
    }

    public CinemachineVirtualCamera FreeLookGetRig(byte idx)
    {
        if (currentCam is CinemachineFreeLook freeLook)
        {
            return freeLook.GetRig(idx);
        }

        return null;
    }

    public void ChangeLookAt(Transform target, CinemachineVirtualCameraBase cam = null)
    {
        if (cam == null)
        {
            currentCam.LookAt = target;
        }
        else cam.LookAt = target;
    }

    public void ChangeFollow(Transform target, CinemachineVirtualCameraBase cam = null)
    {
        if (cam == null)
        {
            currentCam.Follow = target;
        }
        else cam.Follow = target;
    }
}