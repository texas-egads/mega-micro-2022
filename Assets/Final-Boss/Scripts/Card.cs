using System;
using DG.Tweening;
using Final_Boss.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Final_Boss
{
    public class Card : MonoBehaviour
    {
        public CardDescriptor cardDescriptor;
        public float moveAmount = 25;

        // Events
        public static event Action<int> CardSelected;
        public static event Action<int> CardUnselected;
        
        private bool _hasBeenSelected;
        private int _handIndex;
        private float _deckY;

        private void Start()
        {
            cardDescriptor = Instantiate(cardDescriptor);
        }

        //onenable that sets _hasBeenSelected to false
        private void OnEnable()
        {
            _hasBeenSelected = false;
        }

        public void DealCard(Vector3 position, int handIndex)
        {
            transform.position = position;
            _deckY = position.y;
            _handIndex = handIndex;
        }

        private void OnMouseDown()
        {
            if (_hasBeenSelected)
            {
                Debug.Log("Unselecting card");
                
                CardUnselected?.Invoke(_handIndex);
            }
            else
            {
                Debug.Log("Selecting card");
                
                CardSelected?.Invoke(_handIndex);
            }
        }

        public void SelectCard()
        {
            Debug.Log("Card Selected");
            _hasBeenSelected = true;
            transform.DOMoveY(_deckY + moveAmount, 0.5f);
        }

        public void UnselectCard()
        {
            Debug.Log("Card Unselected");
            _hasBeenSelected = false;
            transform.DOMoveY(_deckY, 0.5f);
        }
    }
}