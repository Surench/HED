using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{

    [SerializeField] float power ;
    [SerializeField] float radius;
    [SerializeField] float upforce;

    [SerializeField] GameObject bombVisual;
    [SerializeField] GameObject ExplosionSphere;
    [SerializeField] Animator anim;    

    List<AiController> aiControllers = new List<AiController>();
    List<BotController> botControllers = new List<BotController>();

    private void Start()
    {
        InitBomb();   
    }

    
    void InitBomb()
    {
      

        bombVisual.SetActive(true);
        ExplosionSphere.SetActive(false);

        ExplosionSphere.transform.localScale = new Vector3(radius, radius, radius);
    }

    void Explode()
    {
        anim.Play(GameManager.self.StringToHashes[48],-1,0);

        bombVisual.SetActive(false);
        ExplosionSphere.SetActive(true);

        Invoke("disableExplosionSphere", 0.5f);
    }

    

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag.Equals(GameManager.BotTag))
        {
            Explode();

            BotController botController = other.gameObject.GetComponentInParent<BotController>();

            if (botController == null)
            {
                botController = other.gameObject.GetComponent<BotController>();
            }

            if (botControllers.IndexOf(botController) == -1)
            {
                botControllers.Add(botController);
               // botController.rigidbodies[5].velocity = Vector3.zero; for stickman
                botController.rigidbodies[0].AddExplosionForce(power, transform.position, radius, upforce, ForceMode.Impulse);
            }
        }

        if (other.gameObject.tag.Equals(GameManager.AiTag))
        {
            AiController aiController = other.GetComponentInParent<AiController>();

            if (aiController == null)
            {
                aiController = other.GetComponent<AiController>();
            }

            if (aiControllers.IndexOf(aiController) == -1)
            {
                aiControllers.Add(aiController);

                aiController.FallDown();
                
                aiController.rigidbodies[0].AddExplosionForce(power, transform.position, radius, upforce, ForceMode.Impulse);
            }
        }
    }

    void disableExplosionSphere()
    {
        ExplosionSphere.SetActive(false);
    }


    /*
    [SerializeField] Collider bombColider;

    


    void Explosion ()
    {
        bombColider.transform.localScale = new Vector3(4, 4, 4);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals(GameManager.BotTag))
        {
            Explosion();
        }

        Debug.Log(collision.gameObject.tag);
    }

    void oldExpolison()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider hit in colliders)
        {
            Debug.Log(hit.gameObject.tag);
            if (hit.gameObject.tag.Equals(GameManager.AiTag))
            {
                AiController aiController = hit.GetComponentInParent<AiController>();

                aiController.EnableRagdoll();


                Rigidbody rb = hit.GetComponent<Rigidbody>();

                rb.AddExplosionForce(power, transform.position, radius, upforce, ForceMode.Impulse);

            }


        }

      

    }*/
}



    
    

