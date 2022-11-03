using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigamesManager : MonoBehaviour, IMinigamesManager
{
    public const int STARTING_LIVES = 3;

    [SerializeField] private List<MinigameDefinition> allMinigames;
    [SerializeField] private int numRoundsInEasyMode;
    [SerializeField] private int numRoundsInNormalMode;

    private int currentLives;
    private int currentRound;
    private List<MinigameDefinition> minigames;

    private bool isMinigamePlaying;
    private bool isCurrentMinigameWon;

    // stores a preloaded scene for an upcoming round.
    private Scene nextMinigameScene;

    public void Initialize() {
        currentLives = STARTING_LIVES;
        currentRound = 0;
        minigames = new List<MinigameDefinition>();
        isMinigamePlaying = false;
        isCurrentMinigameWon = false;
    }

    public void StartMinigames() {
        PopulateMinigameList(numRoundsInNormalMode); // TODO figure out what this number should be
    }

    public void PopulateMinigameList(int numberOfRounds) {
        minigames.Clear();

        // TODO
    }


    public MinigameDefinition GetCurrentMinigameDefinition() {
        if (currentRound < 0 || currentRound >= minigames.Count)
            return null;
        
        return minigames[currentRound];
    }
    
    public void DeclareCurrentMinigameWon() {
        if (!isMinigamePlaying)
            return;
        isCurrentMinigameWon = true;
    }

    public void DeclareCurrentMinigameLost() {
        if (!isMinigamePlaying)
            return;
        isCurrentMinigameWon = false;
    }

    public void EndCurrentMinigame() {
        if (!isMinigamePlaying)
            return;
        
        isMinigamePlaying = false;

        // TODO decide what's going to happen. decrease a life? is this a game over? do we have a next minigame? etc

        // TODO if we have to load another minigame, do that now
    }


    // Called when all of the between-minigame cinematics are complete and the
    // next minigame is ready to be put on screen.
    public void LoadNextMinigame() {
        // TODO wait for the next minigame to load

        isCurrentMinigameWon = false;
        isMinigamePlaying = true;

        // TODO activate it and switch over control
    }
}
