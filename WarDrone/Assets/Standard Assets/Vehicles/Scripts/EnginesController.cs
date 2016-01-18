using UnityEngine;
using System.Collections;

public class EnginesController : MonoBehaviour {
	
	public EngineManager FR;
	public EngineManager FL;
	public EngineManager BR;
	public EngineManager BL;
	public float liftPower;
	public float torquePower;
	
	public float powerInput;
	public float yawInput;
	public float rollInput;
	public float pitchInput;
	
	public float powerFactor;
	public float pitchFactor;
	public float rollFactor;
	public float yawFactor;
	public bool globalYaw;
	
	public Vector3 desiredVector;
	public Vector3 currentAngularVelocity;
	public float angularVelocityMagnitude;
	public Vector3 compensationVector;
	public float compensationMagintude;
	
	public float angularCorrection;
	//public float pitchCorrection = 2f;
	//public float rollCorrention = 2f;
	
	private float[,] enginePowerDistribution;
	private Rigidbody myRigidbody;
	
	// Use this for initialization
	void Awake () {
	
		myRigidbody = GetComponentInParent<Rigidbody>();
		enginePowerDistribution = new float[2,2];
		desiredVector = Vector3.zero;
		currentAngularVelocity = Vector3.zero;
		compensationVector = Vector3.zero;
		
		
	}
	
	// Update is called once per frame
	void Update () {
		yawInput = Input.GetAxis("LeftStickX");
		powerInput =  Input.GetAxis("LeftStickY");
		rollInput = Input.GetAxis("RightStickX");
		pitchInput = Input.GetAxis("RightStickY");
		
		
		desiredVector.x = rollInput;
		desiredVector.y = powerInput;
		desiredVector.z = pitchInput;
		
		currentAngularVelocity = myRigidbody.angularVelocity;
		angularVelocityMagnitude = currentAngularVelocity.magnitude;
		
		UpdateEngines();
	}
	
	private void UpdateEngines()
	{
		UpdateHoverValues();
		UpdatePower(desiredVector);
		UpdateYaw();
		
		if(desiredVector.z != 0 || desiredVector.x != 0)
		{
			UpdatePitchValues(desiredVector);
			UpdateRollValues(desiredVector);
		}
		else
		{
			CompensatePitchValues();
		}
		
		
		
		PropagatePowerDistributionToEngines();
		//CalculatePowerDistribution();
		//PropagatePowerDistributionToEngines();
		
	}
	
	private void UpdateYaw()
	{
		if(globalYaw)
		{
			myRigidbody.AddTorque(Vector3.up * yawInput * liftPower * yawFactor);
		}
		else
		{
			myRigidbody.AddTorque(this.transform.up * yawInput * liftPower * yawFactor);
		}
		
	}
	
	private void UpdatePitchValues(Vector3 direction)
	{
		enginePowerDistribution[0,0] += liftPower * direction.z * -1f * pitchFactor;
		enginePowerDistribution[0,1] += liftPower * direction.z * -1f * pitchFactor;
		enginePowerDistribution[1,0] += liftPower * direction.z * pitchFactor;
		enginePowerDistribution[1,1] += liftPower * direction.z * pitchFactor;
	}
	
	private void UpdateRollValues(Vector3 direction)
	{
		enginePowerDistribution[0,0] += liftPower * direction.x * rollFactor;
		enginePowerDistribution[0,1] += liftPower * direction.x * -1f * rollFactor;
		enginePowerDistribution[1,0] += liftPower * direction.x * rollFactor;
		enginePowerDistribution[1,1] += liftPower * direction.x * -1f * rollFactor;
	}
	
	private void UpdateHoverValues()
	{
		enginePowerDistribution[0,0] = liftPower;
		enginePowerDistribution[0,1] = liftPower;
		enginePowerDistribution[1,0] = liftPower;
		enginePowerDistribution[1,1] = liftPower;
	}
	
	private void UpdatePower(Vector3 direction)
	{
		enginePowerDistribution[0,0] += liftPower * direction.y * powerFactor;
		enginePowerDistribution[0,1] += liftPower * direction.y * powerFactor;
		enginePowerDistribution[1,0] += liftPower * direction.y * powerFactor;
		enginePowerDistribution[1,1] += liftPower * direction.y * powerFactor;
	}
	
	

	
	
	private void CompensatePitchValues()
	{
		compensationVector = Vector3.zero;
		compensationVector = currentAngularVelocity * -1f * angularCorrection;
		myRigidbody.AddTorque(compensationVector); //Not simulated by engine power, but works nicely.
		//compensationVector.z = currentAngularVelocity.x * -1f * pitchCorrection;
		//compensationVector.x = currentAngularVelocity.z * rollCorrention;
		//UpdatePitchValues(compensationVector);
		//UpdateRollValues(compensationVector);
	}
	
	private void PropagatePowerDistributionToEngines()
	{
		FL.UpdateEngine(enginePowerDistribution[0,0], 1f);
		FR.UpdateEngine(enginePowerDistribution[0,1], 1f);
		BL.UpdateEngine(enginePowerDistribution[1,0], 1f);
		BR.UpdateEngine(enginePowerDistribution[1,1], 1f);
	}
	
	/*
	private void CalculatePowerDistribution()
	{
		enginePowerDistribution[0,0] = liftPower * (1 + powerInput + (pitchInput * -1f * pitchFactor) + (rollInput * rollFactor));
		enginePowerDistribution[0,1] = liftPower * (1 + powerInput + (pitchInput * -1f * pitchFactor) + (rollInput * -1f * rollFactor));
		enginePowerDistribution[1,0] = liftPower * (1 + powerInput + (pitchInput * pitchFactor) + (rollInput * rollFactor));
		enginePowerDistribution[1,1] = liftPower * (1 + powerInput + (pitchInput * pitchFactor) + (rollInput * -1f * rollFactor));
	}
	

	private void DistributePitchInput()
	{
		
	}
	
	*/
}
