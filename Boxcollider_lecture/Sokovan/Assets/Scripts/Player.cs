using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 30f;
    Rigidbody playerRigidbody;
    public GameManager gameManager;

    
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (gameManager.isGameOver == true)
        {
            return;
        }
            // 유저 입력 받기
            float inputX = Input.GetAxis("Horizontal");
            float inputZ = Input.GetAxis("Vertical");

            float fallSpeed = playerRigidbody.velocity.y;

            Vector3 velocity = new Vector3(inputX, 0, inputZ);

            velocity = velocity * speed;

            velocity.y = fallSpeed;

            playerRigidbody.velocity = velocity;
        

    }
}
