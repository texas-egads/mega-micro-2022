using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iDrowned
{

    public class BlinkGreen : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_Text guess1;
        [SerializeField] private TMPro.TMP_Text guess2;
        [SerializeField] private TMPro.TMP_Text guess3;

        private bool isGreen;

        [SerializeField] private float blinkTimer = .1f;

        [SerializeField] private Color color;

        public void Blink()
        {
            StartCoroutine(blinkRoutine());
            isGreen = !isGreen;
            if (isGreen)
            {
                guess1.color = color;
                guess2.color = color;
                guess3.color = color;
            }
            else
            {
                guess1.color = Color.white;
                guess2.color = Color.white;
                guess3.color = Color.white;
            }
        }

        IEnumerator blinkRoutine()
        {
            yield return new WaitForSeconds(blinkTimer);
            Blink();
        }

    }
}