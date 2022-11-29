using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMinigamesManager
{
    /// <summary>
    /// Call this from your minigame the moment your minigame enters a winning state. This could
    /// be the instant the player clears the objective, or at the very beginning of a survival-based
    /// minigame. This does not cause the minigame to end until you call EndCurrentMinigame or
    /// your minigame time runs out.
    /// 
    /// If you do not call either DeclareCurrentMinigameWon or DeclareCurrentMinigameLost during
    /// your minigame time, we will assume that the player lost your minigame.
    /// </summary>
    void DeclareCurrentMinigameWon();

    /// <summary>
    /// Call this from your minigame the moment your minigame enters a losing state. This could
    /// be the instant the player makes an incorrect decision or fails at a survival-based minigame.
    /// This does not cause the minigame to end until you call EndCurrentMinigame or
    /// your minigame time runs out.
    /// 
    /// If you do not call either DeclareCurrentMinigameWon or DeclareCurrentMinigameLost during
    /// your minigame time, we will assume that the player lost your minigame.
    /// </summary>
    void DeclareCurrentMinigameLost();

    /// <summary>
    /// Call this when you want your minigame to end early. You can optionally specify a delay
    /// for how long the game should wait before ending your minigame. A good time to do this would
    /// be after a win or lose animation finishes, when the player already knows the final outcome
    /// of the minigame. If you do not call EndCurrentMinigame during your minigame time, the minigame
    /// will end automatically when your minigame time runs out.
    /// </summary>
    void EndCurrentMinigame(float delay = 0);
}
