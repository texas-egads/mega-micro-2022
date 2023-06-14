namespace Final_Boss.ScriptableObjects
{
    public class CardDescriptorHeal : CardDescriptor
    {
        public int healAmount;
        
        public override void Upgrade(int upgradeAmount)
        {
            healAmount += upgradeAmount;
        }
    }
}