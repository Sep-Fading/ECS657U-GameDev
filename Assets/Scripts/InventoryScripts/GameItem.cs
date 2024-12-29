namespace InventoryScripts
{
    public class GameItem
    {
        private string _name;
        private string _description;
        private int _stackSize;
        private ItemType _itemType;
        private int sellPrice;
        private int buyPrice;

        public GameItem(string name, string description, int stackSize, ItemType itemType,
            int sellPrice=0, int buyPrice=0)
        {
            this._name = name;
            this._description = description; 
            this._stackSize = stackSize; 
            this._itemType = itemType;
            this.sellPrice = sellPrice;
            this.buyPrice = buyPrice;
        }

        public int GetStackSize()
        {
            return _stackSize;
        }

        public string GetName() => this._name;
        public int GetSellPrice() => this.sellPrice;
        public int GetBuyPrice() => this.buyPrice;

        public string GetDescription()
        {
            return _description;
        }
    }

    public enum ItemType
    {
        EQUIPMENT,
        CONSUMABLE,
    }
}