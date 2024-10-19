using UnityEngine;
using UnityEngine.EventSystems;

public class UIKeyListener : MonoBehaviour, IPointerClickHandler
{
    // Referência ao painel do inventário
    public GameObject inventoryPanel;


    void Start()
    {
        // Certifique-se de que o inventário está oculto no início
            inventoryPanel.SetActive(false);
    }

    void Update()
    {
        // Verifica se a tecla Tab foi pressionada
        if (Input.GetKeyDown(KeyCode.Tab))
        {

            // Abre o inventário
            if( inventoryPanel.activeSelf == false) {
                inventoryPanel.SetActive(true);

            }else{
                inventoryPanel.SetActive(false);
            }

            
                
                
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Garante que o GameObject receba foco quando clicado
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}