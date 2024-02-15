using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private Tile _Tile;
    private int TileNum = 1;

    void GenerateGrid()
    {
        for (int row = 0; row < width; row++)
        {
            for (int column = 0; column < height; column++)
            {
                var spawnedTile = Instantiate(_Tile, new Vector3(column, 7 - row, -2), Quaternion.identity);
                spawnedTile.name = $"Tile {TileNum}";
                TileNum++;

                var isOffset = (row + column) % 2 == 1;
                spawnedTile.Init(isOffset);

            }
        }
    }
    void Start()
    {
        GenerateGrid();
    }
}
