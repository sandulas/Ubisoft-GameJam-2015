using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public Camera gameCamera;

	public GameObject prefabBlock;

	public GameObject levelParent;
	// Use this for initialization
	void Start () {
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
			if (block != null)
				block.Tumble();
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

	void InitLevel()
	{
		int rows = 6;

		int[,] level = LevelGenerator.Generate (rows);

		for (int row = 0; row < rows; row++)
		{
			for (int column = 0; column < 4; column++)
			{
				GameObject blockGO = Instantiate(prefabBlock) as GameObject;
				BlockController block = blockGO.GetComponent<BlockController>();

				block.row = row;
				block.column = column;
				block.type = level [column, row];
				block.Init();

				blockGO.name = " row" + row + "column" + column + " type" + level [column, row];

				blockGO.transform.parent = levelParent.transform;
				blockGO.transform.position = new Vector3((column - 1.5f) * 2, 0, row * 2);
			}
			Debug.Log (level [0, row] + ", " + level [1, row] + ", " + level [2, row] + ", " + level [3, row]);
		}

		gameCamera.transform.position = new Vector3(0, 15, 0);
	}
}
