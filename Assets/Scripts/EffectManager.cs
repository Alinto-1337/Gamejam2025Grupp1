using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class EffectManager : MonoBehaviour
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


    public enum EffectPower
    {
        aggressive,
        normal
    }

    private void Awake()
    {
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
        if (Input.GetKeyDown(KeyCode.K))
        {

            PlayScreenShakePulse(5, EffectPower.aggressive);

            //PlayVignettePulse(0.8f, new Color(.9f, 0, 0), EffectPower.aggressive);
        }
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
        StartCoroutine(PlayScreenShakePulseRoutine(intensity, power));
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
    

    private IEnumerator PlayScreenShakePulseRoutine(float intensity, EffectPower power = EffectPower.aggressive)
    {
        float elapsed = 0f;


        AnimationCurve pulseCurve = power == EffectPower.aggressive ? aggressivePulseCurve : normalPulseCurve;
        float pulseDuration = power == EffectPower.aggressive ? aggressivePulseDuration : normalPulseDuration;

        while (elapsed < pulseDuration)
        {
            float t = elapsed / pulseDuration;
            float curveValue = pulseCurve.Evaluate(t) * intensity;

            gameCamera.SetShakeOffset(Random.insideUnitCircle * curveValue);

            elapsed += Time.deltaTime;
            yield return null;
        }


        vignette.intensity.value = 0f;
    }
}
