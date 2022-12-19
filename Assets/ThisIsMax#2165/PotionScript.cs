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

        public Ingredients[] correctIngredientSequence;
        public Ingredients[] playerIngredientSequence;
        public int lengthOfCorrectIngredientSequence;
        public int currentPlayerIngredientSequenceIndex = 0;

        // Potion Displays
        public SpriteRenderer[] potionDisplays;
        public Sprite[] potionSprites;


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
                    playerIngredientSequence[currentPlayerIngredientSequenceIndex] = Ingredients.BlueW;
                    currentPlayerIngredientSequenceIndex++;
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    playerIngredientSequence[currentPlayerIngredientSequenceIndex] = Ingredients.RedA;
                    currentPlayerIngredientSequenceIndex++;
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    playerIngredientSequence[currentPlayerIngredientSequenceIndex] = Ingredients.GreenS;
                    currentPlayerIngredientSequenceIndex++;
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    playerIngredientSequence[currentPlayerIngredientSequenceIndex] = Ingredients.YellowD;
                    currentPlayerIngredientSequenceIndex++;
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
                    Debug.Log("Correct!");
                }else{
                    Debug.Log("Incorrect!");
                }
            }
        }
    }
}
