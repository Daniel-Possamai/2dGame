using UnityEngine;

public class VoidZone : MonoBehaviour
{
    // Método chamado quando outro collider entra no trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto que entrou no trigger é o jogador
        if (collision.CompareTag("Player"))
        {
            // Aqui você pode definir o que acontece quando o jogador cai no void
            // Por exemplo, reiniciar a posição do jogador ou reiniciar o nível
            Debug.Log("Jogador caiu no void!");
            // Exemplo: Reiniciar a posição do jogador
            collision.transform.position = new Vector3(0, 0, 0); // Ajuste conforme necessário
        }
    }
}