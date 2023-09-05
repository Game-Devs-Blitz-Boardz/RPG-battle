using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_ItemToolTip : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    public void ShowToolTip(ItemData_Equipment _item){

        
        itemNameText.text = _item.itemName;
        itemTypeText.text = _item.equipmentType.ToString();

        gameObject.SetActive(true);

    }

    public void HideToolTip() {
        gameObject.SetActive(false);
    }

}
