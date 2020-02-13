using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{

    void Start()
    {
        Quaternion plusRotation = Quaternion.Euler( new Vector3(30, 0, 0));

        transform.rotation = transform.rotation * plusRotation;
        

    }

}
