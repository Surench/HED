using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    [SerializeField] Transform[] path;
    int current = 0;
    float rotSpeed;
    public float speed;
     float radius=0.2f;



    [SerializeField] Rigidbody rb;
  
   

    private void Start()
    {
       
    }


    private void Updated()
    {
        if (Vector3.Distance(path[current].position,transform.position) < radius)
        {
            current++;
            if (current >= path.Length) current = 0;
            //transform.LookAt(path[current].position);
        }

        transform.position = Vector3.MoveTowards(transform.position, path[current].position, Time.deltaTime * speed);
    }


    public List<BotController> botControllers = new List<BotController>();


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals(GameManager.BotTag))
        {
            BotController botController = collision.gameObject.GetComponentInParent<BotController>();


            if (botController == null)
            {
                botController = collision.gameObject.GetComponent<BotController>();
            }

            if (botControllers.IndexOf(botController) == -1)
            {
                botControllers.Add(botController);

                //DO SOMETHING

                
            }


        }
    }

}
