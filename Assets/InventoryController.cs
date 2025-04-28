using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < slotCount; i++)
        {
            slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<slot>();
            if(i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);  // inventoryPanel.transform).GetComponent<GameObject>();
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; 
                slot.currentItem = item;
            }
        }
    }
}
