
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsController : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Volume Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Percentage Text")]
    public TMP_Text musicPercentText;
    public TMP_Text sfxPercentText;

    private const string MUSIC_KEY = "MusicVolume";
    private const string SFX_KEY = "SFXVolume";

    private void Awake()
    {
        // Ambil volume yang tersimpan
        float music = PlayerPrefs.GetFloat(MUSIC_KEY, 0.8f);
        float sfx = PlayerPrefs.GetFloat(SFX_KEY, 0.7f);

        // Atur nilai awal slider
        musicSlider.SetValueWithoutNotify(music);
        sfxSlider.SetValueWithoutNotify(sfx);

        // Tampilkan persentase awal
        UpdateMusicText(music);
        UpdateSFXText(sfx);

        // Terapkan volume awal
        ApplyMusicVolume(music);
        ApplySFXVolume(sfx);

        // Hubungkan slider dengan fungsi
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    // Dipanggil setiap kali slider Music digeser
    public void SetMusicVolume(float value)
    {
        UpdateMusicText(value);
        ApplyMusicVolume(value);

        PlayerPrefs.SetFloat(MUSIC_KEY, value);
    }

    // Dipanggil setiap kali slider SFX digeser
    public void SetSFXVolume(float value)
    {
        UpdateSFXText(value);
        ApplySFXVolume(value);

        PlayerPrefs.SetFloat(SFX_KEY, value);
    }

    // Mengubah nilai Music menjadi persentase
    private void UpdateMusicText(float value)
    {
        int percent = Mathf.RoundToInt(value * 100);
        musicPercentText.text = percent + "%";
    }

    // Mengubah nilai SFX menjadi persentase
    private void UpdateSFXText(float value)
    {
        int percent = Mathf.RoundToInt(value * 100);
        sfxPercentText.text = percent + "%";
    }

    // Menerapkan volume Music
    private void ApplyMusicVolume(float value)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat(
                "MusicVolume",
                ConvertToDecibels(value)
            );
        }
    }

    // Menerapkan volume SFX
    private void ApplySFXVolume(float value)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat(
                "SFXVolume",
                ConvertToDecibels(value)
            );
        }
    }

    // Mengubah skala 0-1 menjadi desibel
    private float ConvertToDecibels(float value)
    {
        if (value <= 0.0001f)
            return -80f;

        return Mathf.Log10(value) * 20f;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
