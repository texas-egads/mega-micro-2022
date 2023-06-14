using UnityEngine;

namespace Final_Boss.ScriptableObjects
{
    [CreateAssetMenu]
    public class CardDescriptorAttack : CardDescriptor
    {
        public int damage;
        
        public override void Upgrade(int upgradeAmount)
        {
            damage += upgradeAmount;
        }
    }
}