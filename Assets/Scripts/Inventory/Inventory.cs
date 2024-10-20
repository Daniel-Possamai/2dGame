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
    
    public Transform playerTransform; // Referência ao transform do player

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

    void Update()
    {
        // Verifica se a tecla Delete foi pressionada
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            Vector3 dropPosition = GetDropPositionInFrontOfPlayer();
            DropSelectedItem(dropPosition);
        }
    }

   private Vector3 GetDropPositionInFrontOfPlayer()
    {
        // Calcula a posição na frente do player
        float dropDistance = 1.0f; // Distância na frente do player
        Vector3 dropPosition = playerTransform.position;

        // Ajusta a posição de drop com base na direção em que o jogador está olhando
        if (playerTransform.localScale.x > 0)
        {
            // Jogador olhando para a direita
            dropPosition += new Vector3(dropDistance, 0, 0);
        }
        else
        {
            // Jogador olhando para a esquerda
            dropPosition += new Vector3(-dropDistance, 0, 0);
        }

        return dropPosition;
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
            itemRectTransform.anchorMin = new Vector2(0.2f, 0.2f); // 10% menor que o slot
            itemRectTransform.anchorMax = new Vector2(0.8f, 0.8f); // 10% menor que o slot
            itemRectTransform.pivot = new Vector2(0.5f, 0.5f);
            itemRectTransform.offsetMin = Vector2.zero;
            itemRectTransform.offsetMax = Vector2.zero;
        }
    }

    public void AddItemToInventory(Sprite itemSprite)
    {
        // Encontra o primeiro slot vazio e adiciona o item
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].transform.childCount == 0)
            {
                AddItem(itemSprite, i);
                return;
            }
        }

        Debug.LogWarning("Inventário cheio!");
    }

    public GameObject groundItemPrefab;

    public void DropSelectedItem(Vector3 dropPosition)
    {
        if (selectedItem == null)
        {
            Debug.LogError("Nenhum item selecionado para dropar");
            return;
        }

        GameObject groundItemObject = Instantiate(groundItemPrefab, dropPosition, Quaternion.identity);
        GroundItem groundItem = groundItemObject.GetComponent<GroundItem>();
        groundItem.itemSprite = selectedItem.GetComponent<Image>().sprite;

        // Restaura o tamanho do slot original
        Slot originalSlot = selectedItem.originalSlot;
        originalSlot.transform.localScale = Vector3.one;

        // Remover o item do inventário
        Destroy(selectedItem.gameObject);
        selectedItem = null;
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
        // Restaura a cor e o scale originais do slot e do item
        Slot slot = item.originalSlot;
        Image slotImage = slot.GetComponent<Image>();
        slotImage.color = new Color(1, 1, 1, 1); // Cor original

        item.transform.localScale = Vector3.one; // Scale original
        slot.transform.localScale = Vector3.one; // Scale original
    }

    public void HighlightItem(DraggableItem item)
    {
        // Altera a cor do slot e aumenta o scale do slot e do item para indicar que estão selecionados
        Slot slot = item.originalSlot;
        Image slotImage = slot.GetComponent<Image>();
        slotImage.color = new Color(1.5f, 1.5f, 1.5f, 1); // Cor mais clara

        item.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f); // Aumenta o scale do item
        slot.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f); // Aumenta o scale do slot
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

    // Restaura o tamanho do slot original
    originalSlot.transform.localScale = Vector3.one;

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