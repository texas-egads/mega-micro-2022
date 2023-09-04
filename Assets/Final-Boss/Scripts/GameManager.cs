using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Final_Boss.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
        
        public GameObject timerIcon;
        public TMP_Text roundTimerText;
        public TMP_Text cardTitleText;
        public TMP_Text cardDescriptionText;

        private Card[] _cardsInHand;
        private Card[] _enemyCardsInHand;
        private int _selectedCardIndex;
        private int _enemySelectedCardIndex;
        private int _playerHealth;
        private int _playerMana;
        private int _enemyHealth;
        private int _enemyMana;
        private Coroutine _roundTimer;
        [SerializeField] private int maxPlayerHealth = 30;
        [SerializeField] private int maxEnemyHealth = 30;

        private bool playerUsedMana, enemyUsedMana;
        private Card previousPlayerCard, previousEnemyCard;

        private bool evaluatingRound;

        private bool playerStunned;

        private bool resetMana;


        private int playerAtk, playerDef;
        private int enemyAtk, enemyDef;
        private int playerHeal, enemyHeal;

        //UI stuff
        public TextMeshProUGUI playerHealthCounter, enemyHealthCounter;
        public TextMeshProUGUI playerDamageMarker, enemyDamageMarker;

        public GameObject playerAudioParent;
        public GameObject enemyAudioParent;
        
        public Animator blackFade;

        public AudioSource bgMusic, timerSound;

        public Button playButton, skipButton;

        public Image leftX, middleX, rightX;

        private int PlayerHealth
        {
            get => _playerHealth;
            set
            {
                _playerHealth = value;
                playerHealthChanged.Invoke((float) _playerHealth / maxPlayerHealth);

                //string of _playerHealth
                playerHealthCounter.text = (_playerHealth) + ""; 
            }
        }

        private int EnemyHealth
        {
            get => _enemyHealth;
            set
            {
                _enemyHealth = value;
                enemyHealthChanged.Invoke((float) _enemyHealth / maxEnemyHealth);
                enemyHealthCounter.text = (_enemyHealth) + ""; //  + " / " + maxEnemyHealth);
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
            timerIcon.SetActive(false);
            roundTimerText.gameObject.SetActive(false);
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
            StartCoroutine(DelayBeforeRound(2f));

            //StartRound();
        }

        private IEnumerator DelayBeforeRound(float seconds) {
            yield return new WaitForSeconds(seconds);
            StartRound();
        }

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


            //randomly shuffle playerdeck, shuffle from the third element to the second to last element:
            for(int i = 2; i < 50; i++) {
                int randomNum = Random.Range(2, playerDeck.Count);
                int randomNum2 = Random.Range(2, playerDeck.Count);
                Card temp = playerDeck[randomNum2];
                playerDeck[randomNum2] = playerDeck[randomNum];
                playerDeck[randomNum] = temp;
            }



            //do the same thing for enemy deck, but instead of arcane counter, add this your card:
            for(int i = 0; i < 2; i++) {
                enemyDeck.Add(Instantiate(ClawSlash, enemyDeckParent).GetComponent<Card>());
            }
            //now a for loop that runs 6 times. in this loop, randomly choose between claw slash, fire blast, and thunder strike:
            for(int i = 0; i < 6; i++) {
                int randomNum = Random.Range(0, 10);
                if(randomNum <= 6) {
                    AddCard(ClawSlash, false);
                }
                else if(randomNum <= 8) {
                    AddCard(FireBlast, false);
                }
                else if(randomNum == 9) {
                    AddCard(ThunderStrike, false);
                }
            }
            //now a loop that runs 6 time, randomly choose between warlock shield, healing chant, and shadow dodge:
            for(int i = 0; i < 6; i++) {
                int randomNum = Random.Range(0, 4);
                if(randomNum <= 1) {
                    AddCard(WarlockShield, false);
                }
                else if(randomNum == 2) {
                    AddCard(HealingChant, false);
                }
                else if(randomNum == 3) {
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
            cardTitleText.text = "Select a card!";
            cardDescriptionText.text = "";
            _selectedCardIndex = -1;

            int nullCount = 0;
            
            //for player:
            for(int i = 0; i < _cardsInHand.Length; i++) {
                if(_cardsInHand[i] == null) {
                    nullCount++;
                }
            }
            Debug.Log("null count: " + nullCount);
            var dealAmount = nullCount;
            for (var i = 0; i < dealAmount; ++i)
            {
                DrawCard(true);
            }

            if(playerStunned) {
                StartCoroutine(HandlePlayerStunned());
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

            StartCoroutine(RevealEnemyCards(2));

            //turn all of the player card colliders on
            for(int i = 0; i < _cardsInHand.Length; i++) {
                if(_cardsInHand[i] != null) {
                    _cardsInHand[i].GetComponent<Collider2D>().enabled = true;
                }
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

            SelectEnemyCard();

            _roundTimer = StartCoroutine(RunRoundTimer());
            roundStarted.Invoke();
        }

        private IEnumerator HandlePlayerStunned() {
            yield return new WaitForSeconds(1);
            leftX.gameObject.SetActive(true);
            middleX.gameObject.SetActive(true);
            rightX.gameObject.SetActive(true);

            cardTitleText.text = "Stunned! Cannot play a card :(";
            cardDescriptionText.text = "";
            
            playButton.interactable = false;

        }

        private void SelectEnemyCard() {
            //randomly select a card from enemy hand. if not enough mana, randomly select from the other two cards. if not enough again, select the last card. if that doesnt have enough, just set _enemySelectedCardIndex to -1
            int randomNum = Random.Range(0, 3);
            if(randomNum == 0) {
                if(EnemyMana >= _enemyCardsInHand[0].cardDescriptor.manaCost) {
                    _enemySelectedCardIndex = 0;
                } else if(EnemyMana >= _enemyCardsInHand[1].cardDescriptor.manaCost) {
                    _enemySelectedCardIndex = 1;
                } else if(EnemyMana >= _enemyCardsInHand[2].cardDescriptor.manaCost) {
                    _enemySelectedCardIndex = 2;
                } else {
                    _enemySelectedCardIndex = -1;
                }
            } else if(randomNum == 1) {
                if(EnemyMana >= _enemyCardsInHand[1].cardDescriptor.manaCost) {
                    _enemySelectedCardIndex = 1;
                } else if(EnemyMana >= _enemyCardsInHand[2].cardDescriptor.manaCost) {
                    _enemySelectedCardIndex = 2;
                } else if(EnemyMana >= _enemyCardsInHand[0].cardDescriptor.manaCost) {
                    _enemySelectedCardIndex = 0;
                } else {
                    _enemySelectedCardIndex = -1;
                }
            } else if(randomNum == 2) {
                if(EnemyMana >= _enemyCardsInHand[2].cardDescriptor.manaCost) {
                    _enemySelectedCardIndex = 2;
                } else if(EnemyMana >= _enemyCardsInHand[0].cardDescriptor.manaCost) {
                    _enemySelectedCardIndex = 0;
                } else if(EnemyMana >= _enemyCardsInHand[1].cardDescriptor.manaCost) {
                    _enemySelectedCardIndex = 1;
                } else {
                    _enemySelectedCardIndex = -1;
                }
            }
        }

        IEnumerator EndBoss(bool won) {
            //for black fade, play animation titled FadeOut
            blackFade.gameObject.SetActive(true);
            blackFade.Play("FadeOut");
            //fade out bgMusic and timerSound over 2 seconds
            StartCoroutine(FadeOut(bgMusic, 2f));
            yield return new WaitForSeconds(2.5f);
            if(won) {
                //go to scene called WinScreen. LoadScene
                ScenesManager.LoadSceneImmediateStatic("WinScreen");
            } else {
                ScenesManager.LoadSceneImmediateStatic("LoseScreen");
            }


            
        }

        IEnumerator FadeOut(AudioSource audioSource, float FadeTime) {
            float startVolume = audioSource.volume;
            while(audioSource.volume > 0) {
                audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
                yield return null;
            }
            audioSource.Stop();
            audioSource.volume = startVolume;
        }

    

        public void EndRound()
        {
            //set skip
            skipButton.interactable = false;
            playButton.interactable = false;

            if(playerStunned) {
                playerStunned = false;
                leftX.gameObject.SetActive(false);
                middleX.gameObject.SetActive(false);
                rightX.gameObject.SetActive(false);
            }
                    
            EvaluateRound(); //this does all the fight logic
            Debug.Log("YOOO");

        
            // while(evaluatingRound) {
            //     //stay here
            // }



            // if (EnemyHealth <= 0)
            // {
            //     Debug.Log("WON!");
            //     Managers.MinigamesManager.DeclareCurrentMinigameWon();
            //     Managers.MinigamesManager.EndCurrentMinigame();
            //     StartCoroutine(EndBoss(true));
            // }
            // else if (PlayerHealth <= 0)
            // {
            //     Debug.Log("LOST!");
            //     Managers.MinigamesManager.DeclareCurrentMinigameLost();
            //     Managers.MinigamesManager.EndCurrentMinigame();
            //     StartCoroutine(EndBoss(false));
            // }
            // else
            // {
            //     StartCoroutine(DelayBeforeRound(5f));
            // }
        }

        private IEnumerator SetDamageMarker(int amt, bool isPlayer, bool isHeal) {
            //if its player, set playerDamageMarker to "-" + amt, then set gameObject true. if isHeal, set color to green. else, set color to red. same idea for enemy. wait 2 seconds, then set gameObject false
            if(isPlayer) {
                if(isHeal) {
                    playerDamageMarker.text = "+" + amt;
                    playerDamageMarker.color = Color.green;
                } else {
                    playerDamageMarker.text = "-" + amt;
                    playerDamageMarker.color = Color.red;
                }
                playerDamageMarker.gameObject.SetActive(true);
            } else {
                if(isHeal) {
                    playerDamageMarker.text = "+" + amt;
                    enemyDamageMarker.color = Color.green;
                } else {
                    enemyDamageMarker.text = "-" + amt;
                    enemyDamageMarker.color = Color.red;
                }
                enemyDamageMarker.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(2f);
            if(isPlayer) {
                playerDamageMarker.gameObject.SetActive(false);
            } else {
                enemyDamageMarker.gameObject.SetActive(false);
            }
        }

        private void EvaluateRound()
        {
            evaluatingRound = true;
            Debug.Log(playerDeck.ToString());   
            // Nothing selected
            // if (_selectedCardIndex < 0 || !_cardsInHand[_selectedCardIndex]) return;


            Card playerSelectedCard; 
            CardDescriptor playerCardDescriptor;


            //makes the other cards not pressable
            if(_selectedCardIndex < 0) {
                playerSelectedCard = null;
                playerCardDescriptor = null;
                
            } else {
                playerSelectedCard = _cardsInHand[_selectedCardIndex];
                playerCardDescriptor = playerSelectedCard.cardDescriptor;
                previousPlayerCard = playerSelectedCard;

                //make the 2 colliders that are not _selectedCardIndex not interactable. this array has 3 elements, so we can just do this:
                
            }

            for(int i = 0; i < _cardsInHand.Length; i++) {
                if(_cardsInHand[i] != null) {
                    _cardsInHand[i].GetComponent<Collider2D>().enabled = false;
                }
            }

            //do same with enemySelectedCard and enemyCardDescriptor
            Card enemySelectedCard;
            CardDescriptor enemyCardDescriptor;

            if(_enemySelectedCardIndex < 0) {
                enemySelectedCard = null;
                enemyCardDescriptor = null;
            } else {
                enemySelectedCard = _enemyCardsInHand[_enemySelectedCardIndex];
                enemyCardDescriptor = enemySelectedCard.cardDescriptor;
                previousEnemyCard = enemySelectedCard;
            }





            if(playerSelectedCard != null) {
                // Not enough mana
                if (PlayerMana < playerCardDescriptor.manaCost)
                {
                    playerSelectedCard.UnselectCard();
                    return;
                }

                switch (playerCardDescriptor)
                {
                    

                    case CardDescriptorAttack attackCard:
                    {
                        HandleAttack(attackCard, true);
                        //play from audio parent. get audio source, the first instance of it. because it has multiple audio source components
                        break;
                    }
                    case CardDescriptorDefense defenseCard:
                    {
                        Debug.Log(playerCardDescriptor.cardName);
                        HandleDefense(defenseCard, true);
                        break;
                    }
                    case CardDescriptorHeal healCard:
                    {
                        Debug.Log(playerCardDescriptor.cardName);
                        HandleHeal(healCard, true);
                        break;
                    }
                    case CardDescriptorStun stunCard:
                    {
                        Debug.Log(playerCardDescriptor.cardName);
                        HandleStun(stunCard, true);
                        break;
                    }
                    case CardDescriptorCopy copyCard:
                    {
                        Debug.Log(playerCardDescriptor.cardName);
                        HandleCopy(copyCard, true);
                        break;
                    }
                    default:
                    {
                        Debug.LogError("Could not cast card descriptor");
                        return;
                    }
                }
                StartCoroutine(playCardSound(playerCardDescriptor.cardName, 2f, true));
                
                // RemoveCardAt(_selectedCardIndex);
            }

            if(enemySelectedCard != null) {
                enemySelectedCard.GetComponent<Card>().SelectCardEnemy();
                StartCoroutine(FlipCard(enemySelectedCard.GetComponent<Animator>(), 1.5f));

                //now do the switch cases for enemySelectedCard
                switch (enemyCardDescriptor)
                {
                    case CardDescriptorAttack attackCard:
                    {
                        HandleAttack(attackCard, false);
                        break;
                    }
                    case CardDescriptorDefense defenseCard:
                    {
                        HandleDefense(defenseCard, false);
                        break;
                    }
                    case CardDescriptorHeal healCard:
                    {
                        HandleHeal(healCard, false);
                        break;
                    }
                    case CardDescriptorStun stunCard:
                    {
                        HandleStun(stunCard, false);
                        break;
                    }
                    case CardDescriptorCopy copyCard:
                    {
                        HandleCopy(copyCard, false);
                        break;
                    }
                    default:
                    {
                        Debug.LogError("Could not cast card descriptor");
                        return;
                    }
                }
                StartCoroutine(playCardSound(enemyCardDescriptor.cardName, 3f, false));
            }

            
            StartCoroutine(DoCardFight());
            //player and enemy fight logic done. now we clean up

            StartCoroutine(CleanUpCards(_selectedCardIndex, _enemySelectedCardIndex));
        }

        IEnumerator playCardSound(string cardName, float delay, bool player) {
            yield return new WaitForSeconds(delay);
            //switch case for cardname
            switch(cardName) {
                case "Claw Slash":
                    if(player) {
                        playerAudioParent.GetComponents<AudioSource>()[0].Play();
                    } else {
                        enemyAudioParent.GetComponents<AudioSource>()[0].Play();
                    }
                    break;
                case "Fire Blast":
                    if(player) {
                        playerAudioParent.GetComponents<AudioSource>()[1].Play();
                    } else {
                        enemyAudioParent.GetComponents<AudioSource>()[1].Play();
                    }
                    break;
                case "Thunder Strike":
                    if(player) {
                        playerAudioParent.GetComponents<AudioSource>()[2].Play();
                    } else {
                        enemyAudioParent.GetComponents<AudioSource>()[2].Play();
                    }
                    break;
                case "Healing Chant":
                    if(player) {
                        playerAudioParent.GetComponents<AudioSource>()[3].Play();
                    } else {
                        enemyAudioParent.GetComponents<AudioSource>()[3].Play();
                    }
                    break;
                case "Warlock Shield":
                    if(player) {
                        playerAudioParent.GetComponents<AudioSource>()[4].Play();
                    } else {
                        enemyAudioParent.GetComponents<AudioSource>()[4].Play();
                    }
                    break;
                case "Shadow Dodge":
                    if(player) {
                        playerAudioParent.GetComponents<AudioSource>()[5].Play();
                    } else {
                        enemyAudioParent.GetComponents<AudioSource>()[5].Play();
                    }
                    break;
                default: //play 6th
                    if(player) {
                        playerAudioParent.GetComponents<AudioSource>()[6].Play();
                    } else {
                        enemyAudioParent.GetComponents<AudioSource>()[6].Play();
                    }
                    break;
            }
        }

        private IEnumerator DoCardFight() {
            //cards done, now lets do fight logic
            //so if defense is higher, 
            int playerNetAttack;
            int enemyNetAttack;

            if(playerAtk > enemyDef) {
                playerNetAttack = playerAtk - enemyDef;
            } else {
                playerNetAttack = 0;
            }

            if(enemyAtk > playerDef) {
                enemyNetAttack = enemyAtk - playerDef;
            } else {
                enemyNetAttack = 0;
            }

            //check player card name. play the corresponding audio source. this is from playerAudioParent. the 0th index is claw slash, then its fire, 
            //then thunder, healing, shield, shadow dodge, and arcane counter

            if(resetMana) {
                PlayerMana -= PlayerMana;
                resetMana = false;
            }

            int prevPlayerHP = PlayerHealth;
            int prevEnemyHP = EnemyHealth;

            yield return new WaitForSeconds(4);
            PlayerHealth = Mathf.Max(0, PlayerHealth - enemyNetAttack + playerHeal);
            EnemyHealth = Mathf.Max(0, EnemyHealth - playerNetAttack + enemyHeal);

            int playerChange = Mathf.Abs(PlayerHealth - prevPlayerHP);
            int enemyChange = Mathf.Abs(EnemyHealth - prevEnemyHP);

           

            
            if(PlayerHealth < prevPlayerHP) {
                //set playerDamageMarker color to red, and set text to "-" + enemyNetAttack
                //set
                StartCoroutine(SetDamageMarker(playerChange, true, false));
            }  else if(PlayerHealth > prevPlayerHP) {
                //set playerDamageMarker color to green, and set text to "+" + playerHeal
                StartCoroutine(SetDamageMarker(playerChange, true, true));
            }

            //now for enemy health
            if(EnemyHealth < prevEnemyHP) {
                //set enemyDamageMarker color to red, and set text to "-" + playerNetAttack
                enemyDamageMarker.color = Color.red;
                enemyDamageMarker.text = "-" + playerNetAttack;
                StartCoroutine(SetDamageMarker(enemyChange, false, false));
            } else if(EnemyHealth > prevEnemyHP) {
                //set enemyDamageMarker color to green, and set text to "+" + enemyHeal
                enemyDamageMarker.color = Color.green;
                enemyDamageMarker.text = "+" + enemyHeal;
                StartCoroutine(SetDamageMarker(enemyChange, false, true));
            }

             //set them back to 0
            playerAtk = 0;
            playerDef = 0;
            enemyAtk = 0;
            enemyDef = 0;
            playerHeal = 0;
            enemyHeal = 0;

            if (EnemyHealth <= 0)
            {
                Debug.Log("WON!");
                Managers.MinigamesManager.DeclareCurrentMinigameWon();
                Managers.MinigamesManager.EndCurrentMinigame();
                StartCoroutine(EndBoss(true));
            }
            else if (PlayerHealth <= 0)
            {
                Debug.Log("LOST!");
                Managers.MinigamesManager.DeclareCurrentMinigameLost();
                Managers.MinigamesManager.EndCurrentMinigame();
                StartCoroutine(EndBoss(false));
            }
            else
            {
                StartCoroutine(DelayBeforeRound(1f));
            }



            
        }

        private IEnumerator CleanUpCards(int playerCardIndex, int enemyCardIndex) {
            yield return new WaitForSeconds(4f);
            RemoveCardAt(playerCardIndex);
            RemoveCardAt(enemyCardIndex, false);
            evaluatingRound = false;
        }

        private void HandleAttack(CardDescriptorAttack card, bool isPlayer, bool manaless = false)
        {
            if(isPlayer) {
                playerAtk = card.damage;
                if(manaless) { // i do this bnecause this is called from player arcane counter
                    playerAtk += 1;
                }
                // EnemyHealth = Mathf.Max(0, EnemyHealth - card.damage);
                if(!manaless) {
                    PlayerMana -= card.manaCost;
                
                    if(card.manaCost > 0 ) {
                        playerUsedMana = true;
                    }
                }
            } else {
                enemyAtk = card.damage;
                //PlayerHealth = Mathf.Max(0, PlayerHealth - card.damage);
                EnemyMana -= card.manaCost;
                if(card.manaCost > 0) {
                    enemyUsedMana = true;
                }
            }
        }

        private void HandleDefense(CardDescriptorDefense card, bool isPlayer, bool manaless = false)
        {
            if(isPlayer) {
                if(card.shouldEvade) { // this is shadow dodge
                    playerDef = 99;
                } else {
                    playerDef = card.blockAmount;
                    if(manaless) {
                        playerDef += 1;
                    }
                }
                if(!manaless) {
                    PlayerMana -= card.manaCost;
                    if(card.manaCost > 0) {
                        playerUsedMana = true;
                    }
                }
            } else {
                if(card.shouldEvade) { // this is shadow dodge
                    enemyDef = 99;
                } else {
                    enemyDef = card.blockAmount;
                }
                //enemyDef = card.blockAmount;
                EnemyMana -= card.manaCost;
                if(card.manaCost > 0) {
                    enemyUsedMana = true;
                }
            }
        }

        private void HandleHeal(CardDescriptorHeal card, bool isPlayer, bool manaless = false)
        {
            if(isPlayer) {
             //   PlayerHealth += card.healAmount;
                playerHeal = card.healAmount;
                if(manaless) {
                    playerHeal += 1;
                }
                //StartCoroutine(SetDamageMarker(card.healAmount, true, true));
                if(!manaless) {
                    PlayerMana -= card.manaCost;
                    if(card.manaCost > 0) {
                        playerUsedMana = true;
                    }
                }
            } else {
            //    EnemyHealth += card.healAmount;
                enemyHeal = card.healAmount;
                //StartCoroutine(SetDamageMarker(card.healAmount, false, true));
                EnemyMana -= card.manaCost;
                if(card.manaCost > 0) {
                    enemyUsedMana = true;
                }
            }
        }

        private void HandleStun(CardDescriptorStun card, bool isPlayer, bool manaless = false )
        {
            if(isPlayer) {
                if(manaless) {
                    PlayerMana -= card.manaCost;
                    playerUsedMana = true;
                }

            } else {
                EnemyMana -= card.manaCost;
                enemyUsedMana = true;
                playerStunned = true;
                resetMana = true;
            }
        }

        private void HandleCopy(CardDescriptorCopy card, bool isPlayer, bool manaless = false)
        {
            if(isPlayer) {
                PlayerMana -= card.manaCost;
                if(card.manaCost > 0) {
                    playerUsedMana = true;
                }
                //basically do the action done by the previous enemy card, with greater intensity. in other words, if its attack with 3 damage, do it with 4 damage. dont just call the  handle functions:
                Debug.Log(previousEnemyCard.name);
                if(previousEnemyCard != null) {
                    switch (previousEnemyCard.cardDescriptor)
                    {
                        case CardDescriptorAttack attackCard:
                        {
                            HandleAttack(attackCard, true, true);
                            break;
                        }
                        case CardDescriptorDefense defenseCard:
                        {
                            HandleDefense(defenseCard, true, true);
                            break;
                        }
                        case CardDescriptorHeal healCard:
                        {
                            HandleHeal(healCard, true, true);
                            break;
                        }
                        case CardDescriptorStun stunCard:
                        {
                            HandleStun(stunCard, true, true);
                            break;
                        }
                        default:
                        {
                            Debug.LogError("Could not cast card descriptor");
                            return;
                        }
                    }
                }
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
                   //we want to reveal enemy cards, then flip them back over
                    

                    _enemyCardsInHand[i] = cardToDraw; //add to card in hand
                    return;
                }
            }


            Debug.Log("No available card slots");
        }

        private IEnumerator RevealEnemyCards(int numCards) {
            //it's going to randomly reveal 2 of the enemy cards.
            //first, randomly select 2 cards to reveal in enemyCardsInHand array.
            int randomNum = Random.Range(0, 3);
            //another var. different from randomNum
            int randomNum2 = Random.Range(0, 3);
            if(randomNum == randomNum2) {
                randomNum2 = (randomNum2 + 1) % 3;
            }

            yield return new WaitForSeconds(2);

            //now, reveal the cards at those indices. access animator and play animation called "RevealCard"
            _enemyCardsInHand[randomNum].GetComponent<Animator>().Play("cardflip");
            _enemyCardsInHand[randomNum2].GetComponent<Animator>().Play("cardflip");
            yield return new WaitForSeconds(2);
            //now, flip them back over
            _enemyCardsInHand[randomNum].GetComponent<Animator>().Play("CardHide");
            _enemyCardsInHand[randomNum2].GetComponent<Animator>().Play("CardHide");

            skipButton.interactable = true;
            if(!playerStunned) {
                playButton.interactable = true;
            } 

        }

        private IEnumerator FlipCard(Animator animator, float delay) {
            yield return new WaitForSeconds(delay);
            animator.Play("cardflip");
        }

        private void OnCardSelected(int handIndex)
        {
            if(!playerStunned) {
                Debug.Log($"Card {handIndex} was played");

                if (_selectedCardIndex >= 0 && _cardsInHand[_selectedCardIndex])
                {
                    _cardsInHand[_selectedCardIndex].UnselectCard();
                }

                _selectedCardIndex = handIndex;
                _cardsInHand[handIndex].SelectCard();

                cardTitleText.text = _cardsInHand[handIndex].cardDescriptor.cardName;
                cardDescriptionText.text = _cardsInHand[handIndex].cardDescriptor.cardDescription;

                //check if player mana is greater than or equal to card mana cost. if not, unselect card
                if(PlayerMana >= _cardsInHand[handIndex].cardDescriptor.manaCost) {
                    cardSelected.Invoke();
                }
            }
        }

        private void OnCardUnselected(int handIndex)
        {
            // Already unselected
            if (handIndex != _selectedCardIndex) return;

            if(playerStunned) {
                cardTitleText.text = "Stunned! Cannot play a card :(";
                cardDescriptionText.text = "";
            } else {
                cardTitleText.text = "Select a card!";
                cardDescriptionText.text = "";
            }

            _selectedCardIndex = -1;
            _cardsInHand[handIndex].UnselectCard();
            cardUnselected.Invoke();
        }

        public void OnConfirmRoundPressed()
        {
            if (_roundTimer == null) return;

            StopCoroutine(_roundTimer);
            _roundTimer = null;

            timerIcon.gameObject.SetActive(false);
            roundTimerText.gameObject.SetActive(false);

            //check if the card at selected card has mana that is greater than player mana. if so, unselect
            if(_selectedCardIndex >= 0) {
                if(_cardsInHand[_selectedCardIndex].cardDescriptor.manaCost > PlayerMana) {
                    _cardsInHand[_selectedCardIndex].UnselectCard();
                    _selectedCardIndex = -1;
                }
            }

            EndRound();
        }

        public void OnSkipTurnPressed() {
            //same as OnConfirmRoundPressed, but set _selectedCardIndex to -1
            if (_roundTimer == null) return;

            StopCoroutine(_roundTimer);
            _roundTimer = null;

            timerIcon.gameObject.SetActive(false);
            roundTimerText.gameObject.SetActive(false);

            //if _selectedCardIndex is valid, unselect it
            if(_selectedCardIndex >= 0) {
                _cardsInHand[_selectedCardIndex].UnselectCard();
            }
            _selectedCardIndex = -1;
            EndRound();
        }

        private void RemoveCardAt(int index, bool isPlayer = true)
        {
            if(isPlayer) {
                Debug.Log(index);
                if (index < 0) return;

                var card = _cardsInHand[index];
                _cardsInHand[index] = null;

                if (card == null) return;

                card.gameObject.SetActive(false);
            } else {
                if (index < 0) return;

                var card = _enemyCardsInHand[index];
                _enemyCardsInHand[index] = null;

                if (card == null) return;

                card.gameObject.SetActive(false);
            }

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
            timerIcon.gameObject.SetActive(true);

            while (timeLeft > 0)
            {
                roundTimerText.text = timeLeft.ToString();
                timeLeft -= 1;
                yield return new WaitForSeconds(1);
            }

            roundTimerText.text = "0";
            timerIcon.gameObject.SetActive(false);
            roundTimerText.gameObject.SetActive(false);

            EndRound();
        }
    }
}