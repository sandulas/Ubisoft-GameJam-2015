using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public static Camera theCamera;

	public Camera gameCamera;

	public GameObject prefabBlock;

	public GameObject levelParent;


	public static int rows = 6;
	public static int columns = 4;

	// Use this for initialization
	void Start () {
		theCamera = gameCamera;
		InitLevel();
	}
	
	void Update(){
		if (Input.GetMouseButtonUp(0))
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

	public static BlockController[,] levelBlocks;
	public static BlockController dummyBlock;

	void InitLevel()
	{
		GameObject blockGODummy = Instantiate(prefabBlock) as GameObject;
		dummyBlock = blockGODummy.GetComponent<BlockController>();

		dummyBlock.isDummy = true;
		dummyBlock.row = -1;
		dummyBlock.column = -1;

		dummyBlock.Init();
		dummyBlock.SetToMatrixPosition();

		blockGODummy.name = "DUMMY";
	

		int[,] level = LevelGenerator.Generate (rows);
		levelBlocks = new BlockController[columns, rows];

		for (int row = 0; row < rows; row++)
		{
			for (int column = 0; column < columns; column++)
			{
				GameObject blockGO = Instantiate(prefabBlock) as GameObject;
				BlockController block = blockGO.GetComponent<BlockController>();

				levelBlocks[column, row] = block;

				block.row = row;
				block.column = column;
				block.type = level [column, row];
				block.Init();
				block.SetToMatrixPosition();

				blockGO.name = " row" + row + "column" + column + " type" + level [column, row];
				blockGO.transform.parent = levelParent.transform;

			}
			Debug.Log (level [0, row] + ", " + level [1, row] + ", " + level [2, row] + ", " + level [3, row]);
		}

		gameCamera.transform.position = new Vector3(0, 12.5f, 5);
	}


}
