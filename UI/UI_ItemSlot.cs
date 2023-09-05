using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    private UI ui;
    public InventoryItem item;

    private void Start() {
        ui = GetComponentInParent<UI>();
    }

    public void UpdateSlot(InventoryItem _newItem) {

        item = _newItem;

        itemImage.color = Color.white;

        if (item != null) {

            itemImage.sprite = item.data.icon;

            if (item.stackSize > 1) {
                itemText.text = item.stackSize.ToString();
            } else {
                itemText.text = "";
            }

        }
    }

    public void CleanupSlot() {

        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;

        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData) {

        if (item == null) return;

        if (Input.GetKey(KeyCode.LeftControl)) {
            Inventory.instance.RemoveItem(item.data);
            return;
        }

        if (item.data.itemType == ItemType.Equipment) {
            Inventory.instance.EquipItem(item.data);
        }

    }

    public void OnPointerEnter(PointerEventData eventData) {

        ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);

    }

    public void OnPointerExit(PointerEventData eventData) {

        ui.itemToolTip.HideToolTip();

    }

}
