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

        private Card[] _cardsInHand;
        private int _selectedCardIndex;
        private int _currentPlayerHealth;
        private int _currentEnemyHealth;
        private Coroutine _roundTimer;

        private void Start()
        {
            _cardsInHand = new Card[cardSlots.Length];

            deckSizeText.text = deck.Count.ToString();

            Card.CardSelected += OnCardSelected;
            Card.CardUnselected += OnCardUnselected;

            foreach (var card in deck)
            {
                card.gameObject.SetActive(false);
            }

            _selectedCardIndex = -1;
            
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

            for (var i = 0; i < _cardsInHand.Length; ++i)
            {
                if (_cardsInHand[i] != null) continue;

                randomCard.gameObject.SetActive(true);
                randomCard.DealCard(cardSlots[i].position, i);
                _cardsInHand[i] = randomCard;
                deck.Remove(randomCard);
                deckSizeText.text = deck.Count.ToString();
                return;
            }
            
            
            Debug.Log("No available card slots");
        }

        private void OnCardSelected(int handIndex)
        {
            Debug.Log($"Card {handIndex} was played");

            if (_selectedCardIndex >= 0 && _cardsInHand[_selectedCardIndex])
            {
                _cardsInHand[_selectedCardIndex].UnselectCard();
            }

            _selectedCardIndex = handIndex;
            _cardsInHand[handIndex].SelectCard();
        }

        private void OnCardUnselected(int handIndex)
        {
            // Already unselected
            if (handIndex != _selectedCardIndex) return;

            _selectedCardIndex = -1;
            _cardsInHand[handIndex].UnselectCard();
        }

        public void OnConfirmRoundPressed()
        {
            if (_roundTimer == null) return;

            StopCoroutine(_roundTimer);
            _roundTimer = null;
            
            roundTimerText.gameObject.SetActive(false);
            
            EvaluateRound();
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
            
            EvaluateRound();
        }

        private void EvaluateRound()
        {
            
        }
    }
}