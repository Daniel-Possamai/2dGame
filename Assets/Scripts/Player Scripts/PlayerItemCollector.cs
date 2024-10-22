using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    public Inventory inventory; // Referência ao inventário

    void Start()
    {
        if (inventory == null)
        {
            Debug.LogError("Referência ao inventário não atribuída no PlayerItemCollector.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GroundItem"))
        {
            GroundItem groundItem = other.GetComponent<GroundItem>();
            if (groundItem != null)
            {
                // Adiciona o item ao inventário
                inventory.AddItemToInventory(groundItem.itemSprite);
                // Destroi o item no chão
                Destroy(other.gameObject);
            }
        }
    }
}