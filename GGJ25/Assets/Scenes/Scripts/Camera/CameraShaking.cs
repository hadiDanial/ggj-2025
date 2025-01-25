using UnityEngine;
using System.Collections;

public class CameraShaking : MonoBehaviour 
{

	public float shakeTimer;
	public float shakeIntensity;

	public Vector3 startingPosition;

	void Start()
	{
		startingPosition = transform.position;
	}

	void Update () 
	{
		if(shakeTimer > 0)
		{
			if(transform.position == startingPosition)
			{
			Vector2 shakePos = Random.insideUnitCircle * shakeIntensity;

			transform.position = new Vector3(transform.position.x + shakePos.x, transform.position.y + shakePos.y, startingPosition.z);

			shakeTimer -= Time.deltaTime;
			}
			else
			{
				transform.position = startingPosition;

				shakeTimer -= Time.deltaTime;
			}
		}
		if(shakeTimer < 0)
		{
			transform.position = startingPosition;
		}
	}

	public void ShakeScreen(float intensity, float time)
	{
		shakeTimer = time;
		shakeIntensity = intensity;
	}
}
