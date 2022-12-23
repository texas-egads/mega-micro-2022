using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    private int hits = 0;
    public Text hitsText;
    Animator anim;
    public AudioSource backgroundMusic;
    public AudioSource winMusic;

    private void Start()
    {
        //start zombie off
        //this.gameObject.GetComponent<Renderer>().enabled = false; //anim still plays at start, not solution
        anim = GetComponent<Animator>();
        anim.SetBool("isAlive", false); //anim controller parameter
        backgroundMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Space"))
        {
            hits += 1;
        }

        //end game once hit count is met
        if (hits == 15)
        {
            Debug.Log("you won omg");
            //zombiebara anim and music
            //this.gameObject.GetComponent<Renderer>().enabled = true;
            anim.SetBool("isAlive", true);
            winMusic.Play();

            // from example script
            Managers.MinigamesManager.DeclareCurrentMinigameWon();
            Managers.MinigamesManager.EndCurrentMinigame(1.2f);
        }

        //keep track of hits; del for final ver
        hitsText.text = hits.ToString("spacebar hits: 0");
    }
}
