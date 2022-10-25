using NWH.DWP2.WaterObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    float randomNum;



    //Screen image, status and etc.
    public Image startScreen;
    public Image nextLevelScreen;
    public Image scope;
    public Image gameOverScreen;
    public Image tutorialScreen;

    public TextMeshProUGUI torpedoCountText;
    [HideInInspector]
    public int torpedoCount;

    public TextMeshProUGUI targetsDestroyedText;
    [HideInInspector]
    public int targetsDestroyedCount;
    public TextMeshProUGUI reloadingText;

    //Sounds
    [HideInInspector]
    public AudioSource sonarCheck;
    public AudioSource explosionSoundBox;
    public AudioClip explosionSound;

    //Game phases

    public bool isGameActive;

    //Game objects scripts



    // Start is called before the first frame update
    void Start()
    {
        sonarCheck = GetComponent<AudioSource>();
        explosionSoundBox = GetComponent<AudioSource>();
        Screen.fullScreen = true;
    }

    // Update is called once per frame
    void Update()
    {
        TargetAndTorpedoInfoUpdate();
        LevelCompleteCheck();
        GameOverCheck();
        QuitFullScreen();
    }
    public void StartGame()
    {
        StartCoroutine(ShowTutorial());
        
        torpedoCount = 11;
        targetsDestroyedCount = 0;


        startScreen.gameObject.SetActive(false);
        scope.gameObject.SetActive(true);
        sonarCheck.PlayDelayed(10f);
        Cursor.visible = false;
    }
    void TargetAndTorpedoInfoUpdate()
    {
        if (isGameActive)
        {
            torpedoCountText.text = $"{torpedoCount}";
            targetsDestroyedText.text = $"{targetsDestroyedCount}";
        }

    }
    public void NextLevelSetup()
    {
        torpedoCount = 11;
        targetsDestroyedCount = 0;

        Target1Behavior.MaxSpeed += 1.2f;

        Target2Behavior.MaxSpeed += 1.2f;

        isGameActive = true;
        sonarCheck.Play();
        scope.gameObject.SetActive(true);
        nextLevelScreen.gameObject.SetActive(false);
        Cursor.visible = false;


    }
    void LevelCompleteCheck()
    {
        if (targetsDestroyedCount == 10)
        {
            isGameActive = false;
            sonarCheck.Stop();
            scope.gameObject.SetActive(false);
            nextLevelScreen.gameObject.SetActive(true);
            Cursor.visible = true;

        }
    }
    void GameOverCheck()
    {
        if (targetsDestroyedCount != 10 && torpedoCount == 0 && isGameActive && !GameObject.FindGameObjectWithTag("Torpedo"))
        {
            isGameActive =false;
            sonarCheck.Stop();
            scope.gameObject.SetActive(false);
            gameOverScreen.gameObject.SetActive(true);
            Cursor.visible = false;
        }
    }
    public void ReturnToMainMenu()
    {
        sonarCheck.Stop();
        gameOverScreen.gameObject.SetActive(false);
        startScreen.gameObject.SetActive(true);
    }
    public void QuitProgram()
    {
        Application.Quit();
    }

    private IEnumerator ShowTutorial()
    {
        tutorialScreen.gameObject.SetActive(true);

        yield return new WaitForSeconds(10);

        tutorialScreen.gameObject.SetActive(false);

        isGameActive = true;
    }
    void QuitFullScreen()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
    }


}
