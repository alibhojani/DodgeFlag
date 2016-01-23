using UnityEngine;
using System.Collections;

public enum PowerUpType { 
	SpeedUp,
	PowerThrow	
}
public class PowerUp : MonoBehaviour {
		
	public PowerUpType type;
	public float rotationSpeed = 10f; 
	
	
	// Use this for initialization
	void Start () {
		PowerUpType[] pArray = (PowerUpType[])System.Enum.GetValues(typeof(PowerUpType));
		int r = Random.Range(0, pArray.Length - 1);	
		type = pArray[r];
		//Debug.Log(type.ToString());
		}
	
	void Update () {
		//make powerup look special
		transform.Rotate(new Vector3 (rotationSpeed*Time.deltaTime, 0f, 0f));
		transform.Rotate(new Vector3 (0f, rotationSpeed*Time.deltaTime, 0f));
	}
}
