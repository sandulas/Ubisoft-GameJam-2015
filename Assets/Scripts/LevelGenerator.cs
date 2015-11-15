using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class LevelGenerator
{
	static int[,] level;

	public static int[,] GenerateRandom(int rowCount)
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

	public static int[,] GenerateDebug(int rowCount)
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

		level [0, 0] = 0; level [1, 0] = 0;
		level [2, 1] = 0; //level [2, 1] = 0;

		Debug.Log (isObstaclePostionValid(1, 3));
		//Debug.Log (isPathAvailable (0, 2));

		return level;

		//add obstacle (some cubes are replaced with obstacles)
		for (int rowIndex = 1; rowIndex < rowCount; rowIndex++)
		{
			int obstacleCount = Random.Range (0, 4);
			obstacleCount = 2;

			placeObstacles (rowIndex, obstacleCount);

		}
		return level;
	}

	public static int[,] Generate(int rowCount, int minObstaclesPerRow , int maxObstaclesPerRow)
	{
		level = new int[4, rowCount];

		//initialize with random cubes
		for (int position = 0; position < 4; position++)
		{
			for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
			{
				if (rowIndex % 2 == 0)
					level [position, rowIndex] = Random.Range (1, 5);
				else
					level [position, rowIndex] = Random.Range (2, 5);
			}
		}

		//add obstacle (some cubes are replaced with obstacles)
		for (int rowIndex = 1; rowIndex < rowCount; rowIndex++)
		{
			int obstacleCount = Random.Range (minObstaclesPerRow, maxObstaclesPerRow + 1);
			//obstacleCount = 2;

			placeObstacles (rowIndex, obstacleCount);

		}
		return level;
	}
	public static int[,] Generate(int rowCount)
	{
		return Generate (rowCount, 0, 1);
	}
	public static  int[,] GenerateLevel(int levelNumber)
	{
		int rowCount = 6 + (levelNumber - 1) * 2;
		int minObstaclesPerRow, maxObstaclesPerRow;

		switch (levelNumber)
		{
		case 1:
			minObstaclesPerRow = 0;
			maxObstaclesPerRow = 1;
			break;
		case 2:
			minObstaclesPerRow = 0;
			maxObstaclesPerRow = 2;
			break;
		case 3:
			minObstaclesPerRow = 1;
			maxObstaclesPerRow = 2;
			break;
		case 4:
			minObstaclesPerRow = 1;
			maxObstaclesPerRow = 2;
			break;
		case 5:
			minObstaclesPerRow = 1;
			maxObstaclesPerRow = 3;
			break;
		case 6:
			minObstaclesPerRow = 1;
			maxObstaclesPerRow = 3;
			break;
		case 7:
			minObstaclesPerRow = 2;
			maxObstaclesPerRow = 3;
			break;
		case 8:
			minObstaclesPerRow = 3;
			maxObstaclesPerRow = 3;
			break;
		default:
			minObstaclesPerRow = 3;
			maxObstaclesPerRow = 3;
			break;
		}

		return Generate (rowCount, minObstaclesPerRow, maxObstaclesPerRow);
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
		int originalStateAtObstaclePosition = level [obstaclePosition,rowIndex];
		level [obstaclePosition, rowIndex] = 0;

		for (int position = 0; position < 4; position++)
			if (level [position, rowIndex - 1] != 0)
			if (!isPathAvailable (rowIndex - 1, position))
			{
				level [obstaclePosition, rowIndex] = originalStateAtObstaclePosition;
				return false;
			}

		return true;
	}

	static bool isPathAvailable(int rowIndex, int position)
	{
		for (int i = 0; i < 3; i++)
		{
			if (level [(position + i) % 4, rowIndex] == 0)
				break;			

			if (level [(position + i) % 4, rowIndex + 1] != 0)
				return true;;
		}
		for (int i = 0; i < 3; i++)
		{
			int positionToCheck = position - i;
			if (positionToCheck < 0)
				positionToCheck = 4 + positionToCheck;

			if (level [positionToCheck, rowIndex] == 0)
				break;
			if (level [positionToCheck, rowIndex + 1] != 0)
				return true;;
		}

		return false;
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
