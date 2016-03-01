using UnityEngine;
using System.Collections;


public class CharacterMove : MonoBehaviour {

    Rigidbody2D body;
    Animator anim;

    public bool playerTwo;

    public int jumpStrength;
    public int walkSpeed;

    //public float throwStrength;
    float currentStrength;
    public float throwMax;

    public float speedLimit;

    public int health;

    public GameObject bulletType;

    bool hasBullet;

    bool facingRight;

    bool jumping;
    bool flapped;
    bool standing;
    bool sliding;
    bool dead;

	// Use this for initialization
	void Start () {

        body = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponentInChildren<Animator>();
        jumping = false;
        flapped = false;
        sliding = false;
        standing = true;

        facingRight = true;

        dead = false;
	
	}
	
	// Update is called once per frame
	void Update() {

        //speed control

        //animation stuff
        anim.SetBool("Dead", dead);
        if (!dead)
        {
            anim.SetBool("Standing", standing);
            anim.SetBool("Sliding", sliding);
            //anim.SetBool("Falling", false);

            anim.SetBool("Jumping", jumping);
            anim.SetBool("Flapped", flapped);
            


            if (facingRight)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }

            //Controls for player one
            if (!playerTwo)
            {
                //Double jump
                if (Input.GetKeyDown(KeyCode.UpArrow) && jumping && !flapped)
                {
                    //double jumped - half strength of a normal jump
                    body.AddForce(new Vector2(0, 1) * (jumpStrength / 2));
                    sliding = false;
                    flapped = true;
                    //anim.SetBool("Flapped", true);
                }
                //normal jump
                if (Input.GetKeyDown(KeyCode.UpArrow) && !jumping)
                {
                    //Jump if they aren't already jumping
                    //anim.SetBool("Jumping", true);
                    body.AddForce(new Vector2(0, 1) * jumpStrength);
                    jumping = true;
                    standing = false;
                }

                if (Input.GetKey(KeyCode.RightArrow))
                {
                    //Move right
                    body.AddForce(new Vector2(1, 0) * walkSpeed);
                    facingRight = true;
                    sliding = true;
                    standing = false;
                    //anim.SetBool("Sliding", true);
                }

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    //Move left
                    body.AddForce(new Vector2(-1, 0) * walkSpeed);
                    facingRight = false;
                    sliding = true;
                    standing = false;
                    //anim.SetBool("Sliding", true);
                }

                if (Input.GetKey(KeyCode.Keypad0) && hasBullet)
                {
                    //Charge up your throw, to the maximum amount
                    if (currentStrength < throwMax)
                    {
                        currentStrength += 1f;
                    }

                }

                if (Input.GetKeyUp(KeyCode.Keypad0) && hasBullet)
                {
                    //shoot a snowball if we have one
                    bulletType.GetComponent<Snowball>().Throw(facingRight, currentStrength);
                    hasBullet = false;
                    Debug.Log("Threw with a strength of " + currentStrength.ToString());
                    //set the strength back to normal
                    currentStrength = 0;
                }
            }// end of player one controls
            if (playerTwo)
            {
                //Double jump
                if (Input.GetKeyDown(KeyCode.W) && jumping && !flapped)
                {
                    //double jumped - half strength of a normal jump
                    body.AddForce(new Vector2(0, 1) * (jumpStrength / 2));
                    sliding = false;
                    flapped = true;
                    //anim.SetBool("Flapped", true);
                }
                //normal jump
                if (Input.GetKeyDown(KeyCode.W) && !jumping)
                {
                    //Jump if they aren't already jumping
                    //anim.SetBool("Jumping", true);
                    body.AddForce(new Vector2(0, 1) * jumpStrength);
                    jumping = true;
                    standing = false;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    //Move right
                    body.AddForce(new Vector2(1, 0) * walkSpeed);
                    facingRight = true;
                    sliding = true;
                    standing = false;
                    //anim.SetBool("Sliding", true);
                }

                if (Input.GetKey(KeyCode.A))
                {
                    //Move left
                    body.AddForce(new Vector2(-1, 0) * walkSpeed);
                    facingRight = false;
                    sliding = true;
                    standing = false;
                    //anim.SetBool("Sliding", true);
                }

                if (Input.GetButton("Fire1") && hasBullet)
                {
                    //Charge up your throw, to the maximum amount
                    if (currentStrength < throwMax)
                    {
                        currentStrength += 1f;
                    }

                }

                if (Input.GetButtonUp("Fire1") && hasBullet)
                {
                    //shoot a snowball if we have one
                    bulletType.GetComponent<Snowball>().Throw(facingRight, currentStrength);
                    hasBullet = false;
                    Debug.Log("Threw with a strength of " + currentStrength.ToString());
                    //set the strength back to normal
                    currentStrength = 0;
                }
                if (Input.GetKey(KeyCode.X))
                {
                    Die();
                }
            }//end of player two controls
        }//end of death check
        
	}//update

    public bool PickUp(GameObject bullet)
    {
        //already have a bullet, don't pick up two
        if (!hasBullet)
        {
            hasBullet = true;
            bulletType = bullet;
            return true;
        }
        return false;
    }

    public void Hurt(float damage)
    {
        health -= System.Convert.ToInt32(damage);
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(this.gameObject.name + " died.");
        dead = true;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //hit the ground, stop jumping
        if(other.gameObject.tag == "Floor" && !dead)
        {
            jumping = false;
            flapped = false;
            sliding = false;
            standing = true;
        }

    }
    
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Floor" && !dead)
        {
            standing = true;
            flapped = false;
            jumping = false;
        }
    }
}
