using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloClass : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Animal jack = new Animal();
        jack.name = "JACK";
        jack.sound = "Bark";
        jack.weight = 4.5f;

        Animal nate = new Animal();
        nate.name = "NATE";
        nate.sound = "Nyaa";
        nate.weight = 1.2f;

        Animal ANNIE = new Animal();
        nate.name = "ANNIE";
        nate.sound = "adad";
        nate.weight = 0.8f;

        nate = jack;

        nate.name = "JIMMY";

        Debug.Log(jack.name);
        Debug.Log(jack.sound);
        

        Debug.Log(nate.name);
        Debug.Log(nate.sound);
        

        Debug.Log(ANNIE.name);
        Debug.Log(ANNIE.sound);
        
    }

   
       
}

public class Animal
{
    public string name;
    public string sound;
    public float weight;
}


