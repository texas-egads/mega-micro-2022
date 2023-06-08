using System;
using System.Collections;
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
        public int roundLengthInSeconds = 30;

        public TMP_Text deckSizeText;
        public TMP_Text roundTimerText;

        private bool[] _availableCardSlots;
        private int _currentPlayerHealth;
        private int _currentEnemyHealth;
        private Coroutine _roundTimer;

        private void Start()
        {
            _availableCardSlots = new bool[cardSlots.Length];
            for (var i = 0; i < _availableCardSlots.Length; ++i)
            {
                _availableCardSlots[i] = true;
            }

            deckSizeText.text = deck.Count.ToString();

            Card.CardPlayed += OnCardPlayed;

            foreach (var card in deck)
            {
                card.gameObject.SetActive(false);
            }
            
            // TODO: REMOVE
            _roundTimer = StartCoroutine(RunRoundTimer());
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
                if (_availableCardSlots[i] == false) continue;

                randomCard.gameObject.SetActive(true);
                randomCard.transform.position = cardSlots[i].position;
                randomCard.DealCard(i);
                _availableCardSlots[i] = false;
                deck.Remove(randomCard);
                deckSizeText.text = deck.Count.ToString();
                return;
            }
            
            
            Debug.Log("No available card slots");
        }

        private void OnCardPlayed(int handIndex)
        {
            Debug.Log($"Card {handIndex} was played");
            _availableCardSlots[handIndex] = true;
        }

        public void OnConfirmRoundPressed()
        {
            if (_roundTimer == null) return;

            var roundTimer = _roundTimer;
            _roundTimer = null;
            StopCoroutine(roundTimer);
            roundTimerText.gameObject.SetActive(false);
        }

        private IEnumerator RunRoundTimer()
        {
            var timeLeft = roundLengthInSeconds;
            roundTimerText.gameObject.SetActive(true);

            while (timeLeft > 0)
            {
                roundTimerText.text = timeLeft.ToString();
                timeLeft -= 1;
                yield return new WaitForSeconds(1);
            }

            roundTimerText.text = "0";
            roundTimerText.gameObject.SetActive(false);
        }
    }
}