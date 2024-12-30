using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace Parkjung2016
{
    public class VolumeManager
    {

        public static VolumeManager Instance;


        private Sequence vignettingBlinkkSeq, hitScreenEffectSeq;

        private Volume volume;
        private Beautify.Universal.Beautify beautify;

        public VolumeManager()
        {
            volume = GameObject.FindObjectOfType<Volume>();
            volume.profile.TryGet(out beautify);
        }

        public static void SetVignettingBlink(float value, float duration,Action action= null)
        {
            if (Instance.vignettingBlinkkSeq != null && Instance.vignettingBlinkkSeq.IsActive())
                Instance.vignettingBlinkkSeq.Kill();
            Instance.vignettingBlinkkSeq = DOTween.Sequence();
            Instance.vignettingBlinkkSeq.Append(DOTween.To(() => Instance.beautify.vignettingBlink.value,
                x => Instance.beautify.vignettingBlink.value = x,
                value, duration));
            Instance.vignettingBlinkkSeq.AppendCallback(() => action?.Invoke());
        }

        public static void enableHitScreenEffect(float duration)
        {
            if (Instance.hitScreenEffectSeq != null && Instance.hitScreenEffectSeq.IsActive())
                Instance.hitScreenEffectSeq.Kill();
            Color color = Instance.beautify.frameColor.value;
            Instance.hitScreenEffectSeq = DOTween.Sequence();
            Instance.hitScreenEffectSeq.Append(DOTween.To(() => Instance.beautify.frameColor.value,
                x => Instance.beautify.frameColor.value = x, new Color(color.r, color.g, color.b, .5f),
                duration));
            Instance.hitScreenEffectSeq.AppendInterval(.3f);
            Instance.hitScreenEffectSeq.Append(DOTween.To(() => Instance.beautify.frameColor.value,
                x => Instance.beautify.frameColor.value = x, new Color(color.r, color.g, color.b, 0),
                duration));
        }
    }
}