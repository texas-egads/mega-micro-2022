using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Terry
{
    public class TitleScreen : MonoBehaviour
    {
        // Start is called before the first frame update
        public void StartButton()
        {
            print("start");
            GameManager.Instance.LoadScene("Main");
        }

        // Update is called once per frame
        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
