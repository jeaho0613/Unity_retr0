using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    private new Renderer renderer;
    private Color originalColor;
    public Color touchColor;
    public bool isOveraped = false;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EndPoint")
        {
            isOveraped = true;
            renderer.material.color = touchColor;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "EndPoint")
        {
            isOveraped = false;
            renderer.material.color = originalColor;
        }
    }
}
