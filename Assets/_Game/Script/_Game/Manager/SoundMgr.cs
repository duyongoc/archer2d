using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : Singleton<SoundMgr>
{
    //= inspector
    [Header("Setting")]
    [SerializeField] private SoundConfigSO config;
    [SerializeField] private Soundy soundyPrefab;

    [Header("Audio")]
    [SerializeField] private Transform musicAudio;
    [SerializeField] private Transform sfxAudio;


    //= private
    private static AudioSource audioMusic;
    private static AudioSource audioSFX;


    // music
    public static AudioClip MUSIC_BACKGROUND;

    // sfx
    public static AudioClip SFX_START_GAME;
    public static AudioClip SFX_TENSION_ARROW;
    public static AudioClip SFX_RELEASE_ARROW;

    public static AudioClip SFX_TARGET_HIT;
    public static AudioClip SFX_FRUIT_HIT;
    public static AudioClip SFX_SHOOT;
    public static AudioClip SFX_PERFECT_SHOOT;

    public static AudioClip SFX_GAMEOVER;
    public static AudioClip SFX_BG_TRANSITION;



    #region UNITY
    private void Start()
    {
        CacheComponent();
        CacheDefine();
    }

    // private void Update()
    // {
    // }
    #endregion


    public static void PlayMusic(AudioClip audi, bool loop = true)
    {
        audioMusic.clip = audi;
        audioMusic.loop = loop;
        audioMusic.Play();
    }

    public static void StopMusic()
    {
        audioMusic.Stop();
    }

    public void PlaySFX(AudioClip audi)
    {
        var sound = Instantiate(soundyPrefab, audioSFX.transform);
        sound.Play(audi);
    }


    public static void StopSFX()
    {
        audioSFX.Stop();
        audioSFX.clip = null;
    }

    public static void StopSFX(AudioClip clip)
    {
        audioSFX.Stop();
    }

    public static void PlaySFXOneShot(AudioClip clip)
    {
        audioSFX.PlayOneShot(clip);
    }

    public static void PlaySFXBlend(AudioClip clip, AudioSource audioSource)
    {
        audioSource.PlayOneShot(clip);
    }

    public static bool MusicPlaying(AudioClip audi)
    {
        return audioMusic.clip == audi && audioMusic.isPlaying;
    }

    public static bool SFXPlaying(AudioClip audi)
    {
        return audioSFX.clip == audi && audioSFX.isPlaying;
    }


    private void CacheDefine()
    {
        MUSIC_BACKGROUND = config.MUSIC_BACKGROUND;

        // sfx
        SFX_START_GAME = config.SFX_START_GAME;
        SFX_TENSION_ARROW = config.SFX_TENSION_ARROW;
        SFX_RELEASE_ARROW = config.SFX_RELEASE_ARROW;

        // character
        SFX_TARGET_HIT = config.SFX_TARGET_HIT;
        SFX_FRUIT_HIT = config.SFX_FRUIT_HIT;
        SFX_SHOOT = config.SFX_SHOOT;
        SFX_PERFECT_SHOOT = config.SFX_PERFECT_SHOOT;

        SFX_GAMEOVER = config.SFX_GAMEOVER;
        SFX_BG_TRANSITION = config.SFX_BG_TRANSITION;
    }

    private void CacheComponent()
    {
        audioMusic = musicAudio.GetComponent<AudioSource>();
        audioSFX = sfxAudio.GetComponent<AudioSource>();
    }
}
