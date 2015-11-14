using UnityEngine;
using System.Collections;

public class Demo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int[,] level = LevelGenerator.Generate (6);
		for (int i = 0; i < level.GetLength(1); i++)
		{
			Debug.Log (level [0, i] + ", " + level [1, i] + ", " + level [2, i] + ", " + level [3, i]);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
