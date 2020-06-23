using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
    [SerializeField] public Rigidbody rb;
    [SerializeField] float foreceSpeed;
    
    [SerializeField] public Rigidbody[] rigidbodies;
    

    [SerializeField] Transform[] arms;
    [SerializeField] Transform hips;

    public bool isLastStickman = false;

    void Start()
    {
        transform.localEulerAngles = GameManager.self.shooterController.transform.localEulerAngles ;


        AddForce();

        CheckVelocityC = StartCoroutine(CheckVelocityRountine());

    }

    public void DisableBot()
    {
        isLastStickman = false;
        if (CheckVelocityC != null) StopCoroutine(CheckVelocityC);
    }

    public void LastStickman()
    {
        isLastStickman = true;
    }

    void AddForce()
    {        
        rb.AddForce(transform.forward * foreceSpeed, ForceMode.Impulse);  //for Bullet      
        //rb.AddForce(transform.up * foreceSpeed, ForceMode.Impulse);  //for stickman      
    }

    private Coroutine CheckVelocityC;
    IEnumerator CheckVelocityRountine ()
    {
       
        yield return new WaitForSeconds(0.1f);

        
        while (true)
        {
           
            
            float speed = rb.velocity.magnitude;

            if (isLastStickman)
            {

                yield return new WaitForSeconds(1.2f);
                rb.velocity = Vector3.zero;
            }

            if (speed < 0.2f)
            {
                
                if(isLastStickman)
                    GameManager.self.LastStickmanStopped();

                break;
            }
            
            yield return new WaitForEndOfFrame();            
        }
        
    }

    private void BodyRotations() // for Stickman
    { 
        transform.localEulerAngles = GameManager.self.shooterController.transform.localEulerAngles + new Vector3(90, 0, 0);

        float RandomRotateY = Random.Range(-180, 180);
        float randomArmsXRotation = Random.Range(-50, 50);

        transform.Rotate(0, RandomRotateY, 0);

        hips.localEulerAngles = new Vector3(randomArmsXRotation, randomArmsXRotation, Random.Range(-30, 30));

        
        for (int i = 0; i < arms.Length; i++)
        {
            arms[i].Rotate(randomArmsXRotation, 0, 0);
        }

    }

    public void BotEnteredHole()
    {
        Destroy(gameObject,1);
        if (isLastStickman)
            GameManager.self.LastStickmanStopped();
    }

    bool isUnfreezed = false;
    public void TriggeredGround()
    {
        UnFreezeRotations();  
    }

    public void TriggeredAi()
    {
        UnFreezeRotations();
    }
    

    void UnFreezeRotations()
    {
        if (isUnfreezed) return;

        isUnfreezed = true;
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].constraints = RigidbodyConstraints.None;
        }        
        
    }
    

}//
