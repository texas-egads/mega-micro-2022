using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Final_Boss.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Final_Boss
{
    public class GameManager : MonoBehaviour
    {
        // Emit percentage of health
        public UnityEvent<float> playerHealthChanged;
        public UnityEvent<float> enemyHealthChanged;
        
        public List<Card> deck = new List<Card>();
        public Transform[] cardSlots;
        public int roundLengthInSeconds = 30;

        public TMP_Text deckSizeText;
        public TMP_Text roundTimerText;

        private Card[] _cardsInHand;
        private int _selectedCardIndex;
        [SerializeField] private int playerHealth = 30;
        private int _currentPlayerMana = 0;
        [SerializeField] private int enemyHealth = 30;
        private int _currentEnemyMana = 0;
        private Coroutine _roundTimer;
        private int _maxPlayerHealth;
        private int _maxEnemyHealth;

        private void Start()
        {
            _cardsInHand = new Card[cardSlots.Length];

            Card.CardSelected += OnCardSelected;
            Card.CardUnselected += OnCardUnselected;

            foreach (var card in deck)
            {
                card.gameObject.SetActive(false);
            }

            deckSizeText.text = deck.Count.ToString();

            _maxPlayerHealth = playerHealth;
            _maxEnemyHealth = enemyHealth;

            StartRound();
        }

        public void StartRound()
        {
            _selectedCardIndex = -1;
            ClearHand();

            var dealAmount = Math.Min(_cardsInHand.Length, deck.Count);
            for (var i = 0; i < dealAmount; ++i)
            {
                DrawCard();
            }

            // 1 mana per round
            _currentPlayerMana += 1;
            _currentEnemyMana += 1;

            _roundTimer = StartCoroutine(RunRoundTimer());
        }

        public void EndRound()
        {
            EvaluateRound();

            if (enemyHealth <= 0)
            {
                Managers.MinigamesManager.DeclareCurrentMinigameWon();
                Managers.MinigamesManager.EndCurrentMinigame();
            }
            else if (playerHealth <= 0)
            {
                Managers.MinigamesManager.DeclareCurrentMinigameLost();
                Managers.MinigamesManager.EndCurrentMinigame();
            }
            else
            {
                StartRound();
            }
        }

        private void EvaluateRound()
        {
            // Nothing selected
            if (_selectedCardIndex < 0 || !_cardsInHand[_selectedCardIndex]) return;

            var selectedCard = _cardsInHand[_selectedCardIndex];
            var cardDescriptor = selectedCard.cardDescriptor;

            switch (cardDescriptor)
            {
                case CardDescriptorAttack attackCard:
                {
                    HandleAttack(attackCard);
                    break;
                }
                case CardDescriptorDefense defenseCard:
                {
                    HandleDefense(defenseCard);
                    break;
                }
                case CardDescriptorHeal healCard:
                {
                    HandleHeal(healCard);
                    break;
                }
                case CardDescriptorStun stunCard:
                {
                    HandleStun(stunCard);
                    break;
                }
                case CardDescriptorCopy copyCard:
                {
                    HandleCopy(copyCard);
                    break;
                }
                default:
                {
                    Debug.LogError("Could not cast card descriptor");
                    break;
                }
            }
        }

        private void DamageEnemy(int damage)
        {
            enemyHealth -= damage;
            enemyHealthChanged.Invoke((float) enemyHealth/_maxEnemyHealth);
        }

        private void DamagePlayer(int damage)
        {
            playerHealth -= damage;
            playerHealthChanged.Invoke((float) playerHealth / _maxPlayerHealth);
        }

        private void HandleAttack(CardDescriptorAttack card)
        {
            DamageEnemy(card.damage);
            _currentPlayerMana -= card.manaCost;
        }

        private void HandleDefense(CardDescriptorDefense card)
        {
        }

        private void HandleHeal(CardDescriptorHeal card)
        {
        }

        private void HandleStun(CardDescriptorStun card)
        {
        }

        private void HandleCopy(CardDescriptorCopy card)
        {
        }

        public void DrawCard()
        {
            if (deck.Count < 1)
            {
                Debug.Log("Out of cards in deck");
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

            EndRound();
        }

        private void RemoveCardAt(int index)
        {
            if (index < 0) return;

            var card = _cardsInHand[index];
            _cardsInHand[index] = null;

            if (card == null) return;

            Destroy(card.gameObject);
        }

        private void ClearHand()
        {
            for (var i = 0; i < _cardsInHand.Length; ++i)
            {
                RemoveCardAt(i);
            }
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

            EndRound();
        }
    }
}