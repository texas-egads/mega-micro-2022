using UnityEngine;

namespace Final_Boss.ScriptableObjects
{
    [CreateAssetMenu]
    public class CardDescriptorStun : CardDescriptor
    {
        public int turnsApplied;
        
        public override void Upgrade(int upgradeAmount)
        {
            turnsApplied += 1;
        }
    }
}