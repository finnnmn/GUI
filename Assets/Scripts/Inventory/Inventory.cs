using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Equipment))]
public class Inventory : MonoBehaviour
{
    #region variables
    PlayerControl player;
    Equipment equipment;

    [Header("Inventory Data")]
    public static int gold = 50;
    public static List<Item> inventory = new List<Item>();
    public InventoryMode inventoryMode = InventoryMode.equipment;

    public Storage currentStorage;

    List<Button> itemButtons = new List<Button>();
    List<Button> itemButtons2 = new List<Button>();

    Item selectedItem = null;
    Item selectedItem2 = null;
   

    public static string sortType = null;

    [Header("Transforms")]
    public Transform dropLocation;

    [Header("UI References")]
    public GameObject inventoryPanel;
    public GameObject equipmentPanel;
    public GameObject storagePanel;
    
    public Button buttonPrefab;

    //references to either player or storage UI elements
    Image itemIcon;
    Text itemName;
    Text itemDescription;
    Text itemPower;
    Button button1;
    Button button2;


    [Space(10)]
    
    [Header("Player inventory")]
    public RectTransform content;
    public Text goldText;
    public Dropdown sortDrop;
    public Image playerItemIcon;
    public Text playerItemName;
    public Text playerItemDescription;
    public Text playerItemPower;
    public Button playerButton1;
    public Button playerButton2;
    [Header("Storage Inventory")]
    public RectTransform sContent;
    public Text titleText;
    public Image storageItemIcon;
    public Text storageItemName;
    public Text storageItemDescription;
    public Text storageItemPower;
    public Button storageButton;
    #endregion

    #region start
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        equipment = GetComponent<Equipment>();
        CloseInventory();

        //setup sort dropdown
        sortDrop.ClearOptions();
        List<string> sortOptions = new List<string> {"All", "Consumables", "Equipment", "Craft", "Key items" };
        sortDrop.AddOptions(sortOptions);

        //add axe and helmet
        AddItem(ItemData.CreateItem(100, 1));
        AddItem(ItemData.CreateItem(201, 1));
    }
    #endregion

    #region update
    private void Update()
    {
        if (Input.GetKeyDown(player.inventoryKey) && (player.currentMenu == null || player.currentMenu == "Inventory"))
        {
            if (!PauseHandler.paused)
            {
                OpenInventory();
            }
            else
            {
                if (DialogueControl.inShop)
                {
                    GetComponent<DialogueControl>().CloseShop();
                }
                else
                {
                    CloseInventory();
                }
            }
        }

        //TESTING
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.I))
        {
            AddItem(ItemData.CreateItem(0, 3));
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            AddItem(ItemData.CreateItem(1, 1));
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddItem(ItemData.CreateItem(100, 1));
        }
        #endif
    }
    #endregion

    #region Add Item
    public static void AddItem(Item item)
    {

        #region add item to inventory
        //check for stacking
        if (item.StackSize > 1)
        {
            int remainingAmount = item.Amount;
            do
            {
                //add to any existing stacks
                foreach (Item checkItem in inventory)
                {
                    if (checkItem.ID == item.ID)
                    {
                        while (checkItem.Amount < checkItem.StackSize && remainingAmount > 0)
                        {
                            checkItem.Amount += 1;
                            remainingAmount -= 1;
                        }
                    }
                }
                if (remainingAmount > 0)
                {
                    //make a new stack of 1 in inventory
                    inventory.Add(ItemData.CreateItem(item.ID, 1));
                    remainingAmount -= 1;

                }
            } while (remainingAmount > 0);
        }
        else
        {
            //add item as new
            inventory.Add(item);
        }

        #endregion


        //set up buttons
        GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().SetUpButtons();
        
    }

    

    #endregion

    #region Remove Item
    public static void RemoveItem(int number, int amount)
    {
        inventory[number].Amount -= amount;
        if (inventory[number].Amount <= 0)
        {
            inventory.Remove(inventory[number]);

            if ((sortType == null || sortType == "All") && GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().inventoryMode != InventoryMode.shop)
            {
                if (inventory.Count > number)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().SelectItem(number);
                }
                else if (inventory.Count > 0)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().SelectItem(number - 1);
                }
                else
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().Deselect();
                }
            }
            else
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().Deselect();
            }
        }
        else
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().SelectItem(number);
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().SetUpButtons();

    }

    #endregion

    #region set up buttons
    void SetUpButtons()
    {

        List<Item> filteredInventory = new List<Item>(inventory);
        foreach (Button itemButton in itemButtons)
        {
            GameObject.Destroy(itemButton.gameObject);
        }
        itemButtons.Clear();

        #region sort
        if (sortType != null && sortType != "All")
        {
            switch (sortType)
            {
                case "Consumables":
                    for (int i = 0; i < inventory.Count; i++)
                    {
                        if (filteredInventory[i].Type != ItemType.Food && filteredInventory[i].Type != ItemType.Potion)
                        {
                            filteredInventory[i] = null;
                        }
                    }
                    break;
                case "Equipment":
                    for (int i = 0; i < inventory.Count; i++)
                    {
                        if (filteredInventory[i].Type != ItemType.Weapon && filteredInventory[i].Type != ItemType.Armour)
                        {
                            filteredInventory[i] = null;
                        }
                    }
                    break;
                case "Craft":
                    for (int i = 0; i < inventory.Count; i++)
                    {
                        if (filteredInventory[i].Type != ItemType.Craft)
                        {
                            filteredInventory[i] = null;
                        }
                    }
                    break;
                case "Key":
                    for (int i = 0; i < inventory.Count; i++)
                    {
                        if (filteredInventory[i].Type != ItemType.Quest)
                        {
                            filteredInventory[i] = null;
                        }
                    }
                    break;
            }
            filteredInventory.RemoveAll(item => item == null);
           
        }
        #endregion


        //set content size to show buttons
        content.sizeDelta = new Vector2(0, 5 + 30 * filteredInventory.Count);

        //create buttons
        if (filteredInventory.Count > 0)
        {
            for (int i = 0; i < filteredInventory.Count; i++)
            {
                Button newButton = Instantiate(buttonPrefab);
                itemButtons.Add(newButton);
                newButton.transform.SetParent(content.transform);

                if (filteredInventory[i].Amount > 1)
                {
                    newButton.GetComponentInChildren<Text>().text = filteredInventory[i].Name + " x" + filteredInventory[i].Amount;
                }
                else
                {
                    newButton.GetComponentInChildren<Text>().text = filteredInventory[i].Name;
                }
 
                int buttonNumber = inventory.IndexOf(filteredInventory[i]);
                newButton.onClick.RemoveAllListeners();
                newButton.onClick.AddListener(() => SelectItem(buttonNumber));
            }
        }

        if (currentStorage != null)
        {
            SetUpStorageButtons();
        }
    }

    void SetUpStorageButtons()
    {
        //clear current buttons
        foreach (Button itemButton in itemButtons2)
        {
            GameObject.Destroy(itemButton.gameObject);
        }
        itemButtons2.Clear();

        //set content size to show buttons
        sContent.sizeDelta = new Vector2(0, 5 + 30 * currentStorage.inventory.Count);

        //create buttons
        if (currentStorage.inventory.Count > 0)
        {
            for (int i = 0; i < currentStorage.inventory.Count; i++)
            {
                Button newButton = Instantiate(buttonPrefab);
                itemButtons2.Add(newButton);
                newButton.transform.SetParent(sContent.transform);
                if (inventoryMode != InventoryMode.shop)
                {
                    if (currentStorage.inventory[i].Amount > 1)
                    {
                        newButton.GetComponentInChildren<Text>().text = currentStorage.inventory[i].Name + " x" + currentStorage.inventory[i].Amount;
                    }
                    else
                    {
                        newButton.GetComponentInChildren<Text>().text = currentStorage.inventory[i].Name;
                    }
                }
                else
                {
                    newButton.GetComponentInChildren<Text>().text = currentStorage.inventory[i].Name + " - " + Mathf.FloorToInt(currentStorage.inventory[i].Value * currentStorage.buyMultiplier) + "G";
                }
                
                Item item = currentStorage.inventory[i];
                newButton.onClick.RemoveAllListeners();
                newButton.onClick.AddListener(() => SelectItem(item));
            }
        }
    }
    #endregion

    #region sort

    public void ChangeSortType(int option)
    {
        switch(option)
        {
            case 0:
                sortType = "All";
                break;
            case 1:
                sortType = "Consumables";
                break;
            case 2:
                sortType = "Equipment";
                break;
            case 3:
                sortType = "Craft";
                break;
            case 4:
                sortType = "Key";
                break;
            default:
                sortType = null;
                break;
        }
        Deselect();
        SetUpButtons();
      
    }
    #endregion

    #region select/use item
    public void SelectItem(int number)
    {
        SelectItem(inventory[number]);
    }

    public void SelectItem(Item item)
    {

        #region inventory or storage
        bool inStorage = false;
        if (currentStorage == null || inventory.Contains(item))
        {
            //reference player inventory
            itemIcon = playerItemIcon;
            itemName = playerItemName;
            itemDescription = playerItemDescription;
            itemPower = playerItemPower;
            button1 = playerButton1;
            button2 = playerButton2;

            button1.interactable = true;
            button2.interactable = true;

            if (selectedItem == null)
            {
                itemIcon.gameObject.SetActive(true);
                itemName.gameObject.SetActive(true);
                itemDescription.gameObject.SetActive(true);
                itemPower.gameObject.SetActive(true);
            }
            selectedItem = item;
        }
        else
        {
            inStorage = true;
            //reference storage inventory
            itemIcon = storageItemIcon;
            itemName = storageItemName;
            itemDescription = storageItemDescription;
            itemPower = storageItemPower;
            button1 = storageButton;

            button1.interactable = true;

            if (selectedItem2 == null)
            {
                itemIcon.gameObject.SetActive(true);
                itemName.gameObject.SetActive(true);
                itemDescription.gameObject.SetActive(true);
                itemPower.gameObject.SetActive(true);
            }
            selectedItem2 = item;
        }
        #endregion

        #region name, icon, description
        itemIcon.sprite = item.Icon;
        if (item.Amount > 1)
        {
            itemName.text = item.Name + " x" + item.Amount;
        }
        else
        {
            itemName.text = item.Name;
        }
        itemDescription.text = item.Description;
        #endregion

        #region power text
        switch (item.Type)
        {
            case ItemType.Food:
                itemPower.text = "Food - Healing " + item.Power;
                break;
            case ItemType.Weapon:
                itemPower.text = "Weapon - Attack " + item.Power;
                break;
            case ItemType.Armour:
                itemPower.text = "Armour - Defence " + item.Power;
                break;
            case ItemType.Potion:
                itemPower.text = "Potion - Healing " + item.Power;
                break;
            case ItemType.Craft:
                itemPower.text = "Material";
                break;
            case ItemType.Quest:
                itemPower.text = "Quest item";
                break;
            case ItemType.Money:
                itemPower.text = "Money";
                break;
        }
        #endregion

        #region buttons
        switch(inventoryMode)
        {
            case (InventoryMode.equipment):
            {
                    #region consume/equip
                    switch (item.Type)
                    {
                        case ItemType.Food:
                            button1.gameObject.SetActive(true);
                            button1.GetComponentInChildren<Text>().text = "Eat";
                            button1.onClick.RemoveAllListeners();
                            button1.onClick.AddListener(() => UseConsumable(item));
                            if (player.characterStatus[0].currentValue == player.characterStatus[0].maxValue)
                            {
                                button1.interactable = false;
                            }
                            break;
                        case ItemType.Weapon:
                        case ItemType.Armour:
                            button1.gameObject.SetActive(true);
                            button1.onClick.RemoveAllListeners();
                            if (item.Equipped == false)
                            {
                                button1.GetComponentInChildren<Text>().text = "Equip";
                                button1.onClick.AddListener(() => EquipItem(item));
                            }
                            else
                            {
                                button1.GetComponentInChildren<Text>().text = "Unequip";
                                button1.onClick.AddListener(() => Unequip(item));
                                button2.gameObject.SetActive(false);
                            }
                            break;
                        case ItemType.Potion:
                            button1.gameObject.SetActive(true);
                            button1.GetComponentInChildren<Text>().text = "Drink";
                            break;
                        case ItemType.Craft:
                        case ItemType.Quest:
                        case ItemType.Money:
                            button1.gameObject.SetActive(false);
                            break;
                    }
                    #endregion

                    #region discard
                    if (!inStorage)
                    {
                        if (item.Equipped == false)
                        {
                            button2.gameObject.SetActive(true);
                            button2.GetComponentInChildren<Text>().text = "Discard";
                            button2.onClick.RemoveAllListeners();
                            button2.onClick.AddListener(() => Discard(item));
                        }
                    }
                    #endregion
                    break;
            }

            case (InventoryMode.chest):
            {
                #region store and take
                if (inStorage)
                {
                    button1.gameObject.SetActive(true);
                    button1.GetComponentInChildren<Text>().text = "Take";
                    button1.onClick.RemoveAllListeners();
                    button1.onClick.AddListener(() => Take(item));
                }
                else
                {
                    button1.gameObject.SetActive(true);
                    button1.GetComponentInChildren<Text>().text = "Store";
                    button1.onClick.RemoveAllListeners();
                    button1.onClick.AddListener(() => Store(item));
                    if (currentStorage.storage >= currentStorage.maxStorage)
                    {
                        button1.interactable = false;
                    }
                }
                #endregion
                break;
            }
                

            case (InventoryMode.shop):
            {
                    #region buy and sell
                    if (inStorage)
                    {
                        button1.gameObject.SetActive(true);
                        button1.GetComponentInChildren<Text>().text = "Buy for " + Mathf.FloorToInt(item.Value * currentStorage.buyMultiplier) + "G";
                        button1.onClick.RemoveAllListeners();
                        button1.onClick.AddListener(() => Buy(item));
                        if (gold < Mathf.FloorToInt(item.Value * currentStorage.buyMultiplier))
                        {
                            button1.interactable = false;
                        }
                    }
                    else
                    {
                        button1.gameObject.SetActive(true);
                        button1.GetComponentInChildren<Text>().text = "Sell for " + Mathf.FloorToInt(item.Value * currentStorage.sellMultiplier) + "G";
                        button1.onClick.RemoveAllListeners();
                        button1.onClick.AddListener(() => Sell(item));
                    }
                    #endregion
                    break;
            }
        }

        #endregion

    }

    #endregion

    #region deselect item
    //deselect in inventory
    public void Deselect()
    {
        selectedItem = null;

        playerItemIcon.gameObject.SetActive(false);
        playerItemName.gameObject.SetActive(false);
        playerItemDescription.gameObject.SetActive(false);
        playerItemPower.gameObject.SetActive(false);
        playerButton1.gameObject.SetActive(false);
        playerButton2.gameObject.SetActive(false);
    }

    //deselect in storage
    public void DeselectStorage()
    {
        selectedItem2 = null;

        storageItemIcon.gameObject.SetActive(false);
        storageItemName.gameObject.SetActive(false);
        storageItemDescription.gameObject.SetActive(false);
        storageItemPower.gameObject.SetActive(false);
        storageButton.gameObject.SetActive(false);
    }
    #endregion

    #region consume/discard
    public void UseConsumable(Item item)
    {
        //heal health by amount or set to max
        player.characterStatus[0].currentValue = Mathf.Min(player.characterStatus[0].currentValue + item.Power, player.characterStatus[0].maxValue);
        //remove consumed item
        RemoveItem(inventory.IndexOf(item), 1);
    }

    void Discard(Item item)
    {

        //drop item on floor
        GameObject dropMesh = item.Mesh;
        GameObject droppedItem = Instantiate(dropMesh, dropLocation.position, Quaternion.identity);
        droppedItem.transform.position = dropLocation.position;

        //make item drop
        droppedItem.AddComponent(typeof(Rigidbody));

        //remove from inventory
        RemoveItem(inventory.IndexOf(item), 1);

    }
    #endregion

    #region equip
    void EquipItem(Item item)
    {
        //equip item
        equipment.EquipItem(item, item.EquipType);
        //remove item from inventory
        RemoveItem(inventory.IndexOf(item), 1);
    }

    void Unequip(Item item)
    {
        equipment.Unequip(item.EquipType);
        Deselect();


    }
    #endregion

    #region store/take

    void Store(Item item)
    {
        //deselect item if stacking
        if (selectedItem != null && selectedItem2 != null)
        {
            if (selectedItem.ID == selectedItem2.ID && selectedItem.StackSize > 1)
            {
                DeselectStorage();
            }
        }
        //add item to storage
        currentStorage.AddItem(item);
        //remove from inventory
        RemoveItem(inventory.IndexOf(item), item.Amount);
        //show updated inventory
        SetUpButtons();
        SetTitleText();
        
        
    }

    void Take(Item item)
    {
        //deselect item if stacking
        if (selectedItem != null && selectedItem2 != null)
        {
            if (selectedItem.ID == selectedItem2.ID && selectedItem.StackSize > 1)
            {
                Deselect();
            }
        }
        //add item to inventory
        AddItem(item);
        //remove from storage
        currentStorage.RemoveItem(currentStorage.inventory.IndexOf(item), item.StackSize);
        //show updated inventory
        SetUpButtons();
        SetTitleText();

    }
    #endregion

    #region buy/sell
    void Sell(Item item)
    {

        //remove from inventory
        RemoveItem(inventory.IndexOf(item), 1);
        //add gold
        gold += Mathf.FloorToInt(item.Value * currentStorage.sellMultiplier);
        //show updated inventory
        SetUpButtons();
        SetGoldText();

        DeselectStorage();


    }

    void Buy(Item item)
    {
        if (gold > item.Value * currentStorage.buyMultiplier)
        {
            //add item to inventory
            AddItem(ItemData.CreateItem(item.ID, 1));
            //spend gold
            gold -= Mathf.FloorToInt(item.Value * currentStorage.buyMultiplier);
            //show updated inventory
            SetUpButtons();
            SetGoldText();
            SelectItem(item);
        }
        

    }
    #endregion

    #region open/close inventory
    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        equipmentPanel.SetActive(false);
        storagePanel.SetActive(false);
        switch(inventoryMode)
        {
            case InventoryMode.equipment:
                equipmentPanel.SetActive(true);
                break;
            case InventoryMode.chest:
            case InventoryMode.shop:
                storagePanel.SetActive(true);
                SetTitleText();
                break;
        }
        PauseHandler.Pause();
        PauseHandler.paused = true;
        player.currentMenu = "Inventory";

        SetUpButtons();
        SetGoldText();

        Deselect();
    }

    public void CloseInventory()
    {
        inventoryMode = InventoryMode.equipment;
        currentStorage = null;
        inventoryPanel.SetActive(false);
        equipmentPanel.SetActive(false);
        storagePanel.SetActive(false);
        PauseHandler.Resume();
        PauseHandler.paused = false;
        player.currentMenu = null;

        Deselect();
    }
    #endregion

    #region update text
    void SetTitleText()
    {
        if (currentStorage != null) {
            switch (inventoryMode)
            {
                case InventoryMode.chest:
                    titleText.text = (currentStorage.inventoryName + "\n" + "Space: " + (currentStorage.maxStorage - currentStorage.storage) + "/" + currentStorage.maxStorage);
                    break;
                case InventoryMode.shop:
                    titleText.text = (currentStorage.inventoryName);
                    break;
            }
        }
    }

    void SetGoldText()
    {
        goldText.text = "Gold: " + gold;
    }
    #endregion
}

public enum InventoryMode
{
    equipment,
    chest,
    shop
}
