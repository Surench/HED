using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShooterController : MonoBehaviour
{
       
    [SerializeField] GameObject Stickman;
    [SerializeField] Transform Cannon;
    [SerializeField] Animator anim;

    [SerializeField] public List<BotController> botControllers;

    [SerializeField] GameObject shooterButton;

    public void DisableControl()
    {
        shooterButton.SetActive(false);
    }

    public void EnableControl()
    {
        shooterButton.SetActive(true);
    
    }


    public void ShootPlayer () 
    {
        if (GameManager.self.scoreManager.TotalShotsAmount > 0)
        {
                        
            GameObject obj = Instantiate(Stickman, transform.position, Quaternion.identity);
            BotController botController = obj.GetComponent<BotController>();
            botControllers.Add(botController);

            GameManager.self.scoreManager.PlayerShooted();

            if (GameManager.self.scoreManager.TotalShotsAmount == 0)
            {
                botController.LastStickman();
            }

#if UNITY_IOS
            if (GameManager.TapticEnabled) TapticEngine.TriggerLight();
#endif

           
        }


        anim.Play(GameManager.self.StringToHashes[49], -1, 0);


    }


    void LookAtPos(Vector2 MousePos)
    {
        Vector3 clickPos = -Vector3.zero;

        Plane plane = new Plane(Vector3.forward, 0f);

        Ray ray = Camera.main.ScreenPointToRay(StartPosition);
        float ditancToPlane;

        if (plane.Raycast(ray, out ditancToPlane))
        {
            clickPos = ray.GetPoint(ditancToPlane);
        }

        transform.LookAt(clickPos);


            
        Cannon.LookAt(clickPos);
        CalculateCannonRotation();


       
    }

    void CalculateCannonRotation()
    {
        Vector3 newPos = Cannon.localEulerAngles;

        if (180 > newPos.x)
        {
            if (newPos.x < 40)
            {
                newPos.x = 10;
            }
        }
        else
        {
            if (newPos.x > 310)
            {
                newPos.x = 350;
            }
        }

        Cannon.localEulerAngles = newPos;
    }

    PointerEventData pointerData;

    bool isDragging = false;
    Vector2 StartPosition;
    Vector2 CurrentPosition;
    Vector2 TotalDeltaPosition;
    Vector2 LastPosition;
    Vector2 DeltaPosition;

    public void TouchDrag(BaseEventData data)
    {
        pointerData = data as PointerEventData;

        isDragging = true;

        CurrentPosition = pointerData.position;

        TotalDeltaPosition = CurrentPosition - StartPosition;
        DeltaPosition = CurrentPosition - LastPosition;

        LastPosition = pointerData.position;        

    }


    public void TouchDown(BaseEventData data)
    {
        pointerData = data as PointerEventData;

        StartPosition = pointerData.position;
        LastPosition = StartPosition;

        if(!GameManager.GameStarted)
        {
            GameManager.self.GameStart();
        }
        
        LookAtPos(StartPosition);
        ShootPlayer();
    }



    public void TouchUp()
    {
        isDragging = false;
    }


}//
