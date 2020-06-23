using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    [SerializeField] bool moving;
    [SerializeField] float time =1;
    [SerializeField] float Distance;

    private void Start()
    {
       WallMovment();
    }
    
    void WallMovment()
    {
        if(moving)
            MoveWallC = StartCoroutine(MoveWallRoutine());
    }

    Coroutine MoveWallC;
    IEnumerator MoveWallRoutine ()
    {
        while (true)
        {
            transform.position = new Vector3(Mathf.Lerp(-Distance, Distance, Mathf.PingPong(Time.time / time, 1)), transform.position.y, transform.position.z);

            yield return new WaitForFixedUpdate();
        }
        
    }

}
