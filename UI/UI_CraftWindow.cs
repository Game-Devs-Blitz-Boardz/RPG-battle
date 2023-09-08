using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CraftWindow : MonoBehaviour
{
    
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemDescription;
    [SerializeField] Image itemIcon;
    [SerializeField] Button craftButton;

    [SerializeField] private Image[] materialImage;

    public void SetupCraftWindow(ItemData_Equipment _data) {


        craftButton.onClick.RemoveAllListeners();

        for (int i = 0; i < materialImage.Length; i++) {
            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for (int i =0; i < _data.craftingMaterials.Count; i++) {

            if (_data.craftingMaterials.Count > materialImage.Length) {
                Debug.LogWarning("You have more materials that material slots in craft window");
            }

            materialImage[i].sprite = _data.craftingMaterials[i].data.icon;
            materialImage[i].color = Color.white;

            TextMeshProUGUI materialSlotText = materialImage[i].GetComponentInChildren<TextMeshProUGUI>();

            materialSlotText.color = Color.white;
            materialSlotText.text = _data.craftingMaterials[i].stackSize.ToString();

        }

        itemIcon.sprite = _data.icon;
        itemName.text = _data.itemName;
        itemDescription.text = _data.GetDescription();

        craftButton.onClick.AddListener(() => Inventory.instance.CanCraft(_data, _data.craftingMaterials));

    }

}
