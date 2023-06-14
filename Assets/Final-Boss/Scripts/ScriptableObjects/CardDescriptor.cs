using UnityEngine;

namespace Final_Boss.ScriptableObjects
{
    public abstract class CardDescriptor : ScriptableObject
    {
        public string cardName;
        public int manaCost;

        public abstract void Upgrade(int upgradeAmount);
    }
}