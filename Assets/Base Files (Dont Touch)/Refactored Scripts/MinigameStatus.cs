using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MinigameStatus
{
    public int currentHealth;
    public int healthDelta;
    public bool previousMinigameWon => healthDelta >= 0;
    public MinigameDefinition previousMinigame;
    public MinigameDefinition nextMinigame;
    public int nextRoundNumber;
    public bool hasWonGame;
    public bool hasLostGame => currentHealth == 0;
}
