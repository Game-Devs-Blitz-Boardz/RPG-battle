using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{

    public static GameManager instance;

    private Transform player;

    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private string closestCheckpointId;

    [Header("Lost currency")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;

    private void Awake() {
        if (instance != null) {
            Destroy(instance.gameObject);
        } else {
            instance = this;
        }

        checkpoints = FindObjectsOfType<Checkpoint>();
    }

    private void Start() {
        player = PlayerManager.instance.player.transform;
    }

    public void RestartScene() {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
    {
        
        // closestCheckpointId = _data.closestCheckpointId;

        StartCoroutine(LoadWIthDelay(_data));

        Debug.Log("Game manager loaded");

    }


    public void SaveData(ref GameData _data)
    {

        _data.lostCurrencyAmount = lostCurrencyAmount;
        _data.lostCurrencyX = player.position.x;
        _data.lostCurrencyY = player.position.y;

        if (FindClosestCheckpoint() != null)
            _data.closestCheckpointId = FindClosestCheckpoint().id;

        _data.checkpoints.Clear();

        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.id, checkpoint.activationStatus);
        }
    }


    private IEnumerator LoadWIthDelay(GameData _data) {
        yield return new WaitForSeconds(0.1f);

        LoadCheckpoints(_data);
        LoadClosestCheckpoint(_data);
        LoadLostCurrency(_data);

    }

    private void LoadCheckpoints(GameData _data) {
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
        {
            foreach (Checkpoint checkpoint in checkpoints)
            {
                if (checkpoint.id == pair.Key && pair.Value == true)
                {

                    checkpoint.ActivateCheckpoint();
                }
            }    
        }    
    }

    private void LoadLostCurrency(GameData _data) {
        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostCurrencyX = _data.lostCurrencyX;
        lostCurrencyY = _data.lostCurrencyY;

        if (lostCurrencyAmount > 0) {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX, lostCurrencyY), Quaternion.identity);
            newLostCurrency.GetComponent<LostCurrency_Controller>().currency = lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;

    }

    public void LoadClosestCheckpoint(GameData _data) {

        closestCheckpointId = _data.closestCheckpointId;

        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (_data.closestCheckpointId == checkpoint.id)
            {
                player.position = checkpoint.transform.position;
            }
        }

    }

    private Checkpoint FindClosestCheckpoint() {

        float closestDistance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;

        foreach (Checkpoint checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(player.position, checkpoint.transform.position);

            if (distanceToCheckpoint < closestDistance && checkpoint.activationStatus == true) {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }

        return closestCheckpoint;
    }

}
