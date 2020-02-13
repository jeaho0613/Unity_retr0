using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Addscore : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Start Score" + ScoreManger.GetInstance().Getsore());
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ScoreManger.GetInstance().AddScore(5);
            Debug.Log(ScoreManger.GetInstance().Getsore());
        }
    }
}
