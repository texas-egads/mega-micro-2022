namespace Final_Boss.ScriptableObjects
{
    public class CardDescriptorDefense : CardDescriptor
    {
        public int blockAmount;
        public bool shouldEvade;
        public int turnsApplied = 1;
        
        public override void Upgrade(int upgradeAmount)
        {
            if (shouldEvade)
            {
                turnsApplied += upgradeAmount;
            }
            else
            {
                blockAmount += upgradeAmount;
            }
        }
    }
}