using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticalController : MonoBehaviour
{
    public PiramidaController piramidaController;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals(GameManager.BotTag))
        {
            piramidaController.DisableAboveRagdoll();
        }
    }


}
