using UnityEngine;

public class GroundItem : MonoBehaviour
{
    public Sprite itemSprite;
    private bool isCollected = false; // Flag para evitar múltiplas coletas

    void Start()
    {
        // Configura o sprite do item dropado
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = itemSprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            Inventory inventory = FindObjectOfType<Inventory>();
            if (inventory != null)
            {
                inventory.AddItemToInventory(itemSprite);
                isCollected = true; // Marca como coletado
                Destroy(gameObject);
            }
        }
    }
}