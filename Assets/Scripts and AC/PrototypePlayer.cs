using UnityEngine;
using System.Collections;

public class PrototypePlayer : MonoBehaviour {
	
	public float speed = 5f;  
	public float force = 10f; 

	Animator anim; 
	Rigidbody playerRigidbody; 

	public Camera mainCamera; 
	private Vector3 cameraCentreVector; 
	private Vector3 p;
	Vector3 movementVertical;
	Vector3 movementHorizontal;

	
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		playerRigidbody = GetComponent<Rigidbody> ();  		
		Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;				
	
	}
	
	void FixedUpdate () {
		
		p = mainCamera.ViewportToWorldPoint(new Vector3 (0.5f,0.5f, mainCamera.farClipPlane));
		//cameraCentreVector = p - mainCamera.transform.position;

		//if (Input.GetKey(KeyCode.L)) Application.LoadLevel (Application.loadedLevelName);
		
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");
		Animating (h,v); 
		Turn (); 
		Move (h,v);
		Jump();
		
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

	
	
	void Jump () { 	
		if (Input.GetKey(KeyCode.Space)) { 
			playerRigidbody.AddForce(Vector3.up*force);
		}
	}	
		
	void Animating (float h, float v) { 
		bool b = v != 0 || h!=0;
		anim.SetBool ("IsMoving", b);	

	}		

		

	void Turn () {
		
		if (Input.GetAxis ("Mouse X") != 0) { 
			
			Quaternion currentRot = transform.rotation; 
			Vector3 vectorFromRotation = currentRot * Vector3.forward; 
			vectorFromRotation = Quaternion.Euler (0f, Input.GetAxis ("Mouse X") * speed, 0f) * vectorFromRotation; 
			Quaternion newRotation = Quaternion.LookRotation (vectorFromRotation);
			playerRigidbody.MoveRotation (newRotation); 
		}
				
	}

}
	
