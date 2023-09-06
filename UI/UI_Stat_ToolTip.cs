using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Stat_ToolTip : MonoBehaviour
{
    
    [SerializeField] TextMeshProUGUI description;

    public void ShowStatToolTip(string _text) {

        description.text = _text;

        gameObject.SetActive(true);

    }

    public void HideStatToolTip(string _text) {

        description.text = "";
        gameObject.SetActive(false);

    }

}
