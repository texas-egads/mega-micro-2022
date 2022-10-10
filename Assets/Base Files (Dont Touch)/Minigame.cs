using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Minigame")]
public class Minigame : ScriptableObject
{    
    public enum GameTime
    {
        Short,
        Long
    }
    //******* ADD THESE IN THE INSPECTOR *******//
    [Tooltip("Length of your game.\n" +
             "    - Short: 2 measures of 140 BPM music (~3.6s)\n" +
             "    - Long: 4 measures of 140 BPM music (~6.8s)")]
    public GameTime gameTime;

    [Tooltip("The audio file for the music.")]
    public AudioClip music;

    [Tooltip("The volume scale of the music.")]
    [Range(0, 1)] public float volume = 1;

    [Tooltip("The list of possible sound effects that can be played during the minigame.")]
    public SoundAsset[] sounds;

    //*****************************************//

    //******* UPDATE THIS WHEN THE PLAYER WINS *******//
    [Tooltip("Update this via the MinigameManager when the player wins (or loses)." +
        "\n    - The default value at the start of your game is determined by this checkbox.")]
    public bool gameWin;
    //***********************************************//
}
