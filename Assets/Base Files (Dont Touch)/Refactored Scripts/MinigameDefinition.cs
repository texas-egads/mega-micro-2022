using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Minigame Definition")]
public class MinigameDefinition : ScriptableObject
{    
    //******* ADD THESE IN THE INSPECTOR *******//
    [Tooltip("Put the scene name associated with your minigame here. Make sure it is prefixed with your team number!")]
    public string sceneName;

    [Tooltip("Put the title of your minigame here, as you want it to appear in the credits.")]
    public string title;

    [Tooltip("Leave this at Normal unless you've been preapproved to create a different type of minigame.")]
    public MinigameType minigameType;

    [Tooltip("Length of your game.\n" +
             "    - Short: 2 measures of 140 BPM music (~3.4s)\n" +
             "    - Medium: 3 measures of 140 BPM music (~5.1s)\n" +
             "    - Long: 4 measures of 140 BPM music (~6.9s)\n" +
             "    Do not select Uncapped unless you've been approved prior to do so")]
    public MinigameLength gameTime;

    [Tooltip("What you want the impact text leading into your game to say. This should be a short word or phrase telling the player what their goal is.")]
    public string instruction;

    [Tooltip("This will be displayed in the credits. List your members and your contributions here. You can include rich text if you want - it will be processed through TextMeshPro.")]
    [TextArea(5,10)]
    public string creditsText;

    [Tooltip("This will be displayed in the credits. Take an attractive screenshot of your finished game and put it here.")]
    public Sprite creditsScreenshot;

    //*****************************************//
}


public enum MinigameLength
{
    Short = 3429,
    Medium = 5143,
    Long = 6857,
    Uncapped = 0
}

public enum MinigameType
{
    Normal,
    Miniboss,
    Boss
}
