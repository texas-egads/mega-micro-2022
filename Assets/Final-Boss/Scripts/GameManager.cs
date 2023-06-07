using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Final_Boss
{
    public class GameManager : MonoBehaviour
    {
        public List<Card> deck = new List<Card>();
        public Transform[] cardSlots;
        public bool[] availableCardSlots;

        public TMP_Text deckSizeText;

        private void OnEnable()
        {
           
        }

        private void Update()
        {
            deckSizeText.text = deck.Count.ToString();
        }

        public void DrawCard()
        {
            if (deck.Count < 1)
            {
                return;
            }

            var randomCard = deck[Random.Range(0, deck.Count)];

            for (var i = 0; i < availableCardSlots.Length; ++i)
            {
                if (availableCardSlots[i] != true) continue;
                randomCard.gameObject.SetActive(true);
                randomCard.transform.position = cardSlots[i].position;
                availableCardSlots[i] = false;
                deck.Remove(randomCard);
                return;
            }
        }
    }
}