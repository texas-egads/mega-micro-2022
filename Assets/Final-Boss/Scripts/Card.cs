using System;
using System.Collections;
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

        public AudioSource flipSound;

        private void Start()
        {
            cardDescriptor = Instantiate(cardDescriptor);
        }

        //onenable that sets _hasBeenSelected to false
        private void OnEnable()
        {
            _hasBeenSelected = false;
        }

        public void DealCard(Vector3 fromPos, Vector3 toPosition, int handIndex)
        {
            // transform.position = position;
            _deckY = toPosition.y;
            _handIndex = handIndex;
            StartCoroutine(MoveCard(fromPos, toPosition, 1f));
        }

        //create a coroutine that takes vector3 fromPos, vector3 toPos, float duration. move transform from fromPos to toPos over duration:
        public IEnumerator MoveCard(Vector3 fromPos, Vector3 toPos, float duration)
        {
            transform.position = fromPos;
            yield return transform.DOMove(toPos, duration).WaitForCompletion();
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

        public void PlayFlipSound() {
            flipSound.Play();
        }

        public void SelectCard()
        {
            Debug.Log("Card Selected");
            _hasBeenSelected = true;
            transform.DOMoveY(_deckY + moveAmount, 0.5f);
        }

        public void SelectCardEnemy() //only difference, it moves down
        {
            Debug.Log("Card Selected");
            _hasBeenSelected = true;
            transform.DOMoveY(_deckY - moveAmount, 0.5f);
        }

        public void UnselectCard()
        {
            Debug.Log("Card Unselected");
            _hasBeenSelected = false;
            transform.DOMoveY(_deckY, 0.5f);
        }
    }
}