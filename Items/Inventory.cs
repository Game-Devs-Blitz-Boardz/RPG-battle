using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public static Inventory instance;

    public List<ItemData> startingItems;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_StatSlot[] statSlot;

    [Header("Items cooldown")]
    private float lastTimeUsedFlask;
    private float lastTimeUsedArmour;

    private float flaskCooldown;
    private float armourCooldown;


    private void Awake() {
        if (instance == null) 
            instance = this;
        else Destroy(gameObject);
    }

    private void Start() {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();


        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        statSlot = statSlotParent.GetComponentsInChildren<UI_StatSlot>();

        AddStartingItems();

    }

    private void AddStartingItems() {
        for (int i = 0; i < startingItems.Count; i++) {
            AddItem(startingItems[i]);
        }
    }

    public void EquipItem(ItemData _item) {

        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary) {

            if (item.Key.equipmentType == newEquipment.equipmentType) {
                oldEquipment = item.Key;
            }

        }

        if (oldEquipment != null) {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment); 
        }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        // newEquipment.AddModifiers();

        RemoveItem(_item);

        UpdateSlotUI();
    }

    public void UnequipItem(ItemData_Equipment _itemToRemove) {

         if (equipmentDictionary.TryGetValue(_itemToRemove, out InventoryItem value)) {

            equipment.Remove(value);
            equipmentDictionary.Remove(_itemToRemove);
            // _itemToRemove.RemoveModifiers();
        }

    }

    private void UpdateSlotUI() {

        for (int i = 0; i<equipmentSlot.Length; i++) {

            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary) {

                if (item.Key.equipmentType == equipmentSlot[i].slotType) {
                    equipmentSlot[i].UpdateSlot(item.Value);
                }

            }

        }



        for (int i = 0; i < inventoryItemSlot.Length; i++) {
            inventoryItemSlot[i].CleanupSlot();
        }

        for (int i = 0; i<stashItemSlot.Length; i++) {
            stashItemSlot[i].CleanupSlot();
        }

        for (int i = 0; i < statSlot.Length; i++) {
            statSlot[i].UpdateStatValueUI();
        }



        for (int i = 0; i<inventory.Count; i++) {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++) {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }

    }

    public void AddItem(ItemData _item) {

        if (_item.itemType == ItemType.Equipment && CanAddItem()) {
            AddToInventory(_item);
        } else if (_item.itemType == ItemType.Material) {
            AddToStash(_item);
        }

        UpdateSlotUI();

    }

    private void AddToInventory(ItemData _item) {

        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value)) {
            value.AddStack();
        } else {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }

    }

    private void AddToStash(ItemData _item) {

        if (stashDictionary.TryGetValue(_item, out InventoryItem value)) {
            value.AddStack();
        } else {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }

    }

    public void RemoveItem(ItemData _item) {

        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value)) {

            if (value.stackSize <= 1) {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            } else {
                value.RemoveStack();
            }

        }

        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue)) {
            if (stashValue.stackSize <= 1) {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            } else {
                stashValue.RemoveStack();
            }
        }

        UpdateSlotUI();

    }

    public bool CanAddItem() {

        if (inventory.Count >= inventoryItemSlot.Length) {
            Debug.Log("Inventory is full");
            return false;
        }

        return true;

    }

    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials) {

        List<InventoryItem> materialsToRemove = new List<InventoryItem>();

        for (int i = 0; i < _requiredMaterials.Count; i++) {

            if (stashDictionary.TryGetValue(_requiredMaterials[i].data, out InventoryItem stashValue)) {

                if (stashValue.stackSize < _requiredMaterials[i].stackSize) {
                    Debug.Log("Not enough materials");
                    return false;
                } else {
                    materialsToRemove.Add(stashValue);
                }

                // add this to used material

            } else {
                Debug.Log("Not enough materials");
                return false;
            }

        }

        for (int i = 0; i < materialsToRemove.Count; i++) {
            RemoveItem(materialsToRemove[i].data);
        }

        AddItem(_itemToCraft);
        Debug.Log("Here is your item " + _itemToCraft.name);

        return true;

    }

    public List<InventoryItem> GetEquipmentList() => equipment;

    public List<InventoryItem> GetStashList() => stash;

    public ItemData_Equipment GetEquipment(EquipmentType _equipmentType) {

        ItemData_Equipment equipedItem = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary) {

            if (item.Key.equipmentType == _equipmentType) {
                equipedItem = item.Key;
            }

        }

        return equipedItem;

    }

    public void UseFlask() {

        ItemData_Equipment currentFlask = GetEquipment(EquipmentType.Flask);

        if (currentFlask == null) return;

        bool canUseFlask = Time.time > lastTimeUsedFlask + flaskCooldown;

        if (canUseFlask) {

            flaskCooldown = currentFlask.itemCooldown;
            currentFlask.Effect(null);
            lastTimeUsedFlask = Time.time;
        } else {
            Debug.Log("flask on cooldown");
        }

    }

    public bool CanUseArmour() {

        ItemData_Equipment currentArmour = GetEquipment(EquipmentType.Armour);

        if (Time.time > lastTimeUsedArmour + armourCooldown) {
            armourCooldown = currentArmour.itemCooldown;
            lastTimeUsedArmour = Time.time;
            return true;
        }

        Debug.Log("armour on cooldown");
        return false;

    }

}
