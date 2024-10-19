using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public GameObject slotPrefab;
    public Sprite emptySlotSprite;
    public Sprite pickaxeSprite;
    public Sprite axeSprite; // Novo item
    public Sprite swordSprite; // Novo item

    private List<Slot> slots = new List<Slot>();
    public DraggableItem selectedItem; // Tornar pública para acesso

    void Start()
    {
        // Inicializa o inventário com slots vazios
        for (int i = 0; i < 12; i++)
        {
            GameObject slotObject = Instantiate(slotPrefab, transform);
            Slot slot = slotObject.AddComponent<Slot>();
            slot.slotID = i;
            slotObject.GetComponent<Image>().sprite = emptySlotSprite;

            slots.Add(slot);
        }

        // Adiciona itens de exemplo
        AddItem(pickaxeSprite, 0);
        AddItem(axeSprite, 1); // Adiciona um machado no slot 1
        AddItem(swordSprite, 2); // Adiciona uma espada no slot 2
    }

    public void AddItem(Sprite itemSprite, int slotID)
    {
        if (slotID < 0 || slotID >= slots.Count)
        {
            Debug.LogError("ID de slot inválido");
            return;
        }

        Slot slot = slots[slotID];
        if (slot.transform.childCount == 0)
        {
            // Cria um GameObject filho para o item
            GameObject item = new GameObject("Item");
            item.transform.SetParent(slot.transform);
            Image itemImage = item.AddComponent<Image>();
            itemImage.sprite = itemSprite;
            itemImage.color = new Color(1, 1, 1, 1); // Opaque

            // Adiciona componentes de arrastar e soltar
            DraggableItem draggableItem = item.AddComponent<DraggableItem>();
            draggableItem.originalSlot = slot;

            // Ajusta o tamanho e a posição do item para ser menor que o slot
            RectTransform itemRectTransform = item.GetComponent<RectTransform>();
            itemRectTransform.anchorMin = new Vector2(0.1f, 0.1f); // 10% menor que o slot
            itemRectTransform.anchorMax = new Vector2(0.9f, 0.9f); // 10% menor que o slot
            itemRectTransform.pivot = new Vector2(0.5f, 0.5f);
            itemRectTransform.offsetMin = Vector2.zero;
            itemRectTransform.offsetMax = Vector2.zero;
        }
    }

    public void SelectItem(DraggableItem item)
    {
        // Se já houver um item selecionado, deselecione-o
        if (selectedItem != null)
        {
            DeselectItem(selectedItem);
        }

        // Selecione o novo item
        selectedItem = item;
        HighlightItem(selectedItem);
    }

    public void DeselectItem(DraggableItem item)
    {
        // Restaura a cor original do item
        Image itemImage = item.GetComponent<Image>();
        itemImage.color = new Color(1, 1, 1, 1); // Opaque
    }

    public void HighlightItem(DraggableItem item)
    {
        // Altera a cor do item para indicar que está selecionado
        Image itemImage = item.GetComponent<Image>();
        itemImage.color = new Color(1, 1, 0, 1); // Amarelo
    }

    public void MoveSelectedItemToSlot(Slot targetSlot)
    {
        if (selectedItem == null)
        {
            Debug.LogError("Nenhum item selecionado");
            return;
        }

        Slot originalSlot = selectedItem.originalSlot;

        if (targetSlot.transform.childCount == 0)
        {
            // Move o item para o novo slot
            selectedItem.transform.SetParent(targetSlot.transform);
            selectedItem.transform.localPosition = Vector3.zero;

            // Atualiza o originalSlot do item
            selectedItem.originalSlot = targetSlot;
        }
        else
        {
            // Troca os itens entre os slots
            Transform targetItem = targetSlot.transform.GetChild(0);
            targetItem.SetParent(originalSlot.transform);
            targetItem.localPosition = Vector3.zero;

            selectedItem.transform.SetParent(targetSlot.transform);
            selectedItem.transform.localPosition = Vector3.zero;

            // Atualiza os originalSlot dos itens
            DraggableItem targetDraggableItem = targetItem.GetComponent<DraggableItem>();
            targetDraggableItem.originalSlot = originalSlot;
            selectedItem.originalSlot = targetSlot;
        }

        // Deseleciona o item após a troca
        DeselectItem(selectedItem);
        selectedItem = null;
    }
}

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public int slotID;

    public void OnPointerClick(PointerEventData eventData)
    {
        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory.selectedItem != null)
        {
            inventory.MoveSelectedItemToSlot(this);
        }
    }
}

public class DraggableItem : MonoBehaviour, IPointerClickHandler
{
    public Slot originalSlot;

    public void OnPointerClick(PointerEventData eventData)
    {
        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory.selectedItem == null)
        {
            inventory.SelectItem(this);
        }
        else
        {
            // Troca os itens entre os slots
            Slot targetSlot = originalSlot;
            inventory.MoveSelectedItemToSlot(targetSlot);
        }
    }
}