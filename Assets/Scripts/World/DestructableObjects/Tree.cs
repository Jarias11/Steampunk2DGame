using UnityEngine;

public class Tree : MonoBehaviour
{
    private Health health;
    private ItemDropper itemDropper;

    private void Awake()
    {
        health = GetComponent<Health>();
        itemDropper = GetComponent<ItemDropper>();

        if (health != null)
        {
            health.Died += OnHarvested;
        }
    }

    private void OnHarvested()
    {
        itemDropper?.DropItems();
        Destroy(gameObject);
    }
}