using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamersWithGangrene
{
    public class KeyScript : MonoBehaviour
    {
        GameObject W;
        GameObject A;
        GameObject S;
        GameObject D;
        GameObject Space;

        GameObject WPressed;
        GameObject APressed;
        GameObject SPressed;
        GameObject DPressed;
        GameObject SpacePressed;
        // Start is called before the first frame update
        void Start()
        {
            W = GameObject.Find("W");
            A = GameObject.Find("A");
            S = GameObject.Find("S");
            D = GameObject.Find("D");
            Space = GameObject.Find("Space");

            WPressed = GameObject.Find("WPressed");
            APressed = GameObject.Find("APressed");
            SPressed = GameObject.Find("SPressed");
            DPressed = GameObject.Find("DPressed");
            SpacePressed = GameObject.Find("SpacePressed");

            HideKey(WPressed);
            HideKey(APressed);
            HideKey(SPressed);
            HideKey(DPressed);
            HideKey(SpacePressed);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                HideKey(W);
                DisplayKey(WPressed);
            } else if (Input.GetKeyDown(KeyCode.A))
            {
                HideKey(A);
                DisplayKey(APressed);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                HideKey(S);
                DisplayKey(SPressed);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                HideKey(D);
                DisplayKey(DPressed);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                HideKey(Space);
                DisplayKey(SpacePressed);
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                DisplayKey(W);
                HideKey(WPressed);
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                DisplayKey(A);
                HideKey(APressed);
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                DisplayKey(S);
                HideKey(SPressed);
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                DisplayKey(D);
                HideKey(DPressed);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                DisplayKey(Space);
                HideKey(SpacePressed);
            }
        }

        void HideKey(GameObject key)
        {
            key.SetActive(false);
        }

        void DisplayKey(GameObject key)
        {
            key.SetActive(true);
        }
    }

}