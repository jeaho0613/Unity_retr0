using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public ItemBox[] itemBoxes;
    public GameObject winUI;
    public bool isGameOver = false;

    private void Start()
    {
        isGameOver = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }

        if (isGameOver == true) return;

        int count = 0;
        for (int index = 0; index < itemBoxes.Length; index++)
        {
            if (itemBoxes[index].isOveraped == true)
            {
                count++;
            }
        }

        if (count >= 3)
        {
            Debug.Log("게임 승리!");
            winUI.SetActive(true);
            isGameOver = true;
        }
    }

}
