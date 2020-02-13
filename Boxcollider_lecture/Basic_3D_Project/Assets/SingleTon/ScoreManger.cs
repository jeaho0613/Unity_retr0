using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManger : MonoBehaviour
{
    private static ScoreManger instance;
    public static ScoreManger GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<ScoreManger>();

            if (instance == null)
            {
                GameObject container = new GameObject("Score Manger");

                instance = container.AddComponent<ScoreManger>();
            }
        }
        return instance;
    }

    private int score = 0;

    private void Start()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public int Getsore()
    {
        return score;
    }

    public void AddScore(int newScore)
    {
        score = score + newScore;
    }

}
