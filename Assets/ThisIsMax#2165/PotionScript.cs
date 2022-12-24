using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThisIsMax{
    public class PotionScript : MonoBehaviour
    {
        public enum Ingredients{
            None,
            BlueW,
            RedA,
            GreenS,
            YellowD
        };

        // Sounds
        public AudioClip buttonPressSound;
        public AudioClip winSound;
        public AudioClip loseSound;

        public Ingredients[] correctIngredientSequence;
        public Ingredients[] playerIngredientSequence;
        public int lengthOfCorrectIngredientSequence;
        public int currentPlayerIngredientSequenceIndex = 0;

        // Potion Displays
        public SpriteRenderer[] potionDisplays;
        public Sprite[] potionSprites;

        // For the falling potion
        public GameObject fallingPotionPrefab;
        public GameObject bluePotionIcon;
        public GameObject redPotionIcon;
        public GameObject greenPotionIcon;
        public GameObject yellowPotionIcon;

        // For capybara
        public GameObject capybara;
        public float capybaraXOffset;
        public float capybaraYOffset;

        // Start is called before the first frame update
        void Start()
        {
            // Initialize the arrays
            correctIngredientSequence = new Ingredients[lengthOfCorrectIngredientSequence];
            playerIngredientSequence = new Ingredients[lengthOfCorrectIngredientSequence];

            // Randomly Decide the correct ingredient sequence
            for (int i = 0; i < lengthOfCorrectIngredientSequence; i++)
            {
                correctIngredientSequence[i] = (Ingredients)Random.Range(1, 5);
            }

            // Set the potion displays
            for (int i = 0; i < lengthOfCorrectIngredientSequence; i++)
            {
                switch (correctIngredientSequence[i])
                {
                    case Ingredients.BlueW:
                        potionDisplays[i].sprite = potionSprites[0];
                        break;
                    case Ingredients.RedA:
                        potionDisplays[i].sprite = potionSprites[1];
                        break;
                    case Ingredients.GreenS:
                        potionDisplays[i].sprite = potionSprites[2];
                        break;
                    case Ingredients.YellowD:
                        potionDisplays[i].sprite = potionSprites[3];
                        break;
                }
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            // Get Player Input
            // Blue = W, Red = A, Green = S, Yellow = D
            if(currentPlayerIngredientSequenceIndex < lengthOfCorrectIngredientSequence){
                if (Input.GetKeyDown(KeyCode.W))
                {
                    AudioSource pop = Managers.AudioManager.CreateAudioSource();
                    pop.PlayOneShot(buttonPressSound);

                    playerIngredientSequence[currentPlayerIngredientSequenceIndex] = Ingredients.BlueW;
                    currentPlayerIngredientSequenceIndex++;
                    // Create a falling potion
                    GameObject created = Instantiate(fallingPotionPrefab, bluePotionIcon.transform.position, Quaternion.identity);
                    // Create a capybara
                    Vector3 pos = bluePotionIcon.transform.position;
                    pos.y += capybaraYOffset;
                    pos.x -= capybaraXOffset;
                    pos.z -= 1;
                    GameObject charlie = Instantiate(capybara, pos, Quaternion.identity);
                    charlie.transform.rotation = Quaternion.Euler(0, 0, 320);
                    // Set Correct Sprite
                    created.GetComponent<SpriteRenderer>().sprite = potionSprites[0];
                    // Apply force 
                    created.GetComponent<Rigidbody2D>().AddForce(new Vector2(100,5));
                    created.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-100, 100));
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    AudioSource pop = Managers.AudioManager.CreateAudioSource();
                    pop.PlayOneShot(buttonPressSound);

                    playerIngredientSequence[currentPlayerIngredientSequenceIndex] = Ingredients.RedA;
                    currentPlayerIngredientSequenceIndex++;
                    // Create a falling potion
                    GameObject created = Instantiate(fallingPotionPrefab, redPotionIcon.transform.position, Quaternion.identity);
                    // Create a capybara
                    Vector3 pos = redPotionIcon.transform.position;
                    pos.y += capybaraYOffset;
                    pos.x -= capybaraXOffset;
                    pos.z -= 1;
                    GameObject charlie = Instantiate(capybara, pos, Quaternion.identity);
                    charlie.transform.rotation = Quaternion.Euler(0, 0, 320);

                    // Set Correct Sprite
                    created.GetComponent<SpriteRenderer>().sprite = potionSprites[1];
                    // Apply force
                    created.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-100, 100));
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    AudioSource pop = Managers.AudioManager.CreateAudioSource();
                    pop.PlayOneShot(buttonPressSound);

                    playerIngredientSequence[currentPlayerIngredientSequenceIndex] = Ingredients.GreenS;
                    currentPlayerIngredientSequenceIndex++;
                    // Create a falling potion
                    GameObject created = Instantiate(fallingPotionPrefab, greenPotionIcon.transform.position, Quaternion.identity);

                    // Create a capybara
                    Vector3 pos = greenPotionIcon.transform.position;
                    pos.y += capybaraYOffset;
                    pos.x -= capybaraXOffset;
                    pos.z -= 1;
                    GameObject charlie = Instantiate(capybara, pos, Quaternion.identity);
                    charlie.transform.rotation = Quaternion.Euler(0, 0, 320);

                    // Set Correct Sprite
                    created.GetComponent<SpriteRenderer>().sprite = potionSprites[2];
                    // Apply force
                    created.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-100, 100));
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    AudioSource pop = Managers.AudioManager.CreateAudioSource();
                    pop.PlayOneShot(buttonPressSound);

                    playerIngredientSequence[currentPlayerIngredientSequenceIndex] = Ingredients.YellowD;
                    currentPlayerIngredientSequenceIndex++;
                    // Create a falling potion
                    GameObject created = Instantiate(fallingPotionPrefab, yellowPotionIcon.transform.position, Quaternion.identity);

                    // Create a capybara
                    Vector3 pos = yellowPotionIcon.transform.position;
                    pos.y += capybaraYOffset;
                    pos.x -= capybaraXOffset;
                    pos.z -= 1;
                    GameObject charlie = Instantiate(capybara, pos, Quaternion.identity);
                    charlie.transform.rotation = Quaternion.Euler(0, 0, 320);

                    // Set Correct Sprite
                    created.GetComponent<SpriteRenderer>().sprite = potionSprites[3];
                    // Apply force
                    created.GetComponent<Rigidbody2D>().AddForce(new Vector2(-100,5));
                    created.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-100, 100));
                }
            }else
            {
                // Check if the player's ingredient sequence is correct
                bool isCorrect = true;
                for (int i = 0; i < lengthOfCorrectIngredientSequence; i++)
                {
                    if (correctIngredientSequence[i] != playerIngredientSequence[i])
                    {
                        isCorrect = false;
                        break;
                    }
                }
                if(isCorrect){
                    AudioSource win = Managers.AudioManager.CreateAudioSource();
                    win.PlayOneShot(winSound);
                    Managers.MinigamesManager.DeclareCurrentMinigameWon();
                    Managers.MinigamesManager.EndCurrentMinigame(1f);
                    this.enabled = false;
                }else{
                    AudioSource lose = Managers.AudioManager.CreateAudioSource();
                    lose.PlayOneShot(loseSound);
                    Managers.MinigamesManager.DeclareCurrentMinigameLost();
                    Managers.MinigamesManager.EndCurrentMinigame(1f);
                    this.enabled = false;
                }
            }
        }
    }
}
