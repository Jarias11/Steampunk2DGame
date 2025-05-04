using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    [System.Serializable]
    public class Drop{
        public Item item;
        public int quantity = 1;
        
    }

    [Header("Drop Settings")]
    [SerializeField] GameObject worldItemPrefab;
    [SerializeField] private Drop[] drops;

    public void DropItems(){
        foreach (var drop in drops){
            if(drop.item == null || drop.quantity <= 0) continue;

            GameObject go = Instantiate(worldItemPrefab, transform.position, Quaternion.identity);
            WorldItem worldItem = go.GetComponent<WorldItem>();
            if (worldItem != null){
                worldItem.Initialize(drop.item, drop.quantity);
            }
            else{
                Debug.LogError("WorldItem component not found on the prefab.");
            }
        }
    }
}