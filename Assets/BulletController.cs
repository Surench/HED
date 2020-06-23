using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    
    [SerializeField] Rigidbody rb;
    [SerializeField] float foreceSpeed;

    [SerializeField] public Rigidbody[] rigidbodies;


    [SerializeField] Transform[] arms;
    [SerializeField] Transform hips;

    public bool isLastStickman = false;

    void Start()
    {       
        AddForce();

        CheckVelocityC = StartCoroutine(CheckVelocityRountine());

    }

   

    void AddForce()
    {
        rb.AddForce(transform.up * foreceSpeed, ForceMode.Impulse);
    }

    private Coroutine CheckVelocityC;
    IEnumerator CheckVelocityRountine()
    {
        yield return new WaitForSeconds(0.1f);

        while (true)
        {


            float speed = rb.velocity.magnitude;

            if (speed < 0.2f)
            {

                if (isLastStickman)
                    GameManager.self.LastStickmanStopped();

                break;
            }

            yield return new WaitForEndOfFrame();
        }

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

    public void BotEnteredHole()
    {
        Destroy(gameObject, 1);
        if (isLastStickman)
            GameManager.self.LastStickmanStopped();
    }

   

}
