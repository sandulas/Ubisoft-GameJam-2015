using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BlockStartController : MonoBehaviour {
	static BlockStartController instance;
	static GameObject parentObject;
	public static BlockStartController GetInstance()
	{
		if( !instance )
		{
			parentObject = GameObject.Find("StarterBlock");
			if (parentObject != null)
				instance  = parentObject.GetComponent<BlockStartController>();
			else
				Debug.Log("WTF : no instance found for BlockStartController");
		}
		return instance;
	}

	public GameController gameController;

	public GameObject theCenter;

	public GameObject tumblePivot1;
	public GameObject tumblePivot2;
	public GameObject tumblePivot3;
	public GameObject tumblePivot4;

	int tumbles = 0;

	bool isTumbling = false;
	public bool isFinish = false;
	void Update(){
		if (isTumbling){
			TheCamera.targetPosition = theCenter.transform.position;
		}
	}

	int idx = 0;
	public void InitForStart(){
		isFinish = false;
		tumbles = 0;
		gameObject.SetActive(true);
		idx = Random.Range(0, GameController.columns);

		transform.position = new Vector3((idx - 1.5f) * 2, 0, -4 * 2);

		tumblePivot1.transform.DOKill();
		tumblePivot2.transform.DOKill();
		tumblePivot3.transform.DOKill();
		tumblePivot4.transform.DOKill();

		tumblePivot1.transform.localEulerAngles = Vector3.zero;
		tumblePivot2.transform.localEulerAngles = Vector3.zero;
		tumblePivot3.transform.localEulerAngles = Vector3.zero;
		tumblePivot4.transform.localEulerAngles = Vector3.zero;

		Tumble1();
	}

	public void InitForFinish(int idx){
		isFinish = true;
		tumbles = 0;
		gameObject.SetActive(true);

		transform.position = new Vector3((idx - 1.5f) * 2, 0, (GameController.rows - 1) * 2);

		tumblePivot1.transform.DOKill();
		tumblePivot2.transform.DOKill();
		tumblePivot3.transform.DOKill();
		tumblePivot4.transform.DOKill();

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
		if (isFinish){
			Tumble4();
			return;
		}

		if (tumbles == 0)
			Tumble4();
		else{

			BlockController nextBlock = GameController.levelBlocks[idx, 0];
			bool isGameOver = true;
			if (nextBlock != null){
				if (nextBlock.CanTumble()){
					isGameOver = false;
					nextBlock.Tumble1();
					Tumble4();
				}
			}
			if (isGameOver){
				GameController.isGameOver = true;

				TheSound.GetInstance().PlaySoundFail();

				Debug.Log("GAME OVER");

				TheUI.GetInstance().ShowFailed(2f);
				TheUI.GetInstance().FadeDark(1.5f);
			}

		}
	}

	public void Tumble4(){
		tumblePivot4.transform.DOLocalRotate(Vector3.right * 90, BlockController.tumbleTime).OnComplete(OnTumble4Complete);
	}
	
	void OnTumble4Complete(){
		tumbles++;

		if (isFinish){
			return;
		}

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
