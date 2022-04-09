using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound", menuName = "Setting/Sound")]
public class SoundConfigSO : ScriptableObject
{

    [Header("Music")]
    public AudioClip MUSIC_BACKGROUND;

    [Header("SFX", order = 1)]
    public AudioClip SFX_START_GAME;
    public AudioClip SFX_TENSION_ARROW;
    public AudioClip SFX_RELEASE_ARROW;
    
    [Space(10)]
    public AudioClip SFX_TARGET_HIT;
    public AudioClip SFX_FRUIT_HIT;
    public AudioClip SFX_SHOOT;
    public AudioClip SFX_PERFECT_SHOOT;

    [Space(10)]
    public AudioClip SFX_GAMEOVER;
    public AudioClip SFX_BG_TRANSITION;



}
