using UnityEngine;
using UnityEngine.UI;

public class UI_Settings : MonoBehaviour
{
    public GameObject TerrainPrefab;
    MeshGenerator perlinNoiseScript;

    public Slider OctaveSlider;
    public Slider PersistenceSlider;
    public Slider FrequencySlider;
    public Slider AmplitudeSlider;
    public Slider YIntensitySlider;

    [Range(2, 12)] int octavesUI = 4;
    [Range(0, 1)] float persistenceUI = 0.5f;
    [Range(0.1f, 2f)] float baseFrequencyUI = 0.2f;
    [Range(0.1f, 2f)] float baseAmplitudeUI = 1.0f;
    [Range(2, 20)] float yIntensityUI = 10.0f;

    private void Start()
    {
        perlinNoiseScript = TerrainPrefab.GetComponent<MeshGenerator>();

        OctaveSlider.value = Normalize(octavesUI, 2, 12);
        PersistenceSlider.value = Normalize(persistenceUI, 0, 1);
        FrequencySlider.value = Normalize(baseFrequencyUI, 0.1f, 2f);
        AmplitudeSlider.value = Normalize(baseAmplitudeUI, 0.1f, 2f);
        YIntensitySlider.value = Normalize(yIntensityUI, 2, 20);
    }

    private void Update()
    {
        octavesUI = Mathf.RoundToInt(Denormalize(OctaveSlider.value, 2, 12));
        persistenceUI = Denormalize(PersistenceSlider.value, 0, 1);
        baseFrequencyUI = Denormalize(FrequencySlider.value, 0.1f, 2f);
        baseAmplitudeUI = Denormalize(AmplitudeSlider.value, 0.1f, 2f);
        yIntensityUI = Denormalize(YIntensitySlider.value, 2, 20);

        UpdatePrefabValues();
    }

    void UpdatePrefabValues()
    {
        perlinNoiseScript.octaves = octavesUI;
        perlinNoiseScript.persistence = persistenceUI;
        perlinNoiseScript.baseFrequency = baseFrequencyUI;
        perlinNoiseScript.baseAmplitude = baseAmplitudeUI;
        perlinNoiseScript.yIntensity = yIntensityUI;
    }

    private float Normalize(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }

    private float Denormalize(float value, float min, float max)
    {
        return (value * (max - min)) + min;
    }
}

