namespace InventoryScripts
{
    public class InventoryItem
    {
        public GameItem gameItem;
        public Equipment equipment;
        public bool isEquipped;

        public InventoryItem(GameItem gameItem, Equipment equipment)
        {
            this.isEquipped = false;
            this.gameItem = gameItem;
            this.equipment = equipment;
        }
    }
}