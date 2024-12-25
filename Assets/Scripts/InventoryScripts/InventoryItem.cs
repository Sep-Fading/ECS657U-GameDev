namespace InventoryScripts
{
    // A wrapper class that is used to cross-reference
    // GameItems within Equipments and vice versa.
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