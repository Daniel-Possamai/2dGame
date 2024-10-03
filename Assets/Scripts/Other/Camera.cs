using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject player;

    
    public float MinX, MinY, Velocidade; 

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Velocidade * Time.deltaTime);

        transform.position = new Vector2(
            Mathf.Clamp(transform.position.x,
            player.GetComponent<Transform>().position.x - MinX,
            player.GetComponent<Transform>().position.x + MinX),
            Mathf.Clamp(transform.position.y,
            player.GetComponent<Transform>().position.y - MinY,
            player.GetComponent<Transform>().position.y + MinY));
            
    }
}
