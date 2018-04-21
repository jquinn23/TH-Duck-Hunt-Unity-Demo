/**
 * Property of Jonathan R. Quinn
 * Created: April 18th 2018 
 **/

 /**
  * This script is the basic handler for the duck in my basic duck hunt game
  * It allows the duck to move, plays a sound when it is clicked on, and
  * destroys the duck upon being clicked. 
  **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour {
    public AudioClip duckDeath;
    public GameObject GameManager;
    public AudioSource source;
    private Rigidbody2D duckRigidBody;
    private System.Random rnd = new System.Random();
    private Animator animation; 
    //Handles randomizing the direction upon collision
    int randomDirection;

    //0-7 specifies direction
    public int direction;

    public int speed;

	// Use this for initialization
	void Awake () {
        GameManager = GameObject.Find("GameManager");
        source = GameManager.GetComponent<AudioSource>();
        speed = 2+GameManager.GetComponent<GameController>().difficulty;
        duckRigidBody = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        /**
        Calls the ChangeDirection function after 1 seconds,
        and repeats every second.
        **/
        InvokeRepeating("ChangeDirection", 1.0f, 1.0f);

        //Only two possible initial directions
        int init = rnd.Next(2);
        if(init == 0)
        {
            direction = 1;
        }
        else if(init == 1)
        {
            direction = 7;
        }
	}
    

    /**
     *  Triggers when the user clicks on the duck
     *  destroys the duck, plays the sound effect,
     *  and lets the game manager know it's down a duck. 
     **/
    private void OnMouseDown()
    {
        source.PlayOneShot(duckDeath, 1);
        GameManager.GetComponent<GameController>().DuckDeath();
        Destroy(gameObject);
    }


    /**
     * This function moves the duck in accordance with it's direction variable
     *          0
     *      7+++++++1
     *   6++++++++++++++2
     *      5+++++++3
     *          4
     **/

    void Movement()
    {
        //The variable temp is used in order to control the x and y axis seperately
        if (direction == 0)
        {
            Vector3 temp = duckRigidBody.velocity;
            temp.y = speed;
            duckRigidBody.velocity = temp;
        }
        else if(direction == 1)
        {
            Vector3 temp = duckRigidBody.velocity;
            temp.y = speed;
            temp.x = speed;
            duckRigidBody.velocity = temp;
            animation.SetInteger("Direction", 0);
        }
        else if(direction == 2)
        {
            Vector3 temp = duckRigidBody.velocity;
            temp.x = speed;
            duckRigidBody.velocity = temp;
            animation.SetInteger("Direction", 0);
        }
        else if(direction == 3)
        {
            Vector3 temp = duckRigidBody.velocity;
            temp.y = -speed;
            temp.x = speed;
            duckRigidBody.velocity = temp;
            animation.SetInteger("Direction", 0);
        }
        else if(direction == 4)
        {
            Vector3 temp = duckRigidBody.velocity;
            temp.y = -speed;
            duckRigidBody.velocity = temp;
        }
        else if(direction == 5)
        {
            Vector3 temp = duckRigidBody.velocity;
            temp.y = -speed;
            temp.x = -speed;
            duckRigidBody.velocity = temp;
            animation.SetInteger("Direction", 1);
        }
        else if(direction == 6)
        {
            Vector3 temp = duckRigidBody.velocity;
            temp.x = -speed;
            duckRigidBody.velocity = temp;
            animation.SetInteger("Direction", 1);
        }
        else if(direction == 7)
        {
            Vector3 temp = duckRigidBody.velocity;
            temp.y = speed;
            temp.x = -speed;
            duckRigidBody.velocity = temp;
            animation.SetInteger("Direction", 1);
        }
        else
        {
            throw new System.Exception("Movement value out of bounds");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /**
         * There are only 3 directions per wall that the duck should bounce in upon collision,
         * so the computer creates a random number between 0 and 2 to decide that direction
         **/
        randomDirection = rnd.Next(0, 2);

        //Collision options for the top barrier
        //Possible Directions: 4,3,5
        if(collision.gameObject.tag == "Barrier Top")
        {
            if(randomDirection == 0)
            {
                direction = 4;
            }
            else if(randomDirection == 1)
            {
                direction = 3;
            }
            else if(randomDirection == 2)
            {
                direction = 5;
            }
        }

        //Collision options for the bottom barrier
        //Possible Directions: 0,7,1
        if(collision.gameObject.tag == "Barrier Bottom")
        {
            if (randomDirection == 0)
            {
                direction = 0;
            }
            else if (randomDirection == 1)
            {
                direction = 7;
            }
            else if (randomDirection == 2)
            {
                direction = 1;
            }
        }

        //Collision options for the right barrier
        //Possible Directions: 7,6,5
        if (collision.gameObject.tag == "Barrier Right")
        {
            if (randomDirection == 0)
            {
                direction = 7;
            }
            else if (randomDirection == 1)
            {
                direction = 6;
            }
            else if (randomDirection == 2)
            {
                direction = 5;
            }
        }

        //Collision options for the left barrier
        //Possible Directions: 1,2,3
        if (collision.gameObject.tag == "Barrier Left")
        {
            if (randomDirection == 0)
            {
                direction = 1;
            }
            else if (randomDirection == 1)
            {
                direction = 2;
            }
            else if (randomDirection == 2)
            {
                direction = 3;
            }
        }

    }

    /**
     *  In the interest of making the game more interesting,
     *  and in order to make the duck seem less like a pong
     *  ball and more like a creature, this method changes
     *  the duck's direction to a random direction, and will
     *  be called on an interval (see Awake method) 
     **/
    void ChangeDirection()
    {
        direction = rnd.Next(8);
    }

    // Update is called once per frame
    void Update () {
        Movement();
        if(GameManager.GetComponent<GameController>().gameRunning == false)
        {
            GameManager.GetComponent<GameController>().DuckDeath();
            Destroy(gameObject);
        }
	}
}
