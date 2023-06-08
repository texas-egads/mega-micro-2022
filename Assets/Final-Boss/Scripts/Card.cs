using System;
using UnityEngine;
using UnityEngine.Events;

namespace Final_Boss
{
    public class Card : MonoBehaviour
    {
        [Serializable]
        public enum CardType
        {
            Attack,
            Block,
            Evade,
            Heal,
            Special
        }

        [Header("Details")]
        public string cardName;
        public CardType cardType;
        public int manaCost;
        public int cardValue;
        public int turnsApplied = 1;

        // Events
        public static event Action<int> CardPlayed; 
        
        private bool _hasBeenPlayed;
        private int _handIndex;

        public void DealCard(int handIndex)
        {
            _handIndex = handIndex;
        }

        private void OnMouseDown()
        {
            if (_hasBeenPlayed) return;
            
            Debug.Log("Playing card");
            transform.position += Vector3.up * 5;
            _hasBeenPlayed = true;
            CardPlayed?.Invoke(_handIndex);
        }
    }
}