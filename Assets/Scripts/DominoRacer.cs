using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BlockTypes{
	normal = 0,
	fork = 1,
	hole = -1,
	
	stateKnoked = 99,
	stateStanding = 98,
}

public enum StatesCurrent{
	missing = 0,
	standingNormal = 1,
	standingFork = 2,
	knocked = -1,
}

public enum StatesNext{
	willFall = 0,
	willStand = 1,
}

public class DominoRacer : MonoBehaviour {

	public GameObject dominoPrefab;

	public Camera gameCamera;
	public GameObject[] lanesTap;

	List<List<DominoBlock>> level;

	int rows = 20;
	int lanes = 5;

	float laneDistance = 5;
	float rowsDistance = 4;

	public static List<DominoBlock> currentTumbeling;

	void Start(){
		currentTumbeling = new List<DominoBlock>();

		InitLevel();

		for (int laneIdx = 0; laneIdx < lanes; laneIdx++){
			currentTumbeling.Add(null);
			DominoBlock block = level[0][laneIdx];
			block.Tumble();
		}

		StartCoroutine(Game());

	}

	void Update(){
		if (Input.GetMouseButtonUp(0)){
			Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			Physics.Raycast(ray, out hitInfo);
			if (hitInfo.collider != null){

				for(int idx = 0; idx < lanesTap.Length; idx++){
					GameObject lane = lanesTap[idx];
					if (lane == hitInfo.collider.gameObject){
						Debug.Log("tapped" + idx);
						if (currentTumbeling[idx] != null){
							if (currentTumbeling[idx].forward.canTumble){
								currentTumbeling[idx].forward.willKnock1 = currentTumbeling[idx].forward.forwardLeft;
								currentTumbeling[idx].forward.willKnock2 = currentTumbeling[idx].forward.forwardRight;
								currentTumbeling[idx].forward.child.renderer.material.SetColor("_Color", Color.green);
							}
						}
						break;
					}
				}
			}
		}
	}

	void InitLevel(){
		level = new List<List<DominoBlock>>();

		for (int rowIdx = 0; rowIdx < rows; rowIdx++){
			List<DominoBlock> levelLane = new List<DominoBlock>();
			for (int laneIdx = 0; laneIdx < lanes; laneIdx++){
				GameObject blockGO = Instantiate(dominoPrefab) as GameObject;
				DominoBlock block = blockGO.GetComponent<DominoBlock>();
				block.laneIdx = laneIdx;
				blockGO.transform.parent = transform;
				
				levelLane.Add(block);
				
				block.gameObject.transform.localPosition = new Vector3((laneIdx - lanes / 2) * laneDistance, 0, rowIdx * rowsDistance);
			}
			level.Add(levelLane);
		}

		for (int rowIdx = 0; rowIdx < rows; rowIdx++){
			for (int laneIdx = 0; laneIdx < lanes; laneIdx++){
				DominoBlock block = level[rowIdx][laneIdx];
				if (rowIdx < rows - 1){


					DominoBlock forward = level[rowIdx + 1][laneIdx];
					DominoBlock forwardLeft = null;
					DominoBlock forwardRight = null;
					if (laneIdx > 0)
						forwardLeft = level[rowIdx + 1][laneIdx - 1];
					if (laneIdx < lanes - 1)
						forwardRight = level[rowIdx + 1][laneIdx + 1];

					block.forward = forward;
					block.forwardLeft = forwardLeft;
					block.forwardRight = forwardRight;

					block.willKnock1 = forward;
				}
			}
		}
	}


	IEnumerator Game(){
		yield return new WaitForSeconds(3);
		int counter = 0;
		while(true){

			int rand = Random.Range(-3, lanes + 3);
			if (rand < 0 || rand >= lanes)
				rand = -1;


			float lastRowZ = level[rows-1][0].transform.localPosition.z;

			List<DominoBlock> levelLane = new List<DominoBlock>();
			for (int laneIdx = 0; laneIdx < lanes; laneIdx++){
				GameObject blockGO = Instantiate(dominoPrefab) as GameObject;
				DominoBlock block = blockGO.GetComponent<DominoBlock>();
				block.laneIdx = laneIdx;
				blockGO.transform.parent = transform;

				level[rows-1][laneIdx].willKnock1 = block;

				level[rows-1][laneIdx].forward = block;
				if (laneIdx > 0)
					level[rows-1][laneIdx - 1].forwardRight = block;
				if (laneIdx < lanes - 1)
					level[rows-1][laneIdx + 1].forwardLeft = block;


				if (rand != -1){
					if (laneIdx == rand){
						block.canTumble = false;
						block.child.renderer.material.SetColor("_Color", Color.black);
					}
				}

				levelLane.Add(block);
				
				block.gameObject.transform.localPosition = new Vector3((laneIdx - lanes / 2) * laneDistance, 0, lastRowZ + 4);
			}

			level.Add(levelLane);

			yield return new WaitForSeconds(1f);

			for (int laneIdx = 0; laneIdx < lanes; laneIdx++){
				DominoBlock block = level[0][laneIdx];
				Destroy(block.gameObject);
			}

			level.RemoveAt(0);
		}
	}




	/*
	public GameObject dominoBlockPrefab;

	List<List<DominoBlock>> level;

	int rows = 10;
	int lanes = 4;
	int gameRowIdx = 3;
	
	void Start () {


		InitLevel();
		StartCoroutine(Game());
	}

	void InitLevel(){


		level = new List<List<DominoBlock>>();


		for (int rowIdx = 0; rowIdx < rows; rowIdx++){
			List<DominoBlock> levelLane = new List<DominoBlock>();
			for (int laneIdx = 0; laneIdx < lanes; laneIdx++){
				GameObject blockGO = Instantiate(dominoBlockPrefab) as GameObject;
				DominoBlock block = blockGO.GetComponent<DominoBlock>();
				block.SetType(BlockTypes.normal);
				block.stateCurrent = StatesCurrent.standingNormal;
				if (rowIdx < gameRowIdx){
					block.SetType(BlockTypes.stateKnoked);
					block.stateCurrent = StatesCurrent.knocked;
				}
				if (rowIdx == gameRowIdx){
					block.stateNext = StatesNext.willFall;
				}

				levelLane.Add(block);

				block.gameObject.transform.position = new Vector3(laneIdx * 2 - lanes, rowIdx * 2 - rows, 0);
			}
			level.Add(levelLane);
		}
		Debug.Log(level.Count);
		Debug.Log(level[0].Count);
	}

	IEnumerator Game(){


		int counter = 0;
		while (true){

			// set game row
			for (int laneIdx = 0; laneIdx < lanes; laneIdx++){

				DominoBlock block = level[gameRowIdx][laneIdx];

				if (block.stateNext == StatesNext.willFall){
					block.SetType(BlockTypes.stateKnoked);

					DominoBlock nextBlock = level[gameRowIdx + 1][laneIdx];
					if (nextBlock.stateCurrent != StatesCurrent.missing)
						nextBlock.stateNext = StatesNext.willFall;
				}

			}

			// add rows

			level.RemoveAt(0);

			int rand = -1;
			if (counter % 5 == 0)
				rand = Random.Range(0, lanes);

			List<DominoBlock> levelLane = new List<DominoBlock>();
			for (int laneIdx = 0; laneIdx < lanes; laneIdx++){
				GameObject blockGO = Instantiate(dominoBlockPrefab) as GameObject;
				DominoBlock block = blockGO.GetComponent<DominoBlock>();
				block.SetType( BlockTypes.normal);
				block.stateCurrent = StatesCurrent.standingNormal;

				if (rand != -1){
					if (laneIdx == rand){
						block.SetType( BlockTypes.hole);
						block.stateCurrent = StatesCurrent.missing;
					}
				}

				levelLane.Add(block);
				
				block.gameObject.transform.position = new Vector3(laneIdx * 2 - lanes, (rows - 1) * 2 - rows, 0);
			}
			level.Add(levelLane);



			// update positions
			for (int rowIdx = 0; rowIdx < rows; rowIdx++){
				for (int laneIdx = 0; laneIdx < lanes; laneIdx++){

					DominoBlock block = level[rowIdx][laneIdx];
					block.gameObject.transform.position = new Vector3(laneIdx * 2 - lanes, rowIdx * 2 - rows, 0);
				}
			}

			counter ++;
			yield return new WaitForSeconds(0.5f);
		}
	}
*/
}
