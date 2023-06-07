using System;
using UnityEngine;

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
    }
}