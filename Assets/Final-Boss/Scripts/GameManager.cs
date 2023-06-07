using System;
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
        
        public TMP_Text deckSizeText;
        
        private bool[] _availableCardSlots;

        private void Start()
        {
            _availableCardSlots = new bool[deck.Count];
            for (var i = 0; i < _availableCardSlots.Length; ++i)
            {
                _availableCardSlots[i] = true;
            }
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

            for (var i = 0; i < _availableCardSlots.Length; ++i)
            {
                if (_availableCardSlots[i] != true) continue;
                randomCard.gameObject.SetActive(true);
                randomCard.transform.position = cardSlots[i].position;
                _availableCardSlots[i] = false;
                deck.Remove(randomCard);
                return;
            }
        }
    }
}