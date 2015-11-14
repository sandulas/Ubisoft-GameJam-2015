using UnityEngine;
using System.Collections;

public class Demo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int[,] level = LevelGenerator.Generate (6);
		string matrix = "";

		for (int i = level.GetLength(1)-1; i >= 0; i--)
		{
			matrix = matrix + level [0, i] + ", " + level [1, i] + ", " + level [2, i] + ", " + level [3, i] + "\n";
		}
		Debug.Log (matrix);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
