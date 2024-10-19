using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public GameObject slotPrefab;
    public Sprite emptySlotSprite;
    public Sprite selectedSlotSprite;
    public Sprite pickaxeSprite;

    private List<GameObject> slots = new List<GameObject>();
    private int selectedIndex = -1;

    void Start()
    {
        // Inicializa o inventário com slots vazios
        for (int i = 0; i < 12; i++)
        {
            GameObject slot = Instantiate(slotPrefab, transform);
            slot.GetComponent<Image>().sprite = emptySlotSprite;
            slots.Add(slot);
        }
        AddItem(pickaxeSprite);
        
        // Certifique-se de que o inventário esteja desativado no início
        // gameObject.SetActive(false); 
        
    }

    void Update()
    {
       
    }

    

    public void AddItem(Sprite itemSprite)
    {
        foreach (GameObject slot in slots)
        {
            Image slotImage = slot.GetComponent<Image>();
            if (slotImage.sprite == emptySlotSprite)
            {
                slotImage.sprite = itemSprite;
                break;
            }
        }
    }

    public void SelectSlot(int index)
    {
        if (selectedIndex >= 0)
        {
            slots[selectedIndex].GetComponent<Image>().sprite = emptySlotSprite;
        }
        selectedIndex = index;
        slots[selectedIndex].GetComponent<Image>().sprite = selectedSlotSprite;
    }
}