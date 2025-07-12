using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    // Audio sources for music and sound effects
    [Header("------- Audio Source -------")]
    [SerializeField] AudioSource musicSource; // AudioSource for background music
    [SerializeField] AudioSource SFXSource; // AudioSource for sound effects

    // Audio clips for different sounds
    [Header("------- Audio Clip -------")]
    public AudioClip background; // Background music
    public AudioClip buttonClick; // Sound for button click
    public AudioClip keyDown; // when key pressed
    public AudioClip mouseHover; // mousehover

    public AudioClip touchPickups; // touch skills
    public AudioClip touchTrap; // touch trap
    public AudioClip touchHealth; // touch a healthpack
    public AudioClip touchAmmo; //touch ammo

    // public AudioClip touchStar; // Sound for touching a star
    // public AudioClip touchTriangle; // Sound for touching a triangle

    public AudioClip creditsMusic; // Credits panel music
    public AudioClip startMusic; // Start panel music
    public AudioClip tutorialMusic; // Tutorial panel muic
    public AudioClip whenWin; // Win panel
    public AudioClip whenLose; //Lose panel

    // Volume control sliders for adjusting audio levels
    [Header("------- Volume Controls -------")] 
    public Slider musicVolumeSlider; // Slider to control the volume of background music
    public Slider sfxVolumeSlider; // Slider to control the volume of sound effects

    private void Start()
    {
        // Set the background music clip and start playing it
        PlayMusic(background);

        // Set initial slider values and listeners
        if (musicVolumeSlider != null)
        {
            // Set the slider's value to match the current volume of the music source
            musicVolumeSlider.value = musicSource.volume;
            // Add a listener to update the music volume when the slider value changes
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxVolumeSlider != null)
        {
            // Set the slider's value to match the current volume of the sound effects source
            sfxVolumeSlider.value = SFXSource.volume;
            // Add a listener to update the SFX volume when the slider value changes
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        // Load previously saved volume settings from PlayerPrefs
        // Default volume is set to 1 if no saved value is found
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        SFXSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }


    // Function to play specific music
    public void PlayMusic(AudioClip music)
    {
        // Log the music being played
        Debug.Log("Playing music: " + music.name);
        // Set the music clip
        musicSource.clip = music;
        // Enable looping for the music
        musicSource.loop = true;
        // Start playing the music
        musicSource.Play();
    }


    // Function to play sound effects
    public void PlaySFX(AudioClip clip)
    {
        // Log the name of the sound effect being played
        Debug.Log("Playing sound effect: " + clip.name);
        // Play the sound effect once
        SFXSource.PlayOneShot(clip);
    }


    // Function to set music volume
    public void SetMusicVolume(float volume)
    {
        // Update the volume of the music source
        musicSource.volume = volume;
        // Log the new music volume for debugging purposes
        Debug.Log("Music Volume set to: " + volume);
        // Optionally, save the new volume setting to PlayerPrefs for future sessions
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }


    // Function to set SFX volume
    public void SetSFXVolume(float volume)
    {
        // Update the volume of the SFX source
        SFXSource.volume = volume;
        // Log the new SFX volume for debugging purposes
        Debug.Log("SFX Volume set to: " + volume);
        // Optionally, save the new volume setting to PlayerPrefs for future sessions
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}



//how to use
//AudioManager audioManager;

//put this at the start

/*
audioManager = FindObjectOfType<AudioManager>();
if (audioManager == null)
{
    Debug.LogError("AudioManager not found!");
}
*/

// audioManager.PlaySFX(audioManager.keyDown);
