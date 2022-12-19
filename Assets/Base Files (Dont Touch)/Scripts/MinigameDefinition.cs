using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Minigame Definition")]
public class MinigameDefinition : ScriptableObject
{    
    //******* ADD THESE IN THE INSPECTOR *******//
    [Tooltip("Put the scene name associated with your minigame here. Make sure your scene name has the right format, including your team number!")]
    public string sceneName;
    [Tooltip("Leave this at Normal unless you've been preapproved to create a different type of minigame.")]
    public MinigameType minigameType;

    [Tooltip("Length of your game.\n" +
             "    - Short: 12 beats of music at 140 BPM (~5.1s)\n" +
             "    - Medium: 16 beats of music at 140 BPM (~6.9s)\n" +
             "    - Long: 24 beats of music at 140 BPM (10.29s)\n" +
             "    Do not select Uncapped unless you've been approved prior to do so")]
    public MinigameLength gameTime;

    [Tooltip("What you want the impact text leading into your game to say. This should be a short word or phrase telling the player what their goal is.")]
    public string instruction;

    [Header("Credits")]

    [Tooltip("Put the title of your minigame here, as you want it to appear in the credits.")]
    public string title;

    [Tooltip("This will be displayed in the credits. List your members and your contributions here. You can include rich text if you want - it will be processed through TextMeshPro.")]
    [TextArea(5,10)]
    public string creditsText;

    [Tooltip("This will be displayed in the credits. Take an attractive screenshot of your finished game and put it here.")]
    public Sprite minigameScreenshot;

    //*****************************************//
}


// The length of a minigame. If you convert this to an int it represents the time in milliseconds.
public enum MinigameLength
{
    Short = 5143,
    Medium = 6857,
    Long = 10286,
    Uncapped = 0
}

public enum MinigameType
{
    Normal,
    Miniboss,
    Boss
}
