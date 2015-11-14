using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DominoBlock : MonoBehaviour {

	public GameObject child;

	public DominoBlock forward;
	public DominoBlock forwardLeft;
	public DominoBlock forwardRight;

	public DominoBlock willKnock1; // the block that wil be knoked
	public DominoBlock willKnock2; // the block that will be knoked

	bool isTumbleing = false;

	public int laneIdx = -1;

	public bool canTumble = true;

	void Update(){
		if (isTumbleing){
			float rotationX = transform.localEulerAngles.x;
			float speed = 45;
			float delta = Time.deltaTime * speed;
			rotationX = Mathf.MoveTowards(rotationX, 45, delta);
			transform.localEulerAngles = Vector3.right * rotationX;
			if (rotationX == 45)
				OnTumbleComplete();
		}
	}

	public void Tumble(){
		if (!canTumble)
			return;
		DominoRacer.currentTumbeling[laneIdx] = this;
		isTumbleing = true;
	}

	void OnTumbleComplete(){
		DominoRacer.currentTumbeling[laneIdx] = null;
		isTumbleing = false;
		if (willKnock1 != null)
			willKnock1.Tumble();
		if (willKnock2 != null)
			willKnock2.Tumble();
	}

}
