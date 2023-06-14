namespace Final_Boss.ScriptableObjects
{
    public class CardDescriptorCopy : CardDescriptor
    {
        public int upgradeScale = 1;

        public CardDescriptor UpgradeCard(CardDescriptor card)
        {
            var newCard = Instantiate(card);
            newCard.Upgrade(upgradeScale);
            return newCard;
        }

        public override void Upgrade(int upgradeAmount)
        {
            // Non-upgradable
        }
    }
}