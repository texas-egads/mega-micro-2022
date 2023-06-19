using TMPro;
using UnityEngine;

namespace Final_Boss.Scripts
{
    public class IntToTMPText : MonoBehaviour
    {
        public TMP_Text tmp;
        
        public void OnIntToText(int value)
        {
            tmp.text = value.ToString();
        }
    }
}
