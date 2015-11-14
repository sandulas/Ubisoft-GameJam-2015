using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BlockController : MonoBehaviour {

	public bool isDummy = false;

	public GameObject theCenter;

	public GameObject tumblePivot1;
	public GameObject tumblePivot2;
	public Renderer top;

	public int type = 0;

	public int row = -1;
	public int column = -1;

	public Material materialMovable;
	public Material materialNonMovable;
	public Material materialMoving;

	public bool canTumble = false;
	public bool isTumbling = false;

	public static float tumbleTime = 1;

	public void Init(){
		if (isDummy){
			type = 1;
			gameObject.SetActive(false);
		}

//		if (type == 0)
//			type = 1;

		if (type == 0)
		{
			canTumble = false;
			top.sharedMaterial = materialNonMovable;
		}
		else
		{
			canTumble = true;
			top.sharedMaterial = materialMovable;

			transform.localEulerAngles = Vector3.up * (type - 1) * 90;
		}
		SetToMatrixPosition();
	}

	public void SetToMatrixPosition(){
		tumblePivot1.transform.localEulerAngles = Vector3.zero;
		tumblePivot2.transform.localEulerAngles = Vector3.zero;

		transform.position = new Vector3((column - 1.5f) * 2, 0, row * 2);
	}

	public bool CanTumble(){
		if (!canTumble)
			return false;
		if (GetNextBlock() == null)
			return false;
		return true;
	}

	void Update(){
		if (isTumbling){
			TheCamera.targetPosition = theCenter.transform.position;
		}
	}

	public void Tumble1(){

		isTumbling = true;

		if (column == GameController.columns - 1){
			GameController.dummyBlockLeft.row = row;
			GameController.dummyBlockLeft.column = -1;
			GameController.dummyBlockLeft.transform.eulerAngles = transform.eulerAngles;
			GameController.dummyBlockLeft.SetToMatrixPosition();
			GameController.dummyBlockLeft.Tumble1();
		}
		else
		if (column == 0){
			GameController.dummyBlockRight.row = row;
			GameController.dummyBlockRight.column = GameController.columns;
			GameController.dummyBlockRight.transform.eulerAngles = transform.eulerAngles;
			GameController.dummyBlockRight.SetToMatrixPosition();
			GameController.dummyBlockRight.Tumble1();
		}

		if (!isDummy){
			GameController.outsideBlocks.Add(this);
			GameController.levelBlocks[column, row] = null;
		}

		Debug.Log("tumble " + " row" + row + "column" + column);
		collider.enabled = false;
		if (isDummy)
			gameObject.SetActive(true);
		tumblePivot1.transform.DOLocalRotate(Vector3.right * 90, tumbleTime).OnComplete(OnTumble1Complete);
	}

	public void Tumble2(){
		tumblePivot2.transform.DOLocalRotate(Vector3.right * 90, tumbleTime).OnComplete(OnTumble2Complete).SetDelay(tumbleTime / 3);
	}

	void OnTumble1Complete(){
//		tumblePivot2.transform.DOLocalRotate(Vector3.right * 90, 0.3f).OnComplete(OnTumble2Complete).SetDelay(0.3f);
		if (isDummy){
			return;
		}

		BlockController nextBlock = GetNextBlock();
		bool isGameOver = true;
		if (nextBlock != null){
			if (nextBlock.CanTumble()){
				if (column == GameController.columns - 1){
					GameController.dummyBlockLeft.Tumble2();
				}
				else if (column == 0){
					GameController.dummyBlockRight.Tumble2();
				}
				isGameOver = false;
				Tumble2();
				nextBlock.Tumble1();
			}
		}
		if (isGameOver){
			GameController.isGameOver = true;
			Debug.Log("GAME OVER");
		}
	}

	void OnTumble2Complete(){
		isTumbling = false;
		if (isDummy)
			return;

		if (column == GameController.columns - 1){
			GameController.dummyBlockLeft.tumblePivot1.SetActive(false);
		}
		else if (column == 0){
			GameController.dummyBlockRight.tumblePivot1.SetActive(false);
		}

		collider.enabled = true;

		int nextColumn;
		int nextRow;
		GetNextIdx(out nextColumn, out nextRow);

		if (nextRow != -1){
			column = nextColumn;
			row = nextRow;
			GameController.outsideBlocks.Remove(this);
			GameController.levelBlocks[column, row] = this;
		}

		SetToMatrixPosition();


	}
	
	public void Rotate(){
		if (isDummy)
			return;
		if (isTumbling)
			return;
		if (!canTumble)
			return;

		Debug.Log("rotate " + " row" + row + "column" + column);
		transform.Rotate(Vector3.up * 90);
	}

	public void GetNextIdx(out int nextColumn, out int nextRow){
		nextColumn = -1;
		nextRow = -1;
		if (Mathf.Abs(transform.eulerAngles.y - 0) < 1){
			//			Debug.Log("up");
			nextColumn = column;
			nextRow = row + 1;
		}
		else if (Mathf.Abs(transform.eulerAngles.y - 90) < 1){
			//			Debug.Log("right");
			nextColumn = column + 1;
			nextRow = row;
		}
		else if (Mathf.Abs(transform.eulerAngles.y - 180) < 1){
			//			Debug.Log("down");
			nextColumn = column;
			nextRow = row - 1;
		}
		else if (Mathf.Abs(transform.eulerAngles.y - 270) < 1){
			//			Debug.Log("left");
			nextColumn = column - 1;
			nextRow = row;
		}
		
		if (nextColumn < 0)
			nextColumn = GameController.columns - 1;
		if (nextColumn == GameController.columns)
			nextColumn = 0;
		if (nextRow < 0 || nextRow >= GameController.rows)
			nextRow = -1;

	}

	public BlockController GetNextBlock(){
//		Debug.Log(transform.eulerAngles.y);
		int nextColumn;
		int nextRow;
		GetNextIdx(out nextColumn, out nextRow);
		if (nextRow == -1)
			return null;

		return GameController.levelBlocks[nextColumn, nextRow];

	}
}
