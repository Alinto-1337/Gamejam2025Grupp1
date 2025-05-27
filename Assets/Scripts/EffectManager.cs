using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;
using System.Collections.Generic;
using System;

using Random = UnityEngine.Random;

public class EffectManager : Singleton<EffectManager>
{
    [SerializeField] private Volume volume;
    [SerializeField] private GameCamera gameCamera;

    [Header("GVignette Pulse")]
    [SerializeField] private AnimationCurve aggressivePulseCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float aggressivePulseDuration = 1f;
    [SerializeField] private AnimationCurve normalPulseCurve;
    [SerializeField] private float normalPulseDuration = 1f;

    private bool isPlayingVignetteEffect = false;
    private Vignette vignette;


    [SerializeField] private List<ScreenShakeInstance> activeScreenShakes = new List<ScreenShakeInstance>();


    public enum EffectPower
    {
        aggressive,
        normal
    }

    protected override void Awake()
    {
        base.Awake();
        
        // Ensure the volume has a Vignette effect
        if (volume.profile.TryGet(out vignette) == false)
        {
            vignette = volume.profile.Add<Vignette>();
            vignette.active = true;
        }

        vignette.color.overrideState = true;
        vignette.intensity.overrideState = true;
        vignette.smoothness.overrideState = true;
    }

    private void Update()
    {
        UpdateScreenShake();
    }


    /// <summary>
    /// Pulses the vignette.
    /// </summary>
    /// <param name="intensity">Vignette Intensity (0 - 1)</param>
    public void PlayVignettePulse(float intensity, Color color, EffectPower power)
    {
        if (isPlayingVignetteEffect) return;
        StartCoroutine(PlayVignettePulseRoutine(intensity, color, power));
    }

    public void PlayScreenShakePulse(float intensity, EffectPower power)
    {
        activeScreenShakes.Add(new ScreenShakeInstance(
            intensity,
            power == EffectPower.aggressive ? aggressivePulseDuration : normalPulseDuration,
            power == EffectPower.aggressive ? aggressivePulseCurve : normalPulseCurve
        ));
    }

    private IEnumerator PlayVignettePulseRoutine(float intensity, Color color, EffectPower power = EffectPower.aggressive)
    {
        float elapsed = 0f;
        isPlayingVignetteEffect = true;

        AnimationCurve pulseCurve = power == EffectPower.aggressive ? aggressivePulseCurve : normalPulseCurve;
        float pulseDuration = power == EffectPower.aggressive ? aggressivePulseDuration : normalPulseDuration;

        while (elapsed < pulseDuration)
        {
            float t = elapsed / pulseDuration;
            float curveValue = pulseCurve.Evaluate(t) * intensity;
            vignette.color.value = color;
            vignette.intensity.value = curveValue;
            vignette.smoothness.value = Mathf.Lerp(0.3f, 1f, curveValue);

            elapsed += Time.deltaTime;
            yield return null;
        }
        isPlayingVignetteEffect = false;

        vignette.intensity.value = 0f;
    }

    [System.Serializable]
    struct ScreenShakeInstance
    {
        public float startTime;
        public float pulseDuration;
        float intensity;
        AnimationCurve intensityCurve;

        public ScreenShakeInstance(float intensity, float duration, AnimationCurve intensityCurve)
        {
            startTime = Time.time;
            this.pulseDuration = duration;
            this.intensity = intensity;
            this.intensityCurve = intensityCurve;
        }

        public float UpdateAndGetIntensity()
        {
            float t = (Time.time - this.startTime) / pulseDuration;

            return intensityCurve.Evaluate(t) * intensity;
        }

        public bool ShouldDie()
        {
            return (Time.time - this.startTime) > pulseDuration;
        }
    }

    private void UpdateScreenShake()
    {
        if (activeScreenShakes.Count == 0)
        {
            gameCamera.SetShakeOffset(Vector2.zero);
            return;
        }

        float resultIntensity = 0f;
        for (int i = activeScreenShakes.Count - 1; i >= 0; i--)
        {
            Debug.Log(i);
            if (activeScreenShakes[i].ShouldDie())
            {
                activeScreenShakes.RemoveAt(i);
                continue;
            }
            resultIntensity += activeScreenShakes[i].UpdateAndGetIntensity();
        }

        Debug.Log(resultIntensity);

        gameCamera.SetShakeOffset(Random.insideUnitCircle * resultIntensity);
    }
}
