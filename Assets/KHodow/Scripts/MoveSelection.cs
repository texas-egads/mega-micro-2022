using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KHodow
{
    public class MoveSelection : MonoBehaviour
    {
        [HideInInspector] private Vector2 selectedPos;
        [SerializeField] private TokenResponse selectedToken;

        private void Start() 
        {
            selectedPos = selectedToken.transform.position;
        }
        private void Update()
        {
            ManageControls();
        }
        private void ManageControls()
        {
            if (Input.GetKeyDown(KeyCode.W) && !selectedToken.isFlipping)
            {
                Vector2 checkPos = selectedPos + new Vector2(0f, 3.5f);
                Collider2D newTokenCollider = Physics2D.OverlapCircle(checkPos, 0.01f);
                if (newTokenCollider != null)
                {
                    TokenResponse newToken = newTokenCollider.GetComponent<TokenResponse>();
                    selectedToken.ManageSelect();
                    selectedToken = newToken;
                    selectedPos = selectedToken.transform.position;
                    selectedToken.ManageSelect();
                }
            }
            else if (Input.GetKeyDown(KeyCode.A) && !selectedToken.isFlipping)
            {
                Vector2 checkPos = selectedPos + new Vector2(-3.5f, 0f);
                Collider2D newTokenCollider = Physics2D.OverlapCircle(checkPos, 0.01f);
                if (newTokenCollider != null)
                {
                    TokenResponse newToken = newTokenCollider.GetComponent<TokenResponse>();
                    selectedToken.ManageSelect();
                    selectedToken = newToken;
                    selectedPos = selectedToken.transform.position;
                    selectedToken.ManageSelect();
                }
            }
            else if (Input.GetKeyDown(KeyCode.D) && !selectedToken.isFlipping)
            {
                Vector2 checkPos = selectedPos + new Vector2(3.5f, 0f);
                Collider2D newTokenCollider = Physics2D.OverlapCircle(checkPos, 0.01f);
                if (newTokenCollider != null)
                {
                    TokenResponse newToken = newTokenCollider.GetComponent<TokenResponse>();
                    selectedToken.ManageSelect();
                    selectedToken = newToken;
                    selectedPos = selectedToken.transform.position;
                    selectedToken.ManageSelect();
                }
            }
            else if (Input.GetKeyDown(KeyCode.S) && !selectedToken.isFlipping)
            {
                Vector2 checkPos = selectedPos + new Vector2(0f, -3.5f);
                Collider2D newTokenCollider = Physics2D.OverlapCircle(checkPos, 0.01f);
                if (newTokenCollider != null)
                {
                    TokenResponse newToken = newTokenCollider.GetComponent<TokenResponse>();
                    selectedToken.ManageSelect();
                    selectedToken = newToken;
                    selectedPos = selectedToken.transform.position;
                    selectedToken.ManageSelect();
                }
            }
        }
    }
}

