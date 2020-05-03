using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10f;
    public GameManager gameManager;

    private Rigidbody playerRigidbody;
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (gameManager.isGameOver == true) return;
        float inputX = Input.GetAxis("Horizontal"); // 수평 값
        float inputZ = Input.GetAxis("Vertical"); // 수직 값
        float fallSpeed = playerRigidbody.velocity.y; // 보정 될 y값

        Vector3 velocity = new Vector3(inputX, 0, inputZ); // 속도를 줄 백터값

        velocity = velocity * speed; // 움직일 속도
        velocity.y = fallSpeed; // 보정 될 y값의 속도

        playerRigidbody.velocity = velocity; // player 속도
    }
}
