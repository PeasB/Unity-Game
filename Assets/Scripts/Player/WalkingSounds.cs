using UnityEngine;
using System.Collections;

public class WalkingSounds : MonoBehaviour {
	
	public AudioClip DirtWalking;
	public AudioClip GrassWalking;
	public AudioClip HardwoodWalking;
	public AudioClip TileWalking;


	private Collider2D CurrentSurface;
	private AudioSource AudioPlayer;
	private bool[] OnLayer = new bool[4];

	void Start()
	{
		AudioPlayer = this.GetComponent<AudioSource>();
	}


	void Update()
	{
		//Player is moving and Audio is not playing.
		if ((this.GetComponent<Rigidbody2D>().velocity.x != 0 || this.GetComponent<Rigidbody2D>().velocity.y != 0) && !AudioPlayer.isPlaying)	
			AudioPlayer.Play ();
		else if (this.GetComponent<Rigidbody2D>().velocity.x == 0 && this.GetComponent<Rigidbody2D>().velocity.y == 0 && AudioPlayer.isPlaying) //Player is not moving stop Audio.
			AudioPlayer.Stop ();

		//If Player is not on a surface must be grass.
		if (CurrentSurface == null)                                             
			AudioPlayer.clip = GrassWalking;
		
	}

	void OnTriggerEnter2D(Collider2D Other)
	{
		//Player enters new Surface.
		if (Other.tag != "Camera Bounds") {
		
			switch(Other.tag){
			
			case "Ground Dirt": //Dirt.
				AudioPlayer.clip = DirtWalking;
				//Set the Current Surface.
				CurrentSurface = Other;
				break;
			case "Ground Hardwood": //Hardwood.
				AudioPlayer.clip = HardwoodWalking;
				//Set the Current Surface.
				CurrentSurface = Other;
				break;
			case "Ground Tile": //tiled floor(Kitchen).
				AudioPlayer.clip = TileWalking;
				//Set the Current Surface.
				CurrentSurface = Other;
				break;
			
			}
		
		}
	}
		
	void OnTriggerExit2D(Collider2D Other)
	{
		//Leave the Surface.
		AudioPlayer.Stop();
		CurrentSurface = null;
	}








	



}
