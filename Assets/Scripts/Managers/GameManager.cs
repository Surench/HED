using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Facebook.Unity;
using GameAnalyticsSDK;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager self;

    public Material BotMaterial;

    public static readonly string AiTag = "Ai"; 
    public static readonly string BotTag = "Bot";
    public static readonly string EnvTag = "Environment";
    public static readonly string FloorTag = "Floor";
    public static readonly string RotationArenaTag = "RotationArena";
    public static readonly string SceneTag = "Scene";
    public static readonly string HoleTag = "Hole";
    public static readonly string HeadName = "Head";

    public ShooterController shooterController;
    public SceneManager_ sceneManager;
    public DataManager dataManager;
    public ColorManager colorManager;
    public LevelManager levelManager;
    public ScoreManager scoreManager; 

    [SerializeField] GameObject MenuPanel;
    [SerializeField] GameObject GamePanel;
    [SerializeField] GameObject LvlPassedPanel;
    [SerializeField] GameObject LvlLostPanel;

    [SerializeField] GameObject LevelPassedConfetti;

    public static bool TapticEnabled = true;
    public static bool SoundEnabled = true;
    
    public static bool GameStarted;
    
    [SerializeField] string[] AnimNames;
    [SerializeField] public List<int> StringToHashes;


    private void Awake()
    {
        self = this;
        Application.targetFrameRate = 60;
        AddNames();

        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

   

    void Start()
    {
        GameAnalytics.Initialize();
        OpenMenu();
        AddNames();
        
    }

   

    public void OpenMenu()
    {

        InitGame();

        GameStarted = false;

        MenuPanel.SetActive(false);
        GamePanel.SetActive(true);
        LvlPassedPanel.SetActive(false);
        LvlLostPanel.SetActive(false);
        TutorialContainer.SetActive(false);

        LevelPassedConfetti.SetActive(false);
    }
    

    private void InitGame()
    {
        isTutorialStarted = false;

        DetelePreviousScene();

        levelManager.InitLevel();
        sceneManager.InitScene();
        scoreManager.InitScore();

    }
    

    public void GameStart()
    {
        GameStarted = true;
        MenuPanel.SetActive(false);
        GamePanel.SetActive(true);
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "game");

       // if(LevelManager.currentLevel == 0) startTutorial();
    }


    public void LevelPassed()
    {
        if (!GameStarted) return;

        GameStarted = false;
        GamePanel.SetActive(false);
        LvlPassedPanel.SetActive(true);

        levelManager.LevelPassed();

       // LevelPassedConfetti.SetActive(true);

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level", (LevelManager.currentLevel).ToString());
    }

    public void LastStickmanStopped()
    {
        if(GameStarted)
        {
            LevelLost();
        }
    }


    public void LevelLost()
    {

        LeveLostRoutineC = StartCoroutine(LeveLostRoutine());


    }
    [SerializeField] GameObject TapToRestart;
    [SerializeField] GameObject AoutOFAmmo;

    Coroutine LeveLostRoutineC;
    IEnumerator LeveLostRoutine()
    {

        AoutOFAmmo.SetActive(true);
        TapToRestart.SetActive(false);
        
        GameStarted = false;

        GamePanel.SetActive(false);
        LvlLostPanel.SetActive(true);

        levelManager.LevelLost();


        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Level", (LevelManager.currentLevel + 1));

        yield return new WaitForSeconds(1.2f);
        TapToRestart.SetActive(true);
        AoutOFAmmo.SetActive(false);
    }


    public void DetelePreviousScene()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag(SceneTag);
        for (int i = 0; i < obj.Length; i++)
        {
            Destroy(obj[i]);
        }

        obj = GameObject.FindGameObjectsWithTag(BotTag);
        for (int i = 0; i < obj.Length; i++)
        {
            Destroy(obj[i]);
        }

        /*
        obj = GameObject.FindGameObjectsWithTag(EnvTag);
        for (int i = 0; i < obj.Length; i++)
        {
            Destroy(obj[i]);
        }
        */
    }


    public void ToggleSound()
    {
        SoundEnabled = !SoundEnabled;
       
    }

    [SerializeField] GameObject linesImg;
    public void ToggleTaptic()
    {
        TapticEnabled = !TapticEnabled;
        linesImg.SetActive(TapticEnabled);
    }


    public void SkipLevle()
    {
        levelManager.LevelPassed();
    }


    public void ShopButton()
    {
        ShowComingSoon();
    }

   
    [SerializeField] Animator ShopAnimator;
    public void ShowComingSoon()
    {
        ShopAnimator.Play("ComingSoon", -1, 0);
    }

    void AddNames()
    {
        for (int i = 0; i < AnimNames.Length; i++)
        {
            StringToHashes.Add(Animator.StringToHash(AnimNames[i]));
        }
    }

    public bool isTutorialStarted = false;

    public void startTutorial()
    {
        if (TutorialStartDelayC != null) StopCoroutine(TutorialStartDelayC);

        isTutorialStarted = true;
        shooterController.DisableControl();

        TutorialStartDelayC =  StartCoroutine(TutorialStartDelay());
    }
    

    [SerializeField] GameObject TutorialContainer;
    [SerializeField] RectTransform TutorialContainerRect;

    Coroutine TutorialStartDelayC;
    IEnumerator TutorialStartDelay()
    {
        yield return new WaitForSeconds(1);

        TutorialContainer.SetActive(true);

        int standingStickmanIndex = 0;
        for (int i = 0; i < sceneManager.currentSceneControler.stickmans.Length; i++)
        {
            if (sceneManager.currentSceneControler.stickmans[i].GetComponent<AiController>().AIisStanding)
            {
                standingStickmanIndex = i;
                break;
            }
        }

        

        TutorialContainerRect.anchoredPosition = new Vector2(-123f + (standingStickmanIndex * 123), TutorialContainerRect.anchoredPosition.y);
    }

    public void TutorialShootBtn(BaseEventData data)
    {
        isTutorialStarted = false;
        shooterController.EnableControl();
        TutorialContainer.SetActive(false);
        shooterController.TouchDown(data);
    }
}//
