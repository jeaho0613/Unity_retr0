using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Vector3 move = new Vector3(-5, 5, -5);
    

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            Moves();
        }
    }

    void Moves()
    {
        transform.Translate(move * Time.deltaTime);
    }
}
