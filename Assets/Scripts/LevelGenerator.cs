using UnityEngine;
using System.Collections;

public static class LevelGenerator
{
	// Use this for initialization
	public static int[,] Generate(int n)
	{
		int[,] level = new int[4, n];
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < n; j++)
			{
				level [i, j] = Random.Range (0, 4);
			}
		}
		return level;
	}

}
