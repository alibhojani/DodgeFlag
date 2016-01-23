using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public float speed = 5f; 
	public float force = 1600f;  

	Animator anim; 
	Rigidbody playerRigidbody; 

	public GameObject wrist; 
	public Camera mainCamera; 
	public Ball ball; 
	
	private bool startBallTimer = false;
	private float ballTimer = 0f; 
	
	private bool startPowerUpTimer = false; 
	private float powerUpTimer = 0f; 
	private float powerUpAmount = 5f;

	private Vector3 ballOffset; 
	private Vector3 cameraCentreVector; 
	private Vector3 p;

	/*---------------- Soon to be Networked Variables ---------------------*/
	Vector3 movementVertical;
	Vector3 movementHorizontal;

	private bool hasBall = true; 
	private bool isThrowing = false; 

	private bool isDead = false; 
	private bool pickUp = false; 

	PowerUp power = null;
	
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		playerRigidbody = GetComponent<Rigidbody> ();  		
		Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
		ballOffset = ball.transform.position - wrist.transform.position;					
	
	}
	
	void FixedUpdate () {
		//restart level
		if (Input.GetKey(KeyCode.L)) Application.LoadLevel (Application.loadedLevelName);

		p = mainCamera.ViewportToWorldPoint(new Vector3 (0.5f,0.5f, mainCamera.farClipPlane)); //midpoint of farplane
		cameraCentreVector = p - mainCamera.transform.position; //vector through center of camera to farplane
		
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");
		if (Input.GetKey (KeyCode.Mouse0)) { 
			if (hasBall){
				//isThrowing = true;
				Throw();  
		}
		}

		if (startBallTimer) { 
			ballTimer += Time.deltaTime;
			if (ballTimer >= 0.47f) {
				isThrowing = false; 
				startBallTimer = false;
				//ballTimer = 0f; 
			}
		}
		if (startPowerUpTimer) { 
			powerUpTimer += Time.deltaTime;
			if (powerUpTimer >= 5f) { 
				removePowerUp();
			}
		}
		//if (isDead) Die ();

		Animating (h,v); 
		if (!isThrowing){
			Turn (); 
			Move (h,v);
		}
		if (pickUp)ballToWrist(ball.transform, wrist.transform);
	}


	
	void Move (float h, float v) {
		if (v != 0 || h != 0){	
			movementVertical = transform.forward * v;
			movementHorizontal = transform.right * h; 
			movementVertical = movementVertical * speed * Time.deltaTime;
			movementHorizontal = movementHorizontal * speed * Time.deltaTime;
			playerRigidbody.MovePosition (transform.position + movementVertical + movementHorizontal); 
		}
	}

		//var v = cam.ViewportToWorldPoint(Vector3(0.5, 0.5, cam.nearClipPlane)); TODO: use this for aim mechanic (center of screen)

	void Throw () { 
			//isThrowing = true; 
			Vector3 cameraToBall = ball.transform.position - mainCamera.transform.position; //we need this vector to be established when throw is called
			Vector3 throwDir = cameraCentreVector - cameraToBall; //get throw direction
			throwDir.Normalize();
			ball.owner = null;
			ball.transform.SetParent(null);
			ball.gameObject.AddComponent<Rigidbody>();
			Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
			//Debug.Log(ballRigidbody.mass);
			ballRigidbody.AddForce(throwDir*force);
			hasBall = false; 
			startBallTimer = true;
		
	}
	
	void Jump () { 	
	 	//playerRigidbody.AddForce(transform.up * force);
	}	
		
	void Animating (float h, float v) { 
		bool b = v != 0 || h!=0;
		anim.SetBool ("IsMoving", b);
		anim.SetBool ("IsThrowing", isThrowing);
		
	}		

	void Turn () { //WORKS PERFECTLY
		
		if (Input.GetAxis ("Mouse X") != 0) { 
			
			Quaternion currentRot = transform.rotation; 
			Vector3 vectorFromRotation = currentRot * Vector3.forward; 
			vectorFromRotation = Quaternion.Euler (0f, Input.GetAxis ("Mouse X") * speed, 0f) * vectorFromRotation; 
			Quaternion newRotation = Quaternion.LookRotation (vectorFromRotation);
			playerRigidbody.MoveRotation (newRotation); 
		}
				
	}

	
	void OnTriggerEnter (Collider other) { //TODO: design patternofy powerup pickup
			
			if (power == null) { 
				if (other.tag.Equals ("PowerUp")) { 
					applyPowerUp(other);
					DestroyObject (other.gameObject);
					
				}
			}
			
		}
		
		void OnCollisionEnter (Collision other) { 

				if (!hasBall && ballTimer >= 0.1f) { 
				if (other.gameObject.tag.Equals("Ball")) { 
					
					Destroy (other.gameObject.GetComponent<Rigidbody>());
					//other.transform.position = wrist.transform.position + ballOffset;
					pickUp = true;
					other.transform.SetParent (wrist.transform);
					ball = other.gameObject.GetComponent<Ball>();
					ball.owner = this; 
					hasBall = true;
					ballTimer = 0f; 
					startBallTimer = false; 
				}
			}
		}
		
		void Die () { 
			//TODO: apply dying animation, disable and respawn player [better if done after networking
		}

		void ballToWrist (Transform ball, Transform wrist) { 
			
			ball.position = Vector3.MoveTowards(ball.position, wrist.transform.position + ballOffset, speed*Time.deltaTime/2f); 
			if (ball.position == wrist.transform.position + ballOffset) pickUp = false; //some thing cool happens if you comment this line out!
			
		}

		private void applyPowerUp (Collider powerUp) { 

				startPowerUpTimer = true;
				power = powerUp.gameObject.GetComponent<PowerUp>();
				if (power.type == PowerUpType.SpeedUp) { 
						this.speed *= powerUpAmount;
				}		
				else if (power.type == PowerUpType.PowerThrow) { 
						this.force *= powerUpAmount;
				}
		}

		private void removePowerUp () { 
				
				if (power.type == PowerUpType.SpeedUp) speed /= powerUpAmount;
				if (power.type == PowerUpType.PowerThrow) force /= powerUpAmount;
				startPowerUpTimer = false; 
				powerUpTimer = 0f;
				power = null;
		}
		/*void OnDrawGizmos() {

		Vector3 p = mainCamera.ViewportToWorldPoint(new Vector3 (0.5f,0.5f,mainCamera.nearClipPlane));
		Debug.DrawRay(p, offset);
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(p, 0.1f);

		}*/
		
}
	
