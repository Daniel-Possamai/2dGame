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
        Vector3 mousePosition = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPosition = (player.transform.position + mousePosition) / 2f;
        targetPosition.z = transform.position.z;

        targetPosition.x = Mathf.Clamp(targetPosition.x, player.transform.position.x - MinX, player.transform.position.x + MinX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, player.transform.position.y - MinY, player.transform.position.y + MinY);

        transform.position = Vector3.Lerp(transform.position, targetPosition, Velocidade * Time.deltaTime);
    }
}
