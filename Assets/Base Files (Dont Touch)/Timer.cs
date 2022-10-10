using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Minigame.GameTime gameTime;
    public Text text;
    
    public IEnumerator GameTimer(int time)
    {
        while (time >= 0)
        {
            text.text = time.ToString();
            time -= 1;
            yield return new WaitForSeconds(.42857f);
        }
        gameObject.SetActive(false);
    }
}
