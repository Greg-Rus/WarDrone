using UnityEngine;
using System.Collections;

public class EngineManager : MonoBehaviour {
	Rigidbody myRigidBody;
	//public float engineMaxPower;
	//public float engineMaxTorque;
	public float spinDirection;
	// Use this for initialization
	void Start () {
		myRigidBody = GetComponentInParent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void UpdateEngine(float liftPower, float torquePower)
	{
		myRigidBody.AddForceAtPosition(this.transform.up * liftPower, this.transform.position);
		//myRigidBody.AddTorque(this.transform.up * spinDirection * torquePower);
		Debug.DrawRay(this.transform.position, this.transform.up * liftPower);
	}
}
