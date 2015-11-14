using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BlockController : MonoBehaviour {

	public GameObject cube;
	public Renderer top;

	public int type = 0;

	public int row = -1;
	public int column = -1;

	public Material materialMovable;
	public Material materialNonMovable;

	public void Init(){
		if (type == 0)
		{
			collider.enabled = false;
			top.sharedMaterial = materialNonMovable;
		}
		else
		{
			top.sharedMaterial = materialMovable;

			transform.localEulerAngles = Vector3.up * (type - 1) * 90;
		}
	}

	public void Tumble(){
		cube.transform.DOLocalRotate(Vector3.right * 90, 0.3f);
	}

	public void Rotate(){
		transform.Rotate(Vector3.up * 90);
	}
}
