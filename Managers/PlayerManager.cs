using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
{

    public static PlayerManager instance;
    public Player player;

    public int currency;

    private void Awake() {

        if (instance != null) Destroy(instance.gameObject);
        else instance = this;

    }

    public bool HaveAnoughMoney(int _price) {

        if (_price > currency) {
            Debug.Log("Not enough money");
            return false;
        }

        currency -= _price;
        return true;
    }

    public int GetCurrency() {
        return currency;
    }

    public void LoadData(GameData _data)
    {
        this.currency = _data.currency;
    }

    public void SaveData(ref GameData _data)
    {
        _data.currency = this.currency;
    }
}
