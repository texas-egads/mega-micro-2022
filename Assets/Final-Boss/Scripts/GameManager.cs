using System;
using System.Collections;
using System.Collections.Generic;
using Final_Boss.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Final_Boss
{
    public class GameManager : MonoBehaviour
    {
        // Emit percentage of health
        public UnityEvent<float> playerHealthChanged;
        public UnityEvent<float> enemyHealthChanged;
        
        // Emit number of mana remaining
        public UnityEvent<int> playerManaChanged;
        public UnityEvent<int> enemyManaChanged;
        
        // Emit size of deck
        public UnityEvent<int> deckSizeChanged;

        public List<Card> deck = new List<Card>();
        public Transform[] cardSlots;
        public int roundLengthInSeconds = 30;
        
        public TMP_Text roundTimerText;

        private Card[] _cardsInHand;
        private int _selectedCardIndex;
        private int _playerHealth;
        private int _playerMana;
        private int _enemyHealth;
        private int _enemyMana;
        private Coroutine _roundTimer;
        [SerializeField] private int maxPlayerHealth = 30;
        [SerializeField] private int maxEnemyHealth = 30;

        private int PlayerHealth
        {
            get => _playerHealth;
            set
            {
                _playerHealth = value;
                playerHealthChanged.Invoke((float) _playerHealth / maxPlayerHealth);
            }
        }

        private int EnemyHealth
        {
            get => _enemyHealth;
            set
            {
                _enemyHealth = value;
                enemyHealthChanged.Invoke((float) _enemyHealth / maxEnemyHealth);
            }
        }

        private int PlayerMana
        {
            get => _playerMana;
            set
            {
                _playerMana = value;
                playerManaChanged.Invoke(value);
            }
        }

        private int EnemyMana
        {
            get => _enemyMana;
            set
            {
                _enemyMana = value;
                enemyManaChanged.Invoke(value);
            }
        }

        private void Start()
        {
            _cardsInHand = new Card[cardSlots.Length];

            Card.CardSelected += OnCardSelected;
            Card.CardUnselected += OnCardUnselected;

            foreach (var card in deck)
            {
                card.gameObject.SetActive(false);
            }
            
            deckSizeChanged.Invoke(deck.Count);

            PlayerHealth = maxPlayerHealth;
            EnemyHealth = maxEnemyHealth;

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
            PlayerMana += 1;
            EnemyMana += 1;

            _roundTimer = StartCoroutine(RunRoundTimer());
        }

        public void EndRound()
        {
            EvaluateRound();

            if (EnemyHealth <= 0)
            {
                Managers.MinigamesManager.DeclareCurrentMinigameWon();
                Managers.MinigamesManager.EndCurrentMinigame();
            }
            else if (PlayerHealth <= 0)
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

            // Not enough mana
            if (PlayerMana < cardDescriptor.manaCost) return;

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
                    return;
                }
            }
            
            RemoveCardAt(_selectedCardIndex);
        }

        private void HandleAttack(CardDescriptorAttack card)
        {
            EnemyHealth -= card.damage;
            PlayerMana -= card.manaCost;
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
                deckSizeChanged.Invoke(deck.Count);
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

        private void ReturnCardAt(int index)
        {
            if (index < 0) return;

            var card = _cardsInHand[index];
            _cardsInHand[index] = null;

            if (card == null) return;
            
            card.gameObject.SetActive(false);
            
            deck.Add(card);
            deckSizeChanged.Invoke(deck.Count);
        }

        private void ClearHand()
        {
            for (var i = 0; i < _cardsInHand.Length; ++i)
            {
                ReturnCardAt(i);
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