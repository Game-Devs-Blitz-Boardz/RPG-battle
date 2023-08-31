using UnityEngine;
using System.Collections.Generic;

public class Blackhole_Skill_Controller : MonoBehaviour
{

    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;

    private bool canGrow = true;
    private bool canShrink;
    private bool canCreateHotKeys = true;
    private bool cloneAttackReleased;

    private int amountOfAttacks = 4;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotKey = new List<GameObject>();


    public void SetupBlackHole(float _maxSize, float _growSpeed,float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown) {

        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;

    }

    private void Update() {

        cloneAttackTimer -= Time.deltaTime;

        if (Input.GetKey(KeyCode.R)) {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink) {
           transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink) {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0) {
                Destroy(gameObject);
            }
        }

    }

    private void ReleaseCloneAttack() {
        DestroyHotKeys();
        cloneAttackReleased = true;
        canCreateHotKeys = false;
    }

    private void CloneAttackLogic() {
        if (cloneAttackTimer < 0 && cloneAttackReleased) {

            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);

            float xOffset;

            if (Random.Range(0,100) > 50) 
                xOffset = 2;
            else
                xOffset = -2;

            SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));
            amountOfAttacks--;

            if (amountOfAttacks <= 0) {
                canShrink = true;
                cloneAttackReleased = false;
            }
        }
    }

    private void DestroyHotKeys() {
        if (createdHotKey.Count <= 0) return;

        for (int i = 0; i < createdHotKey.Count; i++) {
            Destroy(createdHotKey[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Enemy>() != null) {

            other.GetComponent<Enemy>().FreezeTime(true);

            CreateHotKey(other);

        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        
        if (other.GetComponent<Enemy>() != null) {
            other.GetComponent<Enemy>().FreezeTime(false);
        }

    }

    private void CreateHotKey(Collider2D other) {

        if (keyCodeList.Count <= 0) {
            Debug.Log("No more keys");
            return;
        }

        if (!canCreateHotKeys) return;

        GameObject newHotKey = Instantiate(hotKeyPrefab, other.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKey.Add(newHotKey);

        KeyCode chosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(chosenKey);

        Blackhole_Hotkey_Controller newHotKeyController = newHotKey.GetComponent<Blackhole_Hotkey_Controller>();

        newHotKeyController.SetupHotKey(chosenKey, other.transform, this);


    }

    public void AddEnemyToList(Transform _enemyTransform) {
        targets.Add(_enemyTransform);
    }


}
