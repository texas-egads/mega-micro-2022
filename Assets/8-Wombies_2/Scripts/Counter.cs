using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    public int hits = 0;
    public Text hitsText;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Space"))
        {
            hits += 1;
            //do grave animation
        }

        //end game once hit count is met
        if (hits == 15)
        {
            Debug.Log("you won omg");
            //zombiebara anim

            // from example script
            Managers.MinigamesManager.DeclareCurrentMinigameWon();
            Managers.MinigamesManager.EndCurrentMinigame(1.2f);
        }


        // keep track of hits for testing; del for final ver
        hitsText.text = hits.ToString("spacebar hits: 0");
    }
}
