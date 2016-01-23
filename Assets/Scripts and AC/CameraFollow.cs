using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	Vector3 offset; 
	public GameObject player; 
	public float smoothing = 0.15f; 

	public float snstvty = 20f; 
	public float cameraSnstvty = 10f;
	private float yRot = 0f;
	private float maxYRot = 65f; // or -45f
	float speed = 5f;

	Vector3 currentPlayerRot; //track player rotation at all times
	
	void Start () {
		offset = transform.position - player.transform.position;
		currentPlayerRot = player.transform.rotation * Vector3.forward; //initial player rotation
	}



	void FixedUpdate () {
		Vector3 newPlayerRot = player.transform.rotation * Vector3.forward; //get new rotation on update
		Quaternion change = Quaternion.FromToRotation(currentPlayerRot, newPlayerRot); //calculate difference
		currentPlayerRot = newPlayerRot; //update "current" rotation 

		offset =  Quaternion.AngleAxis(change.eulerAngles.y, Vector3.up) * offset; //transform offset by change in rotation about the Y axis
       	transform.position = player.transform.position + offset; //apply offset
        transform.RotateAround(transform.position, Vector3.up, change.eulerAngles.y); //rotate camera along its y axis
		cameraXLook();
		
	}


	void cameraXLook () { 
		//Debug.Log (yRot);
		if (Input.GetAxis("Mouse Y") < 0) { 
			if (yRot < maxYRot/2f ) { 
				yRot -= Input.GetAxis ("Mouse Y") * speed;
				transform.Rotate(new Vector3 (-Input.GetAxis("Mouse Y") * speed, 0f, 0f));
			}
		}
		else if (Input.GetAxis("Mouse Y") > 0) { 
			if (yRot > -maxYRot/2f ) { 
				yRot -= Input.GetAxis ("Mouse Y") *speed;
				transform.Rotate(new Vector3 (-Input.GetAxis("Mouse Y") * speed, 0f, 0f));
			}
		}
	}

		

}
