using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiramidaController : MonoBehaviour
{
    [SerializeField] AiController[] aiControllers;

    public void DisableAboveRagdoll()
    {
        for (int i = 0; i < aiControllers.Length; i++)
        {
            aiControllers[i].FallDown();
        } 
    }



}//
