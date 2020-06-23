using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LevelConfigs
{
    public List<string> SceneNames;
}

public class LevelSettings
{
    public int currentLevel;
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] LevelConfigs[] defaultLevelConfigs;
    public LevelConfigs currentLevelConfigs;

    [SerializeField] string[] FirstSceneNames;
    [SerializeField] string[] LastSceneNames;



    [SerializeField] Text CurrentLevelNum;
    [SerializeField] Text NextLevelNum;
    [SerializeField] GameObject NextLevelNumRedbox;


    public static int currentLevel = 0;

    [SerializeField] Text LevelPassedText; 
    [SerializeField] Text LevelFailedText;

    [SerializeField] SliderController sliderController;


    int savedLevelNum = -1;

    public void InitLevel()
    {
        currentLevel = DataManager.GetLevelSettings().currentLevel;
        currentSceneIndex = 0;

        if(savedLevelNum != currentLevel){
            getLevelScenes();
            savedLevelNum = currentLevel;
        }
            

        sliderController.InitSider(currentLevelConfigs.SceneNames.Count);

        CurrentLevelNum.text = (currentLevel + 1).ToString();
        NextLevelNum.text = (currentLevel + 2).ToString();
        NextLevelNumRedbox.SetActive(false);
    }


    public void getLevelScenes()
    {
        if(currentLevel < defaultLevelConfigs.Length)
        {
            currentLevelConfigs = defaultLevelConfigs[currentLevel];
        }
        else
        {
            currentLevelConfigs = new LevelConfigs();
            currentLevelConfigs.SceneNames = new List<string>();

            List<int> availableIndexes = new List<int>();
            for (int i = 0; i < FirstSceneNames.Length; i++)
            {
                availableIndexes.Add(i);
            }

            for (int i = 0; i < 3; i++)
            {
                int j = Random.Range(0, availableIndexes.Count);
                int index = availableIndexes[j];
                availableIndexes.RemoveAt(j);

                currentLevelConfigs.SceneNames.Add(FirstSceneNames[index]);

            }

            availableIndexes.Clear();
            for (int i = 0; i < LastSceneNames.Length; i++)
            {
                availableIndexes.Add(i);
            }

            for (int i = 0; i < 3; i++)
            {
                int j = Random.Range(0, availableIndexes.Count);
                int index = availableIndexes[j];
                availableIndexes.RemoveAt(j);

                currentLevelConfigs.SceneNames.Add(LastSceneNames[index]);

            }



            //defaultLevelConfigs[(int)Random.Range(defaultLevelConfigs.Length*0.5f, defaultLevelConfigs.Length)];
        }
    }

    public int currentSceneIndex = 0;
    public void ScenePassed()
    {
       // GameManager.self.shooterController.DisableControl();

        currentSceneIndex += 1;
        sliderController.UpdateSlider(currentSceneIndex);

        StartCoroutine(ScenePassedAnimations());

        for (int i = 0; i < GameManager.self.shooterController.botControllers.Count; i++)
        {
            GameManager.self.shooterController.botControllers[i].DisableBot();
        }
    }

   
    IEnumerator ScenePassedAnimations()
    {
     


        GameManager.self.scoreManager.ShowMessage();

        if (currentSceneIndex == currentLevelConfigs.SceneNames.Count)
        {
            NextLevelNumRedbox.SetActive(true);
            yield return new WaitForSeconds(4f);//3
            GameManager.self.LevelPassed();
        }
        else
        {
            yield return new WaitForSeconds(3.5f);//2.5
            GameManager.self.sceneManager.LoadNewScene();
        }

    }

    public void LevelLost ()
    {
        LevelFailedText.text = "Level " + (currentLevel + 1) + " Failed";
    }

    public void LevelPassed()
    {
        
        LevelPassedText.text = "Level "+ (currentLevel+1) + " Passed";

       // currentLevel++;

       

        LevelSettings levelSettings = DataManager.GetLevelSettings();
        levelSettings.currentLevel = currentLevel;


        DataManager.SetLevelSettings(levelSettings);

        // LevelPassedText.text = currentLevel.ToString();
    }



}//
