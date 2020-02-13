using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spwaner : MonoBehaviour
{
    public Transform spawnPosition;
    public GameObject target;
    //public Rigidbody target;
    void Start()
    {
        GameObject instance = Instantiate(target, spawnPosition.position, spawnPosition.rotation);
        //Rigidbody instance = Instantiate(target, spawnPosition.position, spawnPosition.rotation);
        // Instantiate는 retrun 값으로 찍어낸 obj를 돌려줌 
        // rigidbody 로 불러와도 동작한다. 단 instantiate 은 rigidbody로  return 함

        //instance.AddForce(0, 1000, 0); //rigidbody로 가져오면 바로 AddForce 사용가능
        instance.GetComponent<Rigidbody>().AddForce(0, 1000, 0); // GameObj로 가져오면 컴포넌트를 따로 가져와야함

        Debug.Log(instance.name);

        
    }

     
}
