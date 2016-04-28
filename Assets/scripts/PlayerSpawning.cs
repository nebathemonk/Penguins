using UnityEngine;
using System.Collections;

public class PlayerSpawning : MonoBehaviour {

	[SerializeField]
	bool respawn   = false;
	public GameObject prefab;
	public CharacterMove player;

	// Use this for initialization
	void Start () {
		//player = GameObject.Find ("Penguin").GetComponent<CharacterMove>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(player.health <= 0)
		{
			respawn = true;
		}
	else
	{
		respawn = false;
	}
	 
	if(respawn)
	{
			Destroy (player.gameObject);
			GameObject newPlayer = Instantiate (prefab, new Vector3 (transform.position.x,transform.position.y,0), Quaternion.identity) as GameObject;
			player = newPlayer.GetComponent<CharacterMove>();
			player.health = 20;
					
	}	
	
	}
}
	