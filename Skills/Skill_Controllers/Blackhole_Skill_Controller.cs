using UnityEngine;
using System.Collections.Generic;

public class Blackhole_Skill_Controller : MonoBehaviour
{

    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    public float maxSize;
    public float growSpeed;
    public bool canGrow;

    private List<Transform> targets = new List<Transform>();

    private void Update() {

        if (canGrow) {
           transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Enemy>() != null) {

            other.GetComponent<Enemy>().FreezeTime(true);

            CreateHotKey(other);

        }
    }

    private void CreateHotKey(Collider2D other) {

        if (keyCodeList.Count == 0) {
            Debug.Log("No more keys");
            return;
        }

        GameObject newHotKey = Instantiate(hotKeyPrefab, other.transform.position + new Vector3(0, 2), Quaternion.identity);

        KeyCode chosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(chosenKey);

        Blackhole_Hotkey_Controller newHotKeyController = newHotKey.GetComponent<Blackhole_Hotkey_Controller>();

        newHotKeyController.SetupHotKey(chosenKey, other.transform, this);


    }

    public void AddEnemyToList(Transform _enemyTransform) {
        targets.Add(_enemyTransform);
    }


}
