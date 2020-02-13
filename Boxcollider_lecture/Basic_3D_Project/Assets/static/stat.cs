using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("TEST Dog의 총개수" + Dog.count);
        Dog.ShowAinalType();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
