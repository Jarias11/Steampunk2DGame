using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public Item itemData;
    public int quantity = 1;
    private SpriteRenderer sR;

    private void Awake()
    {
        sR = GetComponent<SpriteRenderer>();
    }
    public void Initialize(Item item, int count)
    {
        itemData = item;
        this.quantity = count;
        if (itemData != null && itemData.worldIcon != null)
        {
            sR.sprite = itemData.worldIcon;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.playerInventory.AddItem(itemData, quantity);
            Destroy(gameObject);
        }
    }
}
