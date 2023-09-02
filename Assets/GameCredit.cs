using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameCredit : MonoBehaviour
{
    public MinigameDefinition def;
    public Image screenShot;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description; 
    // Start is called before the first frame update
    void Start()
    {
        screenShot.sprite = def.minigameScreenshot;
        title.text = def.title;
        description.text = def.creditsText;
        //set image native size
      //  screenShot.SetNativeSize();
    }

}
