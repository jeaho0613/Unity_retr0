using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public ItemBox[] itemBoxes;
    public bool isGameOver = false;
    public GameObject winUI;
    

    public float LimitTime;
    public Text text_Timer;
    float checkTime = 1f;
    public bool isTimeCheck = false;

    void Start()
    {
        Screen.SetResolution(720, 480, false);
    }

    
    void Update()
    {
        

        if(Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Main");
        }

        if(isTimeCheck == false)
        {
            LimitTime -= Time.deltaTime;
            text_Timer.text = "시간 :" + Mathf.Round(LimitTime);
        }
        else
        {
            isTimeCheck = true;
        }

        if(LimitTime < checkTime)
        {
            
            text_Timer.text = "시간 경과";
            isGameOver = true;
            return;
        }

        if(isGameOver == true)
        {
            return;
        }

        int count = 0;
        for(int i = 0; i < 3; i++)
        {
            if(itemBoxes[i].isOveraped == true)
            {
                count++;
            }
        }

        if(count >=3)
        {
            Debug.Log("게임 승리");
            isGameOver = true;
            winUI.SetActive(true);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        
    }

   
}
