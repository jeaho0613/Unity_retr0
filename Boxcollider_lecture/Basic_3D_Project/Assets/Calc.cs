using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calc : MonoBehaviour
{
    void Start()
    {
        Debug.Log(sum(1,1));
        Debug.Log(sum(-5,8,12));
    }

    public int sum(int a, int b)
    {
        return a + b;
    }

    public int sum(int a, int b, int c)
    {
        return a + b + c;
    }
    
}
