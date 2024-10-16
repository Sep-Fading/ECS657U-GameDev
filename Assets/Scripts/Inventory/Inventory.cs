using System.Collections.Generic;

namespace Inventory
{
    public class Inventory
    { 
        public static Inventory Instance { get; private set; }

        private Inventory()
        {
            return;
        }

        public static Inventory Initialize()
        {
            if (Instance == null)
            {
                Instance = new Inventory();
            }
            return Instance;
        }
        
        /* Class Behaviours and Properties */
        Stack<GameItem>[] _inventoryArray = new Stack<GameItem>[4];

        public void Push(GameItem item)
        {
            for (int i = 0; i < _inventoryArray.Length; i++)
            {
                if (_inventoryArray[i] == null)
                {
                    _inventoryArray[i] = new Stack<GameItem>();
                    _inventoryArray[i].Push(item);
                    return;
                }
            }
        }

        public GameItem Pop(GameItem item)
        {
            for (int i = 0; i < _inventoryArray.Length; i++)
            {
                if (_inventoryArray[i] != null && _inventoryArray[i].Contains(item))
                {
                    return _inventoryArray[i].Pop();
                }
            }

            return null;
        }
    }
}


