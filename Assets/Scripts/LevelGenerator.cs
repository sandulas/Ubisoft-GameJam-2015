using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class LevelGenerator
{
	static int[,] level;

	public static int[,] Generate2(int rowCount)
	{
		level = new int[4, rowCount];

		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < rowCount; j++)
			{
				level [i, j] = Random.Range (0, 5);
			}
		}
		return level;
	}

	public static int[,] Generate(int rowCount)
	{
		level = new int[4, rowCount];

		//initialize with random cubes
		for (int position = 0; position < 4; position++)
		{
			for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
			{
				level [position, rowIndex] = Random.Range (1, 5);
			}
		}

		//add obstacle (some cubes are replaced with obstacles)
		for (int rowIndex = 1; rowIndex < rowCount; rowIndex++)
		{
			int obstacleCount = Random.Range (0, 4);
			//obstacleCount = 2;

			placeObstacles (rowIndex, obstacleCount);

		}
		return level;
	}

	static void placeObstacles(int rowIndex, int obstacleCount)
	{
		//copy the n-th row from level into row, replacing 1, 2 and 3 with 1;
		int[] row = new int[4];
		for (int position = 0; position < 4; position++)
			row [position] = level [position, rowIndex] == 0 ? 0 : 1;

		List<int> availablePositions = new List<int>();

		for (int i = 0; i < obstacleCount; i++)
		{
			//create list of available positions from the row
			availablePositions.Clear ();

			for (int position = 0; position < 4; position++)
			{
				if (row[position] == 1) availablePositions.Add(position);
			}

			//select a random position from the list
			int obstaclePosition = availablePositions[Random.Range (0, availablePositions.Count)];


			//check if the position is valid
			if (isObstaclePostionValid (rowIndex, obstaclePosition))
				row [obstaclePosition] = 0;
			else
				row [obstaclePosition] = 2;
		}

		for (int position = 0; position < 4; position++)
		{
			if (row[position] == 0) level [position, rowIndex] = 0;
		}
	}


	static bool isObstaclePostionValid(int rowIndex, int obstaclePosition)
	{
		return true;
		int originalStateAtObstaclePosition = level [rowIndex, obstaclePosition];

		for (int position = 0; position < 4; position++)
			if (level [rowIndex - 1, position] != 0)
			if (!isPathAvailable (rowIndex - 1, position))
				return false;

		level [rowIndex, obstaclePosition] = originalStateAtObstaclePosition;
		return true;
	}

	static bool isPathAvailable(int rowIndex, int position)
	{
		return true;
	}




	static void printArray(int[] x)
	{
		string s = "";
		for (int i = 0; i < x.Length; i++)
		{
			s = s + x[i] + ", ";
		}
		Debug.Log (s);
	}
}
