using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HipsControlller : MonoBehaviour
{

    public BotController botController;
    public AiController aiController;


    [SerializeField] bool isAi;

    public void BodyPartTriggered(Collision collision)
    {
        CheckCollision(collision);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckCollision(collision);
        
    }

    void CheckCollision(Collision collision)
    {
        /*
        if (collision.gameObject.tag.Equals(GameManager.FloorTag) || collision.gameObject.tag.Equals(GameManager.RotationArenaTag))
        {
            if (!isAi)
                botController.TriggeredGround();
        }

        if (collision.gameObject.tag.Equals(GameManager.AiTag))
        {
            if (!isAi)
                botController.TriggeredAi();
        }
        */
        
        if (collision.gameObject.tag.Equals(GameManager.HoleTag))
        {
            if (!isAi)
            {
                botController.BotEnteredHole();                
            }
        }

        if (collision.gameObject.tag.Equals(GameManager.BotTag))
        {
            if (isAi)
                aiController.Triggered(collision.contacts, collision);
        }       
    }


}//
