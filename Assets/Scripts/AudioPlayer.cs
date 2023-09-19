using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private string audioFolderPath;
    private AudioSource musicPlayer;
    private AudioSource jinglePlayer;
    private AudioSource sfxPlayer;

    private void Awake()
    {
        audioFolderPath = "Audio/";
        musicPlayer = gameObject.AddComponent<AudioSource>();
        sfxPlayer = gameObject.AddComponent<AudioSource>();
        jinglePlayer = gameObject.AddComponent<AudioSource>();

        string mainMusicClipName = "BrinstarTheme";
        playMusicClip(mainMusicClipName, true);
    }

    // Finds AudioClip with given name in Audio folder and plays it
    // using sfxPlayer
    public void playSfxClip(string clipName)
    {
        string clipPath = audioFolderPath + clipName;
        AudioClip clip = Resources.Load<AudioClip>(clipPath);

        sfxPlayer.clip = clip;
        sfxPlayer.Play();
    }

    // Finds AudioClip with given name in Audio folder and plays it
    // using musicPlayer; loops clip if loop parameter is true
    public void playMusicClip(string clipName, bool loop)
    {
        string clipPath = audioFolderPath + clipName;
        AudioClip clip = Resources.Load<AudioClip>(clipPath);

        musicPlayer.clip = clip;
        musicPlayer.loop = loop;
        musicPlayer.Play();
    }

    // Pauses music, finds and plays given AudioClip, then resumes music
    // once given AudioClip is finished playiing
    public void playJingle(string clipName)
    {
        string clipPath = audioFolderPath + clipName;
        AudioClip clip = Resources.Load<AudioClip>(clipPath);
        jinglePlayer.clip = clip;

        musicPlayer.Pause();
        jinglePlayer.Play();

        // Waits slightly longer than the duration of the clip before
        // resuming music
        StartCoroutine(ResumeMusic(clip.length + 0.5f));
    }

    IEnumerator ResumeMusic(float duration)
    {
        yield return new WaitForSeconds(duration);

        musicPlayer.UnPause();
    }
}
