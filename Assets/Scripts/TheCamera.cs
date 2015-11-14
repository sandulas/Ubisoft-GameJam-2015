using UnityEngine;
using System.Collections;

public class TheCamera : MonoBehaviour {

	static TheCamera instance;
	static GameObject parentObject;
	public static TheCamera GetInstance()
	{
		if( !instance )
		{
			parentObject = GameObject.Find("TheCamera");
			if (parentObject != null)
				instance  = parentObject.GetComponent<TheCamera>();
			else
				Debug.Log("WTF : no instance found for TheCamera");
		}
		return instance;
	}

	public static Vector3 targetPosition;

	void Start () {
	
	}

	void Update () {
//		new Vector3(0, 12.5f, 5)
		float camZ = transform.position.z;
		camZ = Mathf.Lerp(camZ, targetPosition.z + 2, 0.5f * Time.deltaTime);
		transform.position = new Vector3(0, 12.5f, camZ);
	}
}
