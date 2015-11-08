using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {

	public float speed=0.1f; 
	public float radius=0.1f;

	Vector3 startPosition;

	void Start() {
		startPosition=transform.position;
	}

	void Update () {
		transform.position=startPosition+new Vector3(Mathf.Cos(Time.time*speed), Mathf.Sin(Time.time*speed))*radius;
	}
	
	void OnDrawGizmos() {
		Gizmos.color=Color.yellow;
		for (int i = 0; i < 360; i++) {
			float alpha=i*Mathf.Deg2Rad;
			float beta=(i+1)*Mathf.Deg2Rad;
			Gizmos.DrawLine(startPosition+new Vector3(Mathf.Cos(alpha), Mathf.Sin(alpha))*radius, startPosition+new Vector3(Mathf.Cos(beta), Mathf.Sin(beta))*radius);
		}
		Gizmos.color=Color.white;
		Gizmos.DrawLine(startPosition, startPosition+new Vector3(Mathf.Cos(Time.time*speed), Mathf.Sin(Time.time*speed))*radius);
	}
}
