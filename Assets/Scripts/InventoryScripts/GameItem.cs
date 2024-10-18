namespace InventoryScripts
{
    public class GameItem
    {
        private string _name;
        private string _description;
        private int _stackSize;
        private ItemType _itemType;

        public GameItem(string name, string description, int stackSize, ItemType itemType)
        {
            this._name = name;
            this._description = description; 
            this._stackSize = stackSize; 
            this._itemType = itemType;
        }

        public int GetStackSize()
        {
            return _stackSize;
        }

        public string GetName() => this._name;
    }

    public enum ItemType
    {
        EQUIPMENT,
        CONSUMABLE,
    }
}