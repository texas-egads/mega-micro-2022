using UnityEngine;

namespace Final_Boss.ScriptableObjects
{
    [CreateAssetMenu]
    public class CardDescriptorHeal : CardDescriptor
    {
        public int healAmount;
        
        public override void Upgrade(int upgradeAmount)
        {
            healAmount += upgradeAmount;
        }
    }
}