using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BicycleController : MonoBehaviour
{
    [SerializeField] float wheelSpeed;
    [SerializeField] float pedalSpeed;

    [SerializeField] GameObject Wheel;
    [SerializeField] GameObject Pedal;

    [SerializeField] Rigidbody rigidbodie;

    void Start()
    {
        Debug.Break();
        RotationC = StartCoroutine(RotatationRoutine());
    }

    public void StopRoutine()
    {
        rigidbodie.isKinematic = false;
        rigidbodie.useGravity = true;

        if (RotationC != null)
        {
            StopCoroutine(RotationC);
        }
    }

    Coroutine RotationC;
    IEnumerator RotatationRoutine()
    {
     
        float duration = 1;
        float StarTime = Time.time;
        float startRot = Pedal.transform.localEulerAngles.z;
        while (true)
        {
            Pedal.transform.localEulerAngles = new Vector3(Pedal.transform.localEulerAngles.x, Pedal.transform.localEulerAngles.y, startRot - (360* (Time.time-StarTime)/duration));


            Pedal.transform.Rotate(-Vector3.forward * pedalSpeed * Time.deltaTime);

            Wheel.transform.Rotate(-Vector3.forward * wheelSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
   


}
