using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloCoroutine : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(HelloUnity());
        StartCoroutine("HiCSharp");
        Debug.Log("END");
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StopCoroutine("HiCSharp");
        }
    }
    IEnumerator HelloUnity()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            Debug.Log("string Coroutine");

        }
    }

    IEnumerator HiCSharp()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            Debug.Log("Nomal Coroutine");
        }

    }

}

