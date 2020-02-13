using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocreManager : MonoBehaviour
{
    public List<int> scores = new List<int>();

    private void Start()
    {
        while(scores.Count > 0)
        {
            scores.RemoveAt(0);
            
        }
    }
}
