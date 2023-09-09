using UnityEngine;

public class PlayerManager : MonoBehaviour
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

}
