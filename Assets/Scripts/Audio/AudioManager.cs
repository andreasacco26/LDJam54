using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<Sound> musicSounds, ambienceSounds, sfxSounds;

    public AudioSource musicSource, ambienceSource, sfxSource;

    public static AudioManager Instance { get; private set; }

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Debug.Log("AudioManager Singleton created");
            Instance = this;
        }
    }

    private void Play(List<Sound> sounds, AudioSource source, string clipName, bool loop) {
        Sound sound = sounds.Find((x) => x.name == clipName);
        if (sound == null) {
            Debug.Log("Cannot reproduce sound: " + clipName + " not found!");
            return;
        }
        source.clip = sound.clip;
        source.loop = loop;
        source.Play();
    }

    public void PlayMusic(string musicName) => Play(musicSounds, musicSource, musicName, true);
    public void PlayAmbience(string ambienceName) => Play(ambienceSounds, ambienceSource, ambienceName, true);
    public void PlaySfx(string sfxName) {
        Sound sound = sfxSounds.Find((x) => x.name == sfxName);
        if (sfxName == null) {
            Debug.Log("Cannot reproduce sound: " + sfxName + " not found!");
            return;
        }
        sfxSource.PlayOneShot(sound.clip);
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
