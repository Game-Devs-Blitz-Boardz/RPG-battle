using UnityEngine;
using System.Collections;
using TMPro;

public class UI : MonoBehaviour
{

    [Header("End Screen")]
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartButton;
    [Space]

    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;

    public UI_ItemToolTip itemToolTip;
    public UI_Stat_ToolTip statToolTip;
    public UI_SkillToolTip skillToolTip;
    public UI_CraftWindow craftWindow;

    private void Awake() {
        SwitchTo(skillTreeUI); // we need this to asign events on skill tree slots before we asign events on skill slots
    }

    private void Start() {
        SwitchTo(inGameUI);

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.C)) {
            SwitchWithKeyTo(characterUI);
        }
        if (Input.GetKeyDown(KeyCode.B)) {
            SwitchWithKeyTo(craftUI);
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            SwitchWithKeyTo(skillTreeUI);
        }
        if (Input.GetKeyDown(KeyCode.O)) {
            SwitchWithKeyTo(optionsUI);
        }

    }
    
    public void SwitchTo(GameObject _menu) {


        for (int i = 0; i < transform.childCount; i++) {

            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null; // we need this to keep fade screen game object active

            if (!fadeScreen)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null) {
            _menu.SetActive(true);
        }

    }

    public void SwitchWithKeyTo(GameObject _menu) {

        if (_menu != null && _menu.activeSelf) {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }

        SwitchTo(_menu);

    }

    private void CheckForInGameUI() {

        for (int i = 0; i < transform.childCount; i++)
        {

            if (transform.GetChild(i).gameObject.activeSelf) return;
        }

        SwitchTo(inGameUI);

    }

    public void SwitchOnEndScreen() {

        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCoroutine());
    }

    IEnumerator EndScreenCoroutine() {
        yield return new WaitForSeconds(1f);
        endText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        restartButton.SetActive(true);
    }

    public void RestartGamebutton() {
        GameManager.instance.RestartScene();
    }

}
