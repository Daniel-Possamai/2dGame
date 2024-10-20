using UnityEngine;

public class GroundItem : MonoBehaviour
{
    public Sprite itemSprite;

    void Start()
    {
        // Configura o sprite do item dropado
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = itemSprite;

        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory inventory = FindObjectOfType<Inventory>();
            if (inventory != null)
            {
                inventory.AddItemToInventory(itemSprite);
                Destroy(gameObject);
            }
        }
    }
}