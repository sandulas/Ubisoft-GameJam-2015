using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BlockStartController : MonoBehaviour {

	public GameController gameController;

	public GameObject theCenter;

	public GameObject tumblePivot1;
	public GameObject tumblePivot2;
	public GameObject tumblePivot3;
	public GameObject tumblePivot4;

	int tumbles = 0;

	bool isTumbling = false;
	void Update(){
		if (isTumbling){
			TheCamera.targetPosition = theCenter.transform.position;
		}
	}

	int idx = 0;
	public void Init(){
		tumbles = 0;
		gameObject.SetActive(true);
		idx = Random.Range(0, GameController.columns);

		transform.position = new Vector3((idx - 1.5f) * 2, 0, -4 * 2);
		tumblePivot1.transform.localEulerAngles = Vector3.zero;
		tumblePivot2.transform.localEulerAngles = Vector3.zero;
		tumblePivot3.transform.localEulerAngles = Vector3.zero;
		tumblePivot4.transform.localEulerAngles = Vector3.zero;

		Tumble1();
	}

	void Tumble1(){
		isTumbling = true;
		tumblePivot1.transform.DOLocalRotate(Vector3.right * 90, BlockController.tumbleTime).OnComplete(OnTumble1Complete);
	}

	void OnTumble1Complete(){
		Tumble2();
	}

	public void Tumble2(){
		tumblePivot2.transform.DOLocalRotate(Vector3.right * 90, BlockController.tumbleTime).OnComplete(OnTumble2Complete);
	}
	
	void OnTumble2Complete(){
		Tumble3();
	}

	public void Tumble3(){
		tumblePivot3.transform.DOLocalRotate(Vector3.right * 90, BlockController.tumbleTime).OnComplete(OnTumble3Complete);
	}
	
	void OnTumble3Complete(){
		if (tumbles == 0)
			Tumble4();
		else{

			BlockController nextBlock = GameController.levelBlocks[idx, 0];
			if (nextBlock != null){
				if (nextBlock.CanTumble()){

					nextBlock.Tumble1();
					Tumble4();
				}
			}

		}
	}

	public void Tumble4(){
		tumblePivot4.transform.DOLocalRotate(Vector3.right * 90, BlockController.tumbleTime).OnComplete(OnTumble4Complete);
	}
	
	void OnTumble4Complete(){
		tumbles++;

		tumblePivot1.transform.localEulerAngles = Vector3.zero;
		tumblePivot2.transform.localEulerAngles = Vector3.zero;
		tumblePivot3.transform.localEulerAngles = Vector3.zero;
		tumblePivot4.transform.localEulerAngles = Vector3.zero;
		transform.position = new Vector3((idx - 1.5f) * 2, 0, -2 * 2);
		if (tumbles == 1)
			Tumble1();
		else{
			gameObject.SetActive(false);
			isTumbling = false;

			GameObject blockGO = gameController.NewBlock();
			BlockController block = blockGO.GetComponent<BlockController>();
			
			GameController.levelBlocks[idx, 0] = block;
			
			block.row = 0;
			block.column = idx;
			block.type = 1;
			block.Init();
			
			blockGO.name = " row" + 0 + "column" + idx + " type" + 1;
			blockGO.transform.parent = gameController.levelParent.transform;


		}
	}
}
