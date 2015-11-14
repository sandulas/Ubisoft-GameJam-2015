using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BlockController : MonoBehaviour {

	public bool isDummy = false;

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

	float tumbleTime = 1;

	public void Init(){
		if (isDummy){
			type = 1;
			tumblePivot1.SetActive(false);
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

	public void Tumble1(){

		isTumbling = true;

		if (column == GameController.columns - 1){
			GameController.dummyBlock.row = row;
			GameController.dummyBlock.column = -1;
			GameController.dummyBlock.transform.eulerAngles = transform.eulerAngles;
			GameController.dummyBlock.SetToMatrixPosition();
			GameController.dummyBlock.Tumble1();
		}

		if (!isDummy)
			GameController.levelBlocks[column, row] = null;

		Debug.Log("tumble " + " row" + row + "column" + column);
		collider.enabled = false;
		tumblePivot1.SetActive(true);
		tumblePivot1.transform.DOLocalRotate(Vector3.right * 90, tumbleTime).OnComplete(OnTumble1Complete);
	}

	public void Tumble2(){
		tumblePivot2.transform.DOLocalRotate(Vector3.right * 90, tumbleTime).OnComplete(OnTumble2Complete).SetDelay(tumbleTime / 2);
	}

	void OnTumble1Complete(){
//		tumblePivot2.transform.DOLocalRotate(Vector3.right * 90, 0.3f).OnComplete(OnTumble2Complete).SetDelay(0.3f);
		if (isDummy){
			return;
		}

		BlockController nextBlock = GetNextBlock();
		if (nextBlock != null){
			if (nextBlock.CanTumble()){
				if (column == GameController.columns - 1){
					GameController.dummyBlock.Tumble2();
				}
				Tumble2();
				nextBlock.Tumble1();
			}
		}
	}

	void OnTumble2Complete(){
		isTumbling = false;
		if (isDummy)
			return;

		if (column == GameController.columns - 1){
			GameController.dummyBlock.tumblePivot1.SetActive(false);
		}

		collider.enabled = true;

		int nextColumn;
		int nextRow;
		GetNextIdx(out nextColumn, out nextRow);

		if (nextRow != -1){
			column = nextColumn;
			row = nextRow;
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
