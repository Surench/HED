using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AiController : MonoBehaviour
{

    [SerializeField] bool inLeftSide;
    [SerializeField] bool Idle;
    [SerializeField] bool jumper;
    [SerializeField] bool switchParent;
    [SerializeField] bool runToBomb;

    [SerializeField] AnimationCurve jumpAnimationCurve;

    [SerializeField] float StartDistance = 2f;
    [SerializeField] float EndDistance = 2f;
    [SerializeField] float length = 4f;
    [SerializeField] float MaxYtoJump = 4.1f;
    [SerializeField] float MinYtoJump = 1.7f;
    [SerializeField] int animIndex = 0;

    [SerializeField] Animator anim;
    [SerializeField] CapsuleCollider MaincapsuleCollider;
    [SerializeField] SkinnedMeshRenderer selfMaterial;
    [SerializeField] PiramidaController piramidaController;


    [SerializeField] public Rigidbody[] rigidbodies;

    [SerializeField] GameObject HitEffects;



    void Start()
    {
        AiMovment();
    }


    void AiMovment()
    {

        anim.Play(GameManager.self.StringToHashes[animIndex]);

        if (!Idle && !jumper)
            MovmentC = StartCoroutine(AiMovmentRoutine(inLeftSide));

        if (jumper)
            JumpC = StartCoroutine(JumpAnimationRountine());

        if (runToBomb)
            RunToBombC = StartCoroutine(RunToBombRountine());
    }

   
    [SerializeField] Transform bombPos;
    [SerializeField] Transform StartPos;
    [SerializeField] Transform Parent;

    private Coroutine RunToBombC;
    IEnumerator RunToBombRountine()
    {

        yield return new WaitForSeconds(0.7f);

        float StartTime = Time.time;
        float duration = 1.5f;
        float t=0;

        bool facingToBomb = true;

        Vector3 endPos = bombPos.position;
        Vector3 startpos = StartPos.position;

         endPos = StartPos.position + ((endPos - StartPos.position) * 0.75f);

         endPos.y = StartPos.position.y;

        //StartPos.eulerAngles = new Vector3(0, 180, 0);
        while (true)
        {
            t = (Time.time - StartTime) / duration;


            if (t > 1f)
            {
                facingToBomb = !facingToBomb;

                

                t = 0;
                StartTime = Time.time;


                StartCoroutine(RotateChar(facingToBomb));

            }


            if (facingToBomb)
            {
                transform.position = Vector3.Lerp(startpos, endPos, t);
            }
            else
            {
                transform.position = Vector3.Lerp( endPos, startpos, t);

            }
                
                   

            yield return new WaitForEndOfFrame();
            
        }


        
    }


    IEnumerator RotateChar(bool facingToBomb)
    {

        float StartTime = Time.time;
        float duration = 0.6f;
        float t = 0;

        while(t<1)
        {

            t = (Time.time - StartTime) / duration;

            if (facingToBomb)
                StartPos.localEulerAngles = Vector3.Lerp(StartPos.localEulerAngles, new Vector3(0, 180, 0), t);
            else
                StartPos.localEulerAngles = Vector3.Lerp(StartPos.localEulerAngles, new Vector3(0, 0, 0), t);


            yield return new WaitForEndOfFrame();


        }

    }



    bool lookingLeft;

    private Coroutine JumpC;
    IEnumerator JumpAnimationRountine ()
    {
        float StartTime = Time.time;    
        float t = 0;

        bool flipped = false;

        anim.Play(GameManager.self.StringToHashes[40], -1, 0);

        while (true)
        {
            t = (Time.time - StartTime) / 1.8f;
            

            if (t>1)
            {
                yield return new WaitForSeconds(1f);
                t = 0;
                StartTime = Time.time;
                flipped = false;
                anim.Play(GameManager.self.StringToHashes[40], -1, 0);
            }

            if (t > 0.08f && !flipped)
            {
                flipped = true;
                anim.SetTrigger("Flip");
            }

            float Ypos= jumpAnimationCurve.Evaluate(t) * MaxYtoJump;
            
            Ypos -= MinYtoJump;

            

            transform.position = new Vector3(transform.position.x, Ypos, transform.position.z);

            yield return new WaitForFixedUpdate();
        }

    }


    private Coroutine MovmentC;
    IEnumerator AiMovmentRoutine (bool Left)
    {
      
        lookingLeft = Left;

        if (lookingLeft)
            transform.rotation = Quaternion.Euler(0, -90, 0);
        else
            transform.rotation = Quaternion.Euler(0, 90, 0);

        while (true)
        {
            if (Left)
                transform.position = new Vector3(Mathf.Lerp(StartDistance, EndDistance, Mathf.PingPong(Time.time /length, 1)), transform.position.y, transform.position.z);
            else
                transform.position = new Vector3(Mathf.Lerp(EndDistance, StartDistance, Mathf.PingPong(Time.time /length, 1)), transform.position.y, transform.position.z);

            
            if (transform.position.x > EndDistance * 0.9)
            {
                transform.rotation = Quaternion.Euler(0, -90, 0);
                
            }

            if (transform.position.x < StartDistance * 0.9)
            {
                transform.rotation = Quaternion.Euler(0, 90, 0);               
            }
            
            yield return new WaitForEndOfFrame();
        }      

    }

   

    private void OnTriggerEnter(Collider other)
    {
    
        if (other.tag.Equals(GameManager.BotTag))
        {
            BotEnteredArea();
        }
    }
    

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals(GameManager.BotTag))
        {
            BotExitedArea();
        }
    }

    int BotBodyPartsAmountInArea = 0;

    void BotEnteredArea()
    {
        if (AIisStanding)
        {
            if(BotBodyPartsAmountInArea == 0)
                DisableKinematic(false);

            BotBodyPartsAmountInArea += 1;
        }
    }

    void BotExitedArea()
    {
        if(AIisStanding)
        {
            BotBodyPartsAmountInArea -= 1;

            if(BotBodyPartsAmountInArea == 0)
                DisableKinematic(true);
        }
    }

    public bool AIisStanding = true;

    public void Triggered(ContactPoint[] contactPoints, Collision collision)
    {

        if (!AIisStanding) return;
        AIisStanding = false;
       

        MaincapsuleCollider.enabled = false;
        DisableKinematic(false);

        if (MovmentC != null)
            StopCoroutine(MovmentC);
        if (RunToBombC != null)
            StopCoroutine(RunToBombC);

        anim.enabled = false;
        
        selfMaterial.material = GameManager.self.BotMaterial;

        if (collision.gameObject.name.Equals(GameManager.HeadName))
            GameManager.self.scoreManager.AddScore(1);
        else
            GameManager.self.scoreManager.AddScore(1);



#if UNITY_IOS
        if (GameManager.TapticEnabled) TapticEngine.TriggerLight();
#endif

        if (piramidaController != null)
        {
            piramidaController.DisableAboveRagdoll();
        }

        HitEffects.SetActive(true);
        HitEffects.transform.position = contactPoints[0].point;

        if (switchParent)
            SwitchTransformParent();
    }

    public void FallDown()
    {
        if (!AIisStanding) return;        
        AIisStanding = false;

        MaincapsuleCollider.enabled = false;

        DisableKinematic(false);


        anim.enabled = false;

        AddRandomRotation();

        selfMaterial.material = GameManager.self.BotMaterial;

        GameManager.self.scoreManager.AddScore(1);

#if UNITY_IOS
        if (GameManager.TapticEnabled) TapticEngine.TriggerLight();
#endif
        if (piramidaController != null)
        {
            piramidaController.DisableAboveRagdoll();
        }

        if (RunToBombC != null)
            StopCoroutine(RunToBombC);

        if (switchParent)
            SwitchTransformParent();

        
    }

    void AddRandomRotation()
    {
        rigidbodies[0].maxAngularVelocity = 150;

        float randomX = Random.Range(-200, 200);
        float randomY = Random.Range(-200, 200);
        
        float randomZ = Random.Range(-200, 200);

        rigidbodies[0].AddTorque(randomX, randomY, randomZ, ForceMode.Impulse);       
    }


    public void DisableKinematic(bool isKinematic)
    {
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = isKinematic;            
        }

       
        

        AddRandomRotation();
    }

    public void SwitchTransformParent()
    {       
        transform.parent = GameManager.self.sceneManager.currentSceneControler.transform;
    }

}//
