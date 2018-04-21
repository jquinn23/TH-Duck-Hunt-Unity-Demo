/**
 *  Property of Jonathan R. Quinn
 *  Created: April 18th 2018 
 **/

/**
 * This script is the manager for the duck hunt game. It handles
 * spawning ducks, keeping score, and increasing the difficulty as
 * the player progresses. 
 **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int numDucks = 0;
    private GameObject[] Spawners = new GameObject[8];
    public int numDucksKilled = 0;
    public GameObject prefab;
    private System.Random rnd = new System.Random();
    public Text scoreText;
    public Text timerText;
    public int timeLeft;
    public Text gameOverText;
    private int score;
    private bool difficultyUpdated;
    public bool gameRunning;

    //Different from gameRunning, this bool is only set upon the first run of the game in order to have a different message appear
    public bool gameBegun;
    public Text beginText;

    public int difficulty;

    // Use this for initialization
    private void Awake()
    {
        for(int j = 0; j < 8; j++)
        {
            Spawners[j] = GameObject.Find("Spawner" + j);
        }
        difficulty = 1;
        difficultyUpdated = false;
        timeLeft = 10;
        timerText.text = "Time Left: " + timeLeft;
        gameOverText.GetComponent<Text>().enabled = false;
        gameRunning = false;
        gameBegun = false;

    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = ("Score: " + score);
        if(gameRunning == true)
        {
            if (numDucks == 0)
            {
                Spawn();
            }
        }
        

        //Increases the difficulty once, every 10 kills
        if(score%10 == 0 && score!=0 && difficultyUpdated == false)
        {
            difficulty++;
            difficultyUpdated = true;
        }

        //Resets the game upon clicking after a game over
        if(Input.GetMouseButtonDown(0))
        {
            if(gameRunning == false)
            {
                gameRunning = true;
                difficulty = 1;
                gameOverText.GetComponent<Text>().enabled = false;
                score = 0;
                timeLeft = 10;
                timerText.GetComponent<Text>().enabled = true;
            }
            
            //Special case for first run of the game
            if(gameBegun == false)
            {
                beginText.GetComponent<Text>().enabled = false;
                gameBegun = true;

                //Will cause the timer to count down once per second, every second
                InvokeRepeating("CountDown", 1.0f, 1.0f);
            }
                
        }
        
    }

    //Randomly chooses a spawner and instantiates a duck at its position. Called when all ducks have been destroyed
    void Spawn()
    {
        timeLeft = 10;
        timerText.text = "Time Left: " + timeLeft;
        Instantiate(prefab, Spawners[rnd.Next(7)].transform.position, new Quaternion(0,0,0,0));
        numDucks++;
    }

    //Method called by the duck upon death
    public void DuckDeath()
    {
        numDucksKilled++;
        numDucks--;

        //Allows remaining ducks to die without adjusting score upon game over
        if(gameRunning == true)
        {
            score++;
        }
        
    }


    void CountDown()
    {
        timeLeft--;
        timerText.text = "Time Left: " + timeLeft;
        if (timeLeft == 0)
        {
            gameRunning = false;
            gameOverText.GetComponent<Text>().enabled = true;
            timerText.GetComponent<Text>().enabled = false;
        }
    }

}
