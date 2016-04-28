using UnityEngine;
using System.Collections;

public class Snowball : MonoBehaviour {

    //if this snowball has been thrown it is 'active'
    //and will hurt a player that gets hit by it
    bool active;

    bool thrown;

    Rigidbody2D body;
    SpriteRenderer sprite;

    //this is the player that is carrying the snowball right now
    GameObject carrier;

	// Use this for initialization
	void Start () {

        body = gameObject.GetComponent<Rigidbody2D>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        active = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (carrier && !thrown)
        {
            //move along with the person that is carrying us
            transform.position = new Vector3(carrier.transform.position.x,carrier.transform.position.y,0);
            //body.MovePosition(new Vector2(carrier.transform.position.x, carrier.transform.position.y));
            /*
            float distanceToPlayerX = carrier.transform.position.x - transform.position.x;
            float distanceToPlayerY = carrier.transform.position.y - transform.position.y;
            body.AddForce(new Vector2(distanceToPlayerX,distanceToPlayerY));
            */
        }

        //I put this in here for beta testing so we can watch the snowball be active
        if (active)
        {
            sprite.color = Color.red;
        }
        else{
            sprite.color = Color.white;
        }
	
	}

    public void Throw(bool throwRight, float newVelocity)
    {
        //get thrown by the player
        if (throwRight)
        {
            body.AddForce(new Vector2(newVelocity * 10,0),ForceMode2D.Impulse);
        }
        else
        {
            body.AddForce(new Vector2(-1 * newVelocity * 10,0),ForceMode2D.Impulse);
        }
        gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
        active = true;
        thrown = true;
        //Debug.Log(this.gameObject.name+" has been thrown.");
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Floor")
        {
            //it hit the floor, so it is no longer active
            active = false;
            thrown = false;
            carrier = null;
            //Debug.Log(this.gameObject.name+" hit the ground.");
        }

        if (other.gameObject.tag == "Player")
        {
            //hit the player when we're active, hurt them
            //but don't hurt the person that threw it most recently
            if (active && other.gameObject != carrier)
            {
                float damage = body.velocity.magnitude;
                Debug.Log("Ouch! hit for "+damage.ToString()+" damage!");
                other.gameObject.GetComponent<CharacterMove>().Hurt(damage);
                thrown = false;
                carrier = null;
            }
            //snowball not active, so the player picks us up
            if(!active)
            {
                //check if the player already has a bullet in their hand
                if(other.gameObject.GetComponent<CharacterMove>().PickUp(this.gameObject))
                {
                    //if they do not, attach to them
                    gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
                    carrier = other.gameObject;
                    //Debug.Log(this.gameObject.name + " has been picked up by " + carrier.gameObject.name);
                }
            }

        }
        

    }
}
