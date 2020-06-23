using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControler : MonoBehaviour
{

    [SerializeField] public GameObject[] stickmans;
    [SerializeField] public int ballsAmount;
    public bool JumperScene;
    
    void Start()
    {
        InitScene();
    }

    [SerializeField] float MinY;

    public void InitScene()
    {
        transform.position = new Vector3(0, MinY, 0);

        GameManager.self.sceneManager.SceneLoaded(this);

        ShowScene();
    }

    public void HideScene()
    {
        if (SceneMovementRoutineC != null) StopCoroutine(SceneMovementRoutineC);

        SceneMovementRoutineC = StartCoroutine(SceneMovementRoutine(MinY, HideSceneFinished));
    }

    public void ShowScene()
    {

        if (SceneMovementRoutineC != null) StopCoroutine(SceneMovementRoutineC);

        SceneMovementRoutineC = StartCoroutine(SceneMovementRoutine(0, ShowSceneFinished));

    }

    void HideSceneFinished()
    {
        GameManager.self.sceneManager.SceneHideFinished();
    }

    void ShowSceneFinished()
    {
        GameManager.self.sceneManager.SceneShowFinished();
    }

    delegate void SceneMovementFinished();
    private SceneMovementFinished sceneMovementFinishedCallback;

    Coroutine SceneMovementRoutineC;
    IEnumerator SceneMovementRoutine(float endY, SceneMovementFinished callback)
    {

        float startT = Time.time;
        float duration = 0.2f;
        float t = 0;

        Vector3 startPos = transform.position;

        while(t<1)
        {
            t = (Time.time - startT) / duration;

            transform.position = Vector3.Lerp(startPos, new Vector3(0, endY, 0), t);

            yield return new WaitForEndOfFrame();
        }

        callback();

    }

}//
