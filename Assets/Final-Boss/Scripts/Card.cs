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
        public float moveAmount = 5;

        // Events
        public static event Action<int> CardPlayed; 
        
        private bool _hasBeenPlayed;
        private int _handIndex;

        private void Start()
        {
            cardDescriptor = Instantiate(cardDescriptor);
        }

        public void DealCard(int handIndex)
        {
            _handIndex = handIndex;
        }

        private void OnMouseDown()
        {
            if (_hasBeenPlayed) return;
            
            Debug.Log("Playing card");
            
            CardPlayed?.Invoke(_handIndex);
        }

        public void CardSelected()
        {
            _hasBeenPlayed = true;
            transform.DOMoveY(moveAmount, 0.5f);
        }

        public void CardUnselected()
        {
            _hasBeenPlayed = false;
            transform.DOMoveY(-moveAmount, 0.5f);
        }
    }
}