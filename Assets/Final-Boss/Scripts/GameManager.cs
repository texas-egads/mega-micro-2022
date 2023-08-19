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

        public UnityEvent roundStarted;
        public UnityEvent cardSelected;
        public UnityEvent cardUnselected;

        //public List<Card> deck = new List<Card>();

        //this is the prefab objects for each
        public GameObject ClawSlash, FireBlast, ThunderStrike, WarlockShield, HealingChant, ShadowDodge, ArcaneCounter, ThisYourCard;
        public GameObject backOfCard;
        private List<Card> playerDeck = new List<Card>();
        private List<Card> enemyDeck = new List<Card>();
        public Transform playerDeckParent;
        public Transform enemyDeckParent;
        private int playerDeckIndex, enemyDeckIndex;


        public Transform[] cardSlots;
        public Transform[] enemyCardSlots;
        public int roundLengthInSeconds = 30;

        public Transform playerPile;
        public Transform enemyPile;
        
        public TMP_Text roundTimerText;
        public TMP_Text cardTitleText;
        public TMP_Text cardDescriptionText;

        private Card[] _cardsInHand;
        private Card[] _enemyCardsInHand;
        private int _selectedCardIndex;
        private int _playerHealth;
        private int _playerMana;
        private int _enemyHealth;
        private int _enemyMana;
        private Coroutine _roundTimer;
        [SerializeField] private int maxPlayerHealth = 30;
        [SerializeField] private int maxEnemyHealth = 30;

        private bool playerUsedMana, enemyUsedMana;
        private Card previousPlayerCard, previousEnemyCard;

        //UI stuff
        public TextMeshProUGUI playerHealthCounter, enemyHealthCounter;

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
                enemyHealthCounter.text = (_enemyHealth + " / " + maxEnemyHealth);
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
            _enemyCardsInHand = new Card[enemyCardSlots.Length];

            Card.CardSelected += OnCardSelected;
            Card.CardUnselected += OnCardUnselected;

            GenerateDecks();

            // foreach (var card in deck)
            // {
            //     card.gameObject.SetActive(false);
            // }
            
            deckSizeChanged.Invoke(playerDeck.Count);

            PlayerHealth = maxPlayerHealth;
            EnemyHealth = maxEnemyHealth;

            //lets wait a bit before the very beginning
            //StartCoroutine(DelayBeforeRound(2f));

            StartRound();
        }

        // private IEnumerator DelayBeforeRound(float seconds) {
        //     yield return new WaitForSeconds(seconds);
        //     StartRound();
        // }

        private void GenerateDecks()
        {
            //the player deck and enemy deck are generated here. the player deck has 15 cards:
            //2 of them must be claw slashes, 1 must be arcane counter. Additionally, 6 are randomly chosen between claw slash, fire blast, and thunder strike. and 6 are randomly chosen between warlock shield, healing chant, and shadow dodge.
            //generate the player deck:
            for(int i = 0; i < 2; i++) {
                playerDeck.Add(Instantiate(ClawSlash, playerDeckParent).GetComponent<Card>());
            }
            //now a for loop that runs 6 times. in this loop, randomly choose between claw slash, fire blast, and thunder strike:
            for(int i = 0; i < 6; i++) {
                int randomNum = Random.Range(0, 3);
                if(randomNum == 0) {
                    AddCard(ClawSlash, true);
                }
                else if(randomNum == 1) {
                    AddCard(FireBlast, true);
                }
                else if(randomNum == 2) {
                    AddCard(ThunderStrike, true);
                }
            }
            //now a loop that runs 6 time, randomly choose between warlock shield, healing chant, and shadow dodge:
            for(int i = 0; i < 6; i++) {
                int randomNum = Random.Range(0, 3);
                if(randomNum == 0) {
                    AddCard(WarlockShield, true);
                }
                else if(randomNum == 1) {
                    AddCard(HealingChant, true);
                }
                else if(randomNum == 2) {
                    AddCard(ShadowDodge, true);
                }
            }
            //now add arcane counter:
            AddCard(ArcaneCounter, true);

            //do the same thing for enemy deck, but instead of arcane counter, add this your card:
            for(int i = 0; i < 2; i++) {
                enemyDeck.Add(Instantiate(ClawSlash, enemyDeckParent).GetComponent<Card>());
            }
            //now a for loop that runs 6 times. in this loop, randomly choose between claw slash, fire blast, and thunder strike:
            for(int i = 0; i < 6; i++) {
                int randomNum = Random.Range(0, 3);
                if(randomNum == 0) {
                    AddCard(ClawSlash, false);
                }
                else if(randomNum == 1) {
                    AddCard(FireBlast, false);
                }
                else if(randomNum == 2) {
                    AddCard(ThunderStrike, false);
                }
            }
            //now a loop that runs 6 time, randomly choose between warlock shield, healing chant, and shadow dodge:
            for(int i = 0; i < 6; i++) {
                int randomNum = Random.Range(0, 3);
                if(randomNum == 0) {
                    AddCard(WarlockShield, false);
                }
                else if(randomNum == 1) {
                    AddCard(HealingChant, false);
                }
                else if(randomNum == 2) {
                    AddCard(ShadowDodge, false);
                }
            }
            //now add this your card:
            AddCard(ThisYourCard, false);
        }

        private void AddCard(GameObject card, bool isPlayer = true) {
            if(isPlayer) {
                playerDeck.Add(Instantiate(card, playerDeckParent).GetComponent<Card>());
            } else {
                enemyDeck.Add(Instantiate(card, enemyDeckParent).GetComponent<Card>());
            }
        }

        public void StartRound()
        {
            _selectedCardIndex = -1;
         //   ClearHand();

            int nullCount = 0;
            
            //for player:
            for(int i = 0; i < _cardsInHand.Length; i++) {
                if(_cardsInHand[i] == null) {
                    nullCount++;
                }
            }
            //Debug.Log(nullCount);
            var dealAmount = nullCount;
            for (var i = 0; i < dealAmount; ++i)
            {
                DrawCard(true);
            }

            //for enemy:
            nullCount = 0;
            for(int i = 0; i < _enemyCardsInHand.Length; i++) {
                if(_enemyCardsInHand[i] == null) {
                    nullCount++;
                }
            }
            //Debug.Log(nullCount);
            dealAmount = nullCount;
            for (var i = 0; i < dealAmount; ++i)
            {
                DrawCard(false);
            }



            // 1 mana per round
            if(!playerUsedMana) {
                PlayerMana = Mathf.Min(PlayerMana + 1, 3);
            }
            if(!enemyUsedMana) {
                EnemyMana = Mathf.Min(EnemyMana + 1, 3);
            }
            playerUsedMana = false;
            enemyUsedMana = false;

            _roundTimer = StartCoroutine(RunRoundTimer());
            roundStarted.Invoke();
        }

        public void EndRound()
        {
            EvaluateRound();

            if (EnemyHealth <= 0)
            {
                Debug.Log("WON!");
                Managers.MinigamesManager.DeclareCurrentMinigameWon();
                Managers.MinigamesManager.EndCurrentMinigame();
            }
            else if (PlayerHealth <= 0)
            {
                Debug.Log("LOST!");
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
            Debug.Log(playerDeck.ToString());   
            // Nothing selected
            if (_selectedCardIndex < 0 || !_cardsInHand[_selectedCardIndex]) return;

            var selectedCard = _cardsInHand[_selectedCardIndex];
            var cardDescriptor = selectedCard.cardDescriptor;

            // Not enough mana
            if (PlayerMana < cardDescriptor.manaCost)
            {
                selectedCard.UnselectCard();
                return;
            }

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
            //RecycleCardAt(_selectedCardIndex);
        }

        private void HandleAttack(CardDescriptorAttack card)
        {
            EnemyHealth = Mathf.Max(0, EnemyHealth - card.damage);
            PlayerMana -= card.manaCost;
            if(card.manaCost > 0) {
                playerUsedMana = true;
            }
        }

        private void HandleDefense(CardDescriptorDefense card)
        {
            PlayerMana -= card.manaCost;
            if(card.manaCost > 0) {
                playerUsedMana = true;
            }
        }

        private void HandleHeal(CardDescriptorHeal card)
        {
            PlayerHealth += card.healAmount;
            PlayerMana -= card.manaCost;
            if(card.manaCost > 0) {
                playerUsedMana = true;
            }
        }

        private void HandleStun(CardDescriptorStun card)
        {
            PlayerMana -= card.manaCost;
            playerUsedMana = true;
        }

        private void HandleCopy(CardDescriptorCopy card)
        {
            PlayerMana -= card.manaCost;
            if(card.manaCost > 0) {
                playerUsedMana = true;
            }
        }



        public void DrawCard(bool isPlayer = true)
        {
            

            if(isPlayer) {
                while(playerDeck[playerDeckIndex].gameObject.activeSelf) { //this loop will terminate once it finds card not already drawn
                    playerDeckIndex = (playerDeckIndex + 1) % playerDeck.Count;
                }
                var cardToDraw = playerDeck[playerDeckIndex];
                playerDeckIndex = (playerDeckIndex + 1) % playerDeck.Count;

                for (var i = 0; i < _cardsInHand.Length; ++i)
                {
                    if (_cardsInHand[i] != null) continue;

                    cardToDraw.gameObject.SetActive(true); //turns on card visibility
                    cardToDraw.DealCard(new Vector3(6.57f, -0.78f, 0), cardSlots[i].position, i); //just sets position
                    StartCoroutine(FlipCard(cardToDraw.GetComponent<Animator>(), 1.5f));
                    _cardsInHand[i] = cardToDraw; //add to card in hand
                    // playerDeck.Remove(cardToDraw); //removes from deck. lets change that back
                    deckSizeChanged.Invoke(playerDeck.Count); //deck size counter
                    return;
                }
            } else {
                while(enemyDeck[enemyDeckIndex].gameObject.activeSelf) { //this loop will terminate once it finds card not already drawn
                    enemyDeckIndex = (enemyDeckIndex + 1) % enemyDeck.Count;
                }
                var cardToDraw = enemyDeck[enemyDeckIndex];
                enemyDeckIndex = (enemyDeckIndex + 1) % enemyDeck.Count;

                for (var i = 0; i < _enemyCardsInHand.Length; ++i)
                {
                    if (_enemyCardsInHand[i] != null) continue;

                    cardToDraw.gameObject.GetComponent<Collider2D>().enabled = false; //bc dont want to interact w/ enemy cards
                    cardToDraw.gameObject.SetActive(true); //turns on card visibility
                    cardToDraw.DealCard(new Vector3(6.57f, 1.7f, 0), enemyCardSlots[i].position, i); //just sets position
                   // cardToDraw.GetComponent<Animator>().Play("cardflip");
                    _enemyCardsInHand[i] = cardToDraw; //add to card in hand
                    return;
                }
            }


            Debug.Log("No available card slots");
        }

        private IEnumerator FlipCard(Animator animator, float delay) {
            yield return new WaitForSeconds(delay);
            animator.Play("cardflip");
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

            cardTitleText.text = _cardsInHand[handIndex].cardDescriptor.cardName;
            cardDescriptionText.text = _cardsInHand[handIndex].cardDescriptor.cardDescription;

            cardSelected.Invoke();
        }

        private void OnCardUnselected(int handIndex)
        {
            // Already unselected
            if (handIndex != _selectedCardIndex) return;

            cardTitleText.text = "Select a card!";
            cardDescriptionText.text = "";

            _selectedCardIndex = -1;
            _cardsInHand[handIndex].UnselectCard();
            cardUnselected.Invoke();
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
            Debug.Log(index);
            if (index < 0) return;

            var card = _cardsInHand[index];
            _cardsInHand[index] = null;

            if (card == null) return;

            card.gameObject.SetActive(false);

          //  Destroy(card.gameObject);
        }


        //returns the cards from the hand to deck. for new shiz each time.
        private void ReturnCardAt(int index)
        {
            if (index < 0) return;

            var card = _cardsInHand[index];
            _cardsInHand[index] = null;

            if (card == null) return;
            
            card.gameObject.SetActive(false);
            
            playerDeck.Add(card);
            deckSizeChanged.Invoke(playerDeck.Count);
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