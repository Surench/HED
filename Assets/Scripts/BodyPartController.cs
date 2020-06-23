using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartController : MonoBehaviour
{
    [SerializeField] HipsControlller hipsControlller;

    private void OnCollisionEnter(Collision collision)
    {
        hipsControlller.BodyPartTriggered(collision);
    }
}
