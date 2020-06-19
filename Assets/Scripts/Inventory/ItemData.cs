using UnityEngine;

public static class ItemData
{
    public static Item CreateItem(int itemID, int number)
    {
        #region create values
        string name;
        string description;
        int value;
        int amount;
        int stackSize;
        string icon;
        string mesh;
        ItemType type;
        EquipmentType equipType = EquipmentType.body;
        int power = 0;
        #endregion

        #region item data
        switch(itemID)
        {
            #region food 0-99
            case 0:
                name = "Apple";
                description = "A tasty fruit. Heals 10 HP";
                value = 5;
                icon = "Food/Apple";
                mesh = "Food/Apple";
                type = ItemType.Food;
                power = 10;
                break;
            case 1:
                name = "Meat";
                description = "The meat of some animal. Heals 20 HP";
                value = 10;
                icon = "Food/Meat";
                mesh = "Food/Meat";
                type = ItemType.Food;
                power = 20;
                break;
            #endregion

            #region weapons 100 - 199
            case 100:
                name = "Axe";
                description = "The only weapon that isn't a primitive shape";
                value = 100;
                icon = "Weapon/Axe";
                mesh = "Weapon/Axe";
                type = ItemType.Weapon;
                equipType = EquipmentType.hand1;
                power = 50;
                break;
            case 101:
                name = "Bow";
                description = "Doesn't come with arrows";
                value = 50;
                icon = "Weapon/Bow";
                mesh = "Weapon/Bow";
                type = ItemType.Weapon;
                equipType = EquipmentType.hand1;
                power = 15;
                break;
            case 102:
                name = "Sword";
                description = "A sword";
                value = 60;
                icon = "Weapon/Sword";
                mesh = "Weapon/Sword";
                type = ItemType.Weapon;
                equipType = EquipmentType.hand1;
                power = 30;
                break;
            #endregion

            #region armour 200-299
            case 200:
                name = "Armour";
                description = "Protects you from stuff. DEF 15";
                value = 50;
                icon = "Armour/Armour";
                mesh = "Armour/Armour";
                type = ItemType.Armour;
                equipType = EquipmentType.body;
                power = 15;
                break;

            case 201:
                name = "Helmet";
                description = "Armour for the head. DEF 10";
                value = 30;
                icon = "Armour/Helmet";
                mesh = "Armour/Helmet";
                type = ItemType.Armour;
                equipType = EquipmentType.head;
                power = 10;
                break;

            case 202:
                name = "Shield";
                description = "A worn shield. DEF 10";
                value = 25;
                icon = "Armour/Shield";
                mesh = "Armour/Shield";
                type = ItemType.Armour;
                equipType = EquipmentType.hand2;
                power = 10;
                break;

            #endregion

            #region potions 300 - 399
            case 300:
                name = "HealthPotion";
                description = "A red potion";
                value = 15;
                icon = "Potion/HealthPotion";
                mesh = "Potion/HealthPotion";
                type = ItemType.Potion;
                power = 50;
                break;
            case 301:
                name = "ManaPotion";
                description = "Actually just water";
                value = 15;
                icon = "Potion/ManaPotion";
                mesh = "Potion/ManaPotion";
                type = ItemType.Potion;
                power = 50;
                break;
            #endregion

            #region craft 400 - 499
            case 400:
                name = "Ingot";
                description = "An unknown metal";
                value = 10;
                icon = "Craft/Ingot";
                mesh = "Craft/Ingot";
                type = ItemType.Craft;
                break;
            #endregion

            #region quest 500-599
            case 500:
                name = "Gem";
                description = "A rare and amazing gem";
                value = 100;
                icon = "Quest/Gem";
                mesh = "Quest/Gem";
                type = ItemType.Quest;
                break;
            #endregion

            #region misc 600 - 699
            case 600:
                name = "Coin";
                description = "For buying things";
                value = 1;
                icon = "Coins";
                mesh = "Coins";
                type = ItemType.Money;
                break;

            #endregion

            default:
                name = "Mysterious Apple";
                description = "Was this supposed to be something else? Very mysterious";
                value = 5;
                icon = "Food/Apple";
                mesh = "Food/Apple";
                type = ItemType.Food;
                power = 10;
                break;
        }
        #endregion

        #region stack size and amount
        switch(type)
        {
            case ItemType.Food:
                stackSize = 10;
                break;
            case ItemType.Potion:
                stackSize = 10;
                break;
            case ItemType.Craft:
                stackSize = 30;
                break;
            case ItemType.Money:
                stackSize = 1000;
                break;
            default:
                stackSize = 1;
                break;
        }

        if (number > stackSize)
        {
            amount = stackSize;
        }
        else
        {
            amount = number;
        }
        #endregion

        #region create and return item
        Item item = new Item
        {
            ID = itemID,
            Name = name,
            Description = description,
            Value = value,
            Amount = amount,
            StackSize = stackSize,
            Type = type,
            EquipType = equipType,
            Equipped = false,
            Icon = Resources.Load<Sprite>("Icons/" + icon) as Sprite,
            Mesh = Resources.Load("Mesh/" + mesh) as GameObject,
            Power = power
        };
        return item;
        #endregion
    }
}
