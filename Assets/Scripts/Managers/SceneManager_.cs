using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneManager_ : MonoBehaviour
{
    [SerializeField] GameObject floor;
    [SerializeField] BoxCollider floorCollider;
    
    public SceneControler currentSceneControler;
    
    [SerializeField] string[] environmentNames;

    
    public void InitScene()
    {
        EnableDisableFloorCollider(false);
        initNewScene();
        //initEnvironment();

    }

    string LoadedSceneName;
    

    [SerializeField] Text levelName;
    public void initNewScene()
    {
        GameManager.self.DetelePreviousScene();

        LoadedSceneName = GameManager.self.levelManager.currentLevelConfigs.SceneNames[GameManager.self.levelManager.currentSceneIndex];
         SceneManager.LoadSceneAsync(LoadedSceneName, LoadSceneMode.Additive);
       
       
        levelName.text = LoadedSceneName;
    }

    public void LoadNewScene()
    {
        EnableDisableFloorCollider(false);
        currentSceneControler.HideScene();
    }

    public void SceneHideFinished()
    {
        initNewScene();
    }

    public void SceneShowFinished()
    {
        EnableDisableFloorCollider(true);
        GameManager.self.shooterController.EnableControl();
        GameManager.self.shooterController.botControllers.Clear();
    }

    
    public void initEnvironment()
    {

        string name = environmentNames[Random.Range(0, environmentNames.Length)];
        SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
       
    }

    public void SceneLoaded(SceneControler newsceneControler)
    {
     
      SceneManager.SetActiveScene(SceneManager.GetSceneByName(LoadedSceneName));
       SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));

        currentSceneControler = newsceneControler;

        GameManager.self.scoreManager.StickmanAmount(currentSceneControler.stickmans.Length, currentSceneControler.ballsAmount);
        
        EnableDisableFloor(!currentSceneControler.JumperScene);
    }
    
    public void EnableDisableFloor(bool enable)
    {
        floor.SetActive(enable);
    }

    public void EnableDisableFloorCollider(bool enable)
    {
        floorCollider.enabled = enable;
    }


}//
