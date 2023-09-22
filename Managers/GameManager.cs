using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{

    public static GameManager instance;
    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private string closestCheckpointId;

    private void Awake() {
        if (instance != null) {
            Destroy(instance.gameObject);
        } else {
            instance = this;
        }

        checkpoints = FindObjectsOfType<Checkpoint>();
    }

    public void RestartScene() {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
    {

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

        closestCheckpointId = _data.closestCheckpointId;

        // foreach (Checkpoint checkpoint in checkpoints)
        // {
        //     if (_data.closestCheckpointId == checkpoint.id)
        //     {
        //         PlayerManager.instance.player.transform.position = checkpoint.transform.position;
        //     }
        // }

        // Invoke(PlacePlayerAtClosestCheckpoint(_data), 0.1f);       
        PlacePlayerAtClosestCheckpoint(_data);

        Debug.Log("Game manager loaded");

    }

    public void PlacePlayerAtClosestCheckpoint(GameData _data) {

        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (_data.closestCheckpointId == checkpoint.id)
            {
                PlayerManager.instance.player.transform.position = checkpoint.transform.position;
            }
        }

    }

    public void SaveData(ref GameData _data)
    {

        _data.closestCheckpointId = FindClosestCheckpoint().id;
        _data.checkpoints.Clear();

        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.id, checkpoint.activationStatus);
        }
    }

    private Checkpoint FindClosestCheckpoint() {

        float closestDistance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;

        foreach (Checkpoint checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(PlayerManager.instance.player.transform.position, checkpoint.transform.position);

            if (distanceToCheckpoint < closestDistance && checkpoint.activationStatus == true) {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }

        return closestCheckpoint;
    }
}
