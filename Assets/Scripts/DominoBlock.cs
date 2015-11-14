using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DominoBlock : MonoBehaviour {

	public DominoBlock willKnock1; // the block that wil be knoked
	public DominoBlock willKnock2; // the block that will be knoked

	public bool isPresent = true;

	public void Tumble(){
		if (isPresent)
			transform.DOLocalRotate(Vector3.right * 45, 1f).OnComplete(OnTumbleComplete);
	}

	void OnTumbleComplete(){
		if (willKnock1 != null)
			willKnock1.Tumble();
		if (willKnock2 != null)
			willKnock2.Tumble();
	}

}
