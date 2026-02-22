using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PPManager : MonoBehaviour
{
    [SerializeField] private Volume volume;        // Assign Global Volume in Inspector
    [SerializeField] private float lerpSpeed = 2f; // How fast to interpolate

    private Vignette vignette;
    private float targetIntensity = 0f;

    void Start()
    {
        // Try to get the Vignette override from the volume profile
        if (volume.profile.TryGet(out Vignette v))
        {
            vignette = v;
            targetIntensity = vignette.intensity.value; // start from current value
        }
    }

    void Update()
    {
        if (vignette != null)
        {
            // Smoothly interpolate toward target intensity
            vignette.intensity.value = Mathf.Lerp(
                vignette.intensity.value,
                targetIntensity,
                Time.deltaTime * lerpSpeed
            );
        }
    }

    // Call this to set a new target intensity (0 = none, 1 = max)
    public void SetVignetteIntensity(float intensity)
    {
        targetIntensity = Mathf.Clamp01(intensity);
    }
}