using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class output : MonoBehaviour
{
    void Start()
    {
        int num1 = 2;
        num1 = num1++ + ++num1;

        Debug.Log("num1 의 값은" + num1);




    }


}