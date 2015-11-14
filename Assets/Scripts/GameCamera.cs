using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

	void Start () {
	
	}
	
	void Update () {
		float speed = Time.deltaTime * 4;
		transform.position = transform.position + Vector3.forward * speed;
	}


}
