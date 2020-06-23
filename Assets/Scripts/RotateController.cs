using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateController : MonoBehaviour
{
    Coroutine rotateC;

  
    [SerializeField] float Speed=1 ;

    void Start()
    {
        rotateC = StartCoroutine(RotateRoutine());
    }


    IEnumerator RotateRoutine()
    {
        while (true)
        {
            transform.Rotate(-Vector3.up * Speed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
    }
}
