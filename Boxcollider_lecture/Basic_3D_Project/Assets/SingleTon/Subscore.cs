using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subscore : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            ScoreManger.GetInstance().AddScore(-2);
            Debug.Log(ScoreManger.GetInstance().Getsore());
        }
    }
}
