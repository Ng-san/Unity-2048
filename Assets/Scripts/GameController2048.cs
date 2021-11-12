using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController2048 : MonoBehaviour
{
    public static GameController2048 instance;
    public static int ticker;

    [SerializeField] GameObject fillPrefab;
    //[SerializeField] GameObject fillPrefabs;

    [SerializeField] Cell2048[] allCells;


    public static Action<string> slide;
    public int myScore;
    [SerializeField] Text scoreDisplay;
    [SerializeField] Text highScore;


    int isGameOver;
    [SerializeField] GameObject gameOverPanel;

    public Color[] fillColors;
    
    [SerializeField] int winningScore;
    [SerializeField] GameObject winningPanel;
    bool hasWon;

    private Vector2 startPos;
    public int pixelDistToDetect = 20;
    private bool fingerDown;

    [SerializeField] GameObject Grid1;
    [SerializeField] GameObject Grid2;
    [SerializeField] GameObject OptionMenu;
    [SerializeField] GameObject Gameobject3x3;
    [SerializeField] GameObject Gameobject4x4;
    
    private void OnEnable()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        StartSpawnFill();
        StartSpawnFill();
        highScore.text = PlayerPrefs.GetInt("HighScore",0).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SpawnFill();
        }
        
        if(fingerDown == false && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            startPos = Input.touches[0].position;
            fingerDown = true;
        }
        #if UNITY_ANDROID||UNITY_IOS
        if(fingerDown)
        {
            if(Input.touches[0].position.y >= startPos.y + pixelDistToDetect)
            {
                fingerDown = false;
                ticker = 0;
                isGameOver = 0;
                slide("w");
            }
            else if(Input.touches[0].position.y <= startPos.y - pixelDistToDetect)
            {
                fingerDown = false;
                ticker = 0;
                isGameOver = 0;
                slide("s");

            }
            else if(Input.touches[0].position.x <= startPos.x - pixelDistToDetect)
            {
                fingerDown = false;
                ticker = 0;
                isGameOver = 0;
                slide("a");
            }
            else if(Input.touches[0].position.x >= startPos.x + pixelDistToDetect)
            {
                fingerDown = false;
                ticker = 0;
                isGameOver = 0;
                slide("d");
            }
        }
        if(fingerDown && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)
        {
            fingerDown = false;
        }
        
        //Testing for PC
        
        #elif UNITY_EDITOR
        
        if(fingerDown == false && Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            fingerDown = true;
        }
        if(fingerDown)
        {
            if(Input.mousePosition.y >= startPos.y + pixelDistToDetect)
            {
                fingerDown = false;
                ticker = 0;
                isGameOver = 0;
                slide("w");
            }
            else if(Input.mousePosition.y <= startPos.y - pixelDistToDetect)
            {
                fingerDown = false;
                ticker = 0;
                isGameOver = 0;
                slide("s");
            }
            else if(Input.mousePosition.x <= startPos.x - pixelDistToDetect)
            {
                fingerDown = false;
                ticker = 0;
                isGameOver = 0;
                slide("a");
            }
            if(Input.mousePosition.x >= startPos.x + pixelDistToDetect)
            {
                fingerDown = false;
                ticker = 0;
                isGameOver = 0;
                slide("d");
            }
        }
        if(fingerDown && Input.GetMouseButtonUp(0))
        {
            fingerDown = false;
        }
        #endif
    }

    public void Menu()
    {
        OptionMenu.SetActive(true);
    }

    //public RectTransform rectTranform;
    public void Option3x3()
    {
        Grid1.SetActive(true);
        /*rectTranform = (RectTransform) transform.GetComponent<RectTransform>();
        if( rectTranform != null)
        {
            rectTranform.sizeDelta = new Vector2(280,280);
        }
        else
        {
            return;
        }
        */
        Grid2.SetActive(false);
        OptionMenu.SetActive(false);
        

    }
    public void Option4x4()
    {
        Grid1.SetActive(false);
        Grid2.SetActive(true);


        /*rectTranform = (RectTransform) transform.GetComponent<RectTransform>();
        if( rectTranform != null)
        {
            rectTranform.sizeDelta = new Vector2(200,200);
        }
        else
        {
            return;
        }
        */
        OptionMenu.SetActive(false);
        SceneManager.LoadScene(0);

    }
    public void SpawnFill()
    {
        bool isFull = true;
        for(int i =0 ; i < allCells.Length ; i++)
        {
            if(allCells[i].fill == null)
            {
                isFull = false;
            }
        }
        if(isFull ==true)
        {
            return;
        }

        int whichSpawn = UnityEngine.Random.Range(0, allCells.Length);
        if(allCells[whichSpawn].transform.childCount != 0)
        {
            Debug.Log(allCells[whichSpawn].name + " is already filled");
            SpawnFill();
            return;
        }
        float chance = UnityEngine.Random.Range(0f , 1f);
        Debug.Log(chance);
        /*if(chance < .2f)
        {
            return;
        }
        else */
        if( chance < .8f)
        {
            GameObject tempFill = Instantiate(fillPrefab, allCells[whichSpawn].transform);
            Debug.Log(2);
            Fill2048 tempFillComp = tempFill.GetComponent<Fill2048>();
            allCells[whichSpawn].GetComponent<Cell2048>().fill = tempFillComp;
            tempFillComp.FillValueUpdate(2);
        }
        else 
        {
            GameObject tempFill = Instantiate(fillPrefab, allCells[whichSpawn].transform);
            Debug.Log(4);
            Fill2048 tempFillComp = tempFill.GetComponent<Fill2048>();
            allCells[whichSpawn].GetComponent<Cell2048>().fill = tempFillComp;
            tempFillComp.FillValueUpdate(4);
        }
    }

    public void StartSpawnFill()
    {
        int whichSpawn = UnityEngine.Random.Range(0, allCells.Length);
        if(allCells[whichSpawn].transform.childCount != 0)
        {
            Debug.Log(allCells[whichSpawn].name + " is already filled");
            SpawnFill();
            return;
        }
        
            GameObject tempFill = Instantiate(fillPrefab, allCells[whichSpawn].transform);
            Debug.Log(2);
            Fill2048 tempFillComp = tempFill.GetComponent<Fill2048>();
            allCells[whichSpawn].GetComponent<Cell2048>().fill = tempFillComp;
            tempFillComp.FillValueUpdate(2);
        
    }
    public void ScoreUpdate(int scoreIn)
    {
        myScore += scoreIn;
        scoreDisplay.text = myScore.ToString();

        if(myScore > PlayerPrefs.GetInt("HighScore",0))
        {
            PlayerPrefs.SetInt("HighScore",myScore);
            highScore.text = myScore.ToString();
        }
    }

    public void GameOverCheck()
    {
        isGameOver++;
        if(isGameOver >=16)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
           
    }

    

    public void WinningCheck(int highestFill)
    {
        if(hasWon)
            return;
        if(highestFill == winningScore)
        {
            winningPanel.SetActive(true);
            hasWon = true;
        }
    }

    public void KeepPlaying()
    {
        winningPanel.SetActive(false);
    }
}
