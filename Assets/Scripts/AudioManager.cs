using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Dictionary<string, Sound> musicSounds, ambienceSounds, sfxSounds;
    public AudioSource musicSource, ambienceSource, sfxSource;

    public static AudioManager Instance { get; private set; }

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Debug.Log("UnitManager Singleton created");
            Instance = this;
        }
    }

    private void Play(Dictionary<string, Sound> sounds, AudioSource source, string clipName) {
        if (!sounds.ContainsKey(clipName)) {
            Debug.Log("Cannot reproduce sound: " + clipName + " not found!");
            return;
        }
        source.clip = sounds[clipName].clip;
        source.Play();
    }

    public void PlayMusic(string musicName) => Play(musicSounds, musicSource, musicName);
    public void PlayAmbience(string ambienceName) => Play(ambienceSounds, ambienceSource, ambienceName);
    public void PlaySfx(string sfxName) {
        if (!sfxSounds.ContainsKey(sfxName)) {
            Debug.Log("Cannot reproduce sound: " + sfxName + " not found!");
            return;
        }
        sfxSource.PlayOneShot(sfxSounds[sfxName].clip);
    }

    private void ToggleSource(AudioSource source) => source.mute = !source.mute;
    public void ToggleMusic() => ToggleSource(musicSource);
    public void ToggleAmbience() => ToggleSource(ambienceSource);
    public void ToggleSfx() => ToggleSource(sfxSource);

    private void Volume(AudioSource source, float volume) => source.volume = volume;
    public void MusicVolume(float volume) => Volume(musicSource, volume);
    public void AmbienceVolume(float volume) => Volume(ambienceSource, volume);
    public void SfxVolume(float volume) => Volume(sfxSource, volume);
}
