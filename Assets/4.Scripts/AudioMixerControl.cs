using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerControl : MonoBehaviour {
  public AudioMixer mixer;

  public void SetMasterVolume(float percentage) {
    mixer.SetFloat("MasterVolume", PercentageToVolume(percentage));
  }

  public void SetMusicVolume(float percentage) {
    mixer.SetFloat("MusicVolume", PercentageToVolume(percentage));
  }

  public void SetFXVolume(float percentage) {
    mixer.SetFloat("FXVolume", PercentageToVolume(percentage));
  }

  private static float PercentageToVolume(float percentage) {
    return Mathf.Log10(Mathf.Clamp(percentage, 0.0001f, 1f)) * 20f;
  }
}
