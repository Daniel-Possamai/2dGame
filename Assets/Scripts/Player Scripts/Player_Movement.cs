using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;



public class Player_Movement : MonoBehaviour
{

// Referência ao componente Animator para controlar animações
public Animator animator;

// Velocidade de movimento do jogador
public float speed;


// Direção do movimento do jogador
public Vector2 Pos;


// Referência ao componente Rigidbody2D para manipulação física
public Rigidbody2D rig;



    // Start é chamado antes da primeira atualização do frame
    void Start()
    {
        // Obtém o componente Animator anexado ao GameObject
        animator = GetComponent<Animator>();

        // Obtém o componente Rigidbody2D anexado ao GameObject
        rig = GetComponent<Rigidbody2D>();
    }

    // Update é chamado uma vez por frame
    void Update()
    {
        
    // Obtém a entrada horizontal do jogador (teclas A/D ou setas esquerda/direita)   
    Pos.x = Input.GetAxis("Horizontal");
    Pos.y = Input.GetAxis("Vertical");
    

    // Define a velocidade do Rigidbody2D com base na direção e velocidade
    // rig.velocity = new Vector2(Pos.x * speed, rig.velocity.y);

    rig.velocity = new Vector2(Pos.x * speed, Pos.y * speed);

    // Define o parâmetro "isRunning" do Animator com base na direção
    if (Pos.x != 0 || Pos.y != 0){
        
        if (Pos.x != 0){
            animator.SetBool("isHorizontal", true);
        } else {
            animator.SetBool("isVertical", true);
        }
        

    } else {
        animator.SetBool("isHorizontal", false);
        animator.SetBool("isVertical", false);
    }


    // Ajusta a escala do jogador para virar o sprite na direção do movimento
    if (Pos.x > 0){
        transform.localScale = new Vector2(1, 1);
    } else if (Pos.x < 0)
    {
        transform.localScale = new Vector2(-1, 1);
    }
}
}