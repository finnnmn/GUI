using UnityEngine;

public class Item
{
    #region variables
    int id;
    string name;
    string description;
    int itemValue;
    int amount;
    int stackSize;
    Sprite icon;
    GameObject mesh;
    ItemType type;
    EquipmentType equipType;
    bool equipped;
    int power;
    #endregion

    #region properties
    public int ID
    {
        get { return id; }
        set { id = value; }
    }
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public string Description
    {
        get { return description; }
        set { description = value; }
    }
    public int Value
    {
        get { return itemValue; }
        set { itemValue = value; }
    }
    public int Amount
    {
        get { return amount; }
        set { amount = value; }
    }

    public int StackSize
    {
        get { return stackSize; }
        set { stackSize = value; }
    }
    public Sprite Icon
    {
        get { return icon; }
        set { icon = value; }
    }
    public GameObject Mesh
    {
        get { return mesh; }
        set { mesh = value; }
    }
    public ItemType Type
    {
        get { return type; }
        set { type = value; }
    }

    public EquipmentType EquipType
    {
        get { return equipType; }
        set { equipType = value; }
    }
    public bool Equipped
    {
        get { return equipped; }
        set { equipped = value; }
    }
    public int Power
    {
        get { return power; }
        set { power = value; }
    }

    #endregion
}

public enum ItemType
{
    Food,
    Weapon,
    Armour,
    Potion,
    Craft,
    Quest,
    Money
}