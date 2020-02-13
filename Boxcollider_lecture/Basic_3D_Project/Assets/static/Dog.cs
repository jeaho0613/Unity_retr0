using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public string nickName;
    public float weigth;

    public static int  count = 0;

    private void Awake()
    {
        count = count + 1;

    }

    private void Start()
    {
        Bark();
    }
    public void Bark()
    {
        Debug.Log("모든 캐들의 수: " + count);
        Debug.Log(nickName + ": Bark!");
    }
   
    public static void ShowAinalType()
    {
        Debug.Log("이것은 개입니다.");
    }
}
