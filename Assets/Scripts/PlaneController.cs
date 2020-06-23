using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class PlaneController : MonoBehaviour
{
    [SerializeField] SplineFollower splineFollower;
    [SerializeField] Rigidbody rb;

    [SerializeField] Transform bigPorp;
    [SerializeField] Transform smallProp;
    [SerializeField] float speed;

    public bool Falldown = false;

    private void Start()
    {
        RotatePropsC = StartCoroutine(RotateProps());
    }


    Coroutine RotatePropsC;
    IEnumerator RotateProps()
    {
        while (true)
        {
            bigPorp.Rotate(-Vector3.right * speed * Time.deltaTime);
            smallProp.Rotate(-Vector3.right * speed * Time.deltaTime);
                       
            yield return new WaitForEndOfFrame();
        }
    }

    public void Triggered(BotController botController)
    {
        Vector3 distanc = transform.position - botController.transform.position;
        float speed = botController.rb.velocity.magnitude;
         
        splineFollower.enabled = false;
        rb.isKinematic = false;
        rb.useGravity = true;

        if (RotatePropsC != null) StopCoroutine(RotatePropsC);

        rb.AddForce(distanc.normalized * speed * 1f,ForceMode.Impulse);
       
    }


    public List<BotController> botControllers = new List<BotController>();

    private void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.tag.Equals(GameManager.BotTag))
        {

            BotController botController = collision.gameObject.GetComponentInParent<BotController>();
            //BotController botController = collision.gameObject.GetComponent<BotController>();


            if (botController == null)
            {
                botController = collision.gameObject.GetComponent<BotController>();
            }

            if (botControllers.IndexOf(botController) == -1)
            {
                botControllers.Add(botController);

                Triggered(botControllers[0]);
            }
           
        }
    }



}
