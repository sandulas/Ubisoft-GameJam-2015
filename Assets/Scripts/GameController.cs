using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public static Camera theCamera;

	public Camera gameCamera;

	public GameObject prefabBlock;

	public GameObject levelParent;

	public BlockStartController startBlock;


	public static int rows = 4;
	public static int columns = 4;


	public static BlockController[,] levelBlocks;
	public static BlockController dummyBlockLeft;
	public static BlockController dummyBlockRight;
	public static List<BlockController> outsideBlocks;

	public static bool isGameOver = false;


	// Use this for initialization
	void Start () {

		startBlock = BlockStartController.GetInstance();

		theCamera = gameCamera;

		InitDummy();


	}

//	void OnGUI(){
//
//		if (GUI.Button(new Rect(0, 0, Screen.width, 50), "NEW GAME")){
//
//		}
//	}


	public int currentLevelNumber = -1;
	public void StartGame(int levelNumber = -1){
		if (levelNumber != -1)
			currentLevelNumber = levelNumber;

		StartCoroutine(StartGameCO());
	}

	IEnumerator StartGameCO(){
		yield return new WaitForSeconds(0.5f);
		isGameOver = false;
		
		dummyBlockLeft.gameObject.SetActive(false);
		dummyBlockRight.gameObject.SetActive(false);
		
		DestroyLevel();
		InitLevel();
		startBlock.InitForStart();
		theCamera.transform.position = new Vector3(0, 12.5f, -10);
		yield return null;
		yield return null;
		yield return null;
		TheUI.GetInstance().FadeDark(0);
	}
	
	void Update(){
		if (isGameOver)
			return;
		if (Input.GetMouseButtonDown(0))
		{
			BlockController block = GetBlockForScreenPos(Input.mousePosition);
			if (block != null)
				block.Rotate();
		}

		if (Input.GetMouseButtonUp(1))
		{
			BlockController block = GetBlockForScreenPos(Input.mousePosition);
			if (block != null){
				levelBlocks[block.column, block.row] = null;
				block.Tumble1();
			}
		}
	}

	BlockController GetBlockForScreenPos(Vector3 screenPos){
		Ray ray = gameCamera.ScreenPointToRay(screenPos);
		RaycastHit hitInfo;
		Physics.Raycast(ray, out hitInfo);
		if (hitInfo.collider != null)
		{
			
			GameObject hitObject = hitInfo.collider.gameObject;
			
			return hitObject.GetComponent<BlockController>();
			
		}
		return null;
	}



	void InitDummy(){
		GameObject blockGODummy = Instantiate(prefabBlock) as GameObject;
		dummyBlockLeft = blockGODummy.GetComponent<BlockController>();
		
		dummyBlockLeft.isDummy = true;
		dummyBlockLeft.row = -1;
		dummyBlockLeft.column = -1;
		
		dummyBlockLeft.Init();
		
		blockGODummy.name = "DUMMY LEFT";


		blockGODummy = Instantiate(prefabBlock) as GameObject;
		dummyBlockRight = blockGODummy.GetComponent<BlockController>();
		
		dummyBlockRight.isDummy = true;
		dummyBlockRight.row = -1;
		dummyBlockRight.column = columns;
		
		dummyBlockRight.Init();
		
		dummyBlockRight.name = "DUMMY RIGHT";
		

	}

	void InitLevel()
	{
		outsideBlocks = new List<BlockController>();

		int[,] level = LevelGenerator.Generate (rows);
		levelBlocks = new BlockController[columns, rows];

		for (int row = 0; row < rows; row++)
		{
			for (int column = 0; column < columns; column++)
			{
				GameObject blockGO = NewBlock();
				BlockController block = blockGO.GetComponent<BlockController>();

				levelBlocks[column, row] = block;

				block.row = row;
				block.column = column;
				block.type = level [column, row];
				block.Init();
//				block.SetToMatrixPosition();

				blockGO.name = " row" + row + "column" + column + " type" + level [column, row];
				blockGO.transform.parent = levelParent.transform;

			}
			Debug.Log (level [0, row] + ", " + level [1, row] + ", " + level [2, row] + ", " + level [3, row]);
		}

//		gameCamera.transform.position = new Vector3(0, 12.5f, 5);
	}

	public GameObject NewBlock(){
		return Instantiate(prefabBlock) as GameObject;
	}

	void DestroyLevel(){
		if (outsideBlocks != null){
			for(int i = 0; i < outsideBlocks.Count; i++)
				if (outsideBlocks[i] != null)
					Destroy(outsideBlocks[i].gameObject);
		}
		if (levelBlocks != null){
			for (int row = 0; row < rows; row++)
			{
				for (int column = 0; column < columns; column++)
				{
					if (levelBlocks[column, row] != null)
					{
						Destroy(levelBlocks[column, row].gameObject);
					}
				}
			}
		}
		levelBlocks = null;
		outsideBlocks = null;

	}


}
