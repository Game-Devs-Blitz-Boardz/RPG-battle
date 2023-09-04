using UnityEngine;
using System.Collections.Generic;

public class ItemDrop : MonoBehaviour
{

    [SerializeField] private int possibleItemDrop;
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>();
    
    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private ItemData item;


    public void GenerateDrop() {
        for (int i = 0; i < possibleDrop.Length; i++) {

            if (Random.Range(0, 100) <= possibleDrop[i].dropChance) {
                dropList.Add(possibleDrop[i]);
            }
        }

        for (int i = 0; i<possibleItemDrop; i++) {
            ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];

            dropList.Remove(randomItem);
            DropItem(randomItem);
        }

    }

    public void DropItem(ItemData _itemData) {

        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-5f, 5f), Random.Range(15f, 20f));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);

    }

}