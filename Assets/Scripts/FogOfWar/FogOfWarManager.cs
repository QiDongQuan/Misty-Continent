using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogOfWarManager : MonoBehaviour
{
    public Tilemap fogOfWarTilemap; // 迷雾Tilemap
    public Tile clearTile;          // 用于表示"已探索区域"的Tile
    public Tile playerTile;//标记玩家的位置
    public Transform player;
    public int radius = 3;

    void Update()
    {
        ClearFogAroundPlayer(player.position, radius);
    }

    void ClearFogAroundPlayer(Vector3 worldPosition, int radius)
    {
        Vector3Int currentPlayerTile = fogOfWarTilemap.WorldToCell(worldPosition);

        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                if (x * x + y * y <= radius * radius) // 判断是否在指定的“视野”范围内
                {
                    Vector3Int tilePosition = new Vector3Int(currentPlayerTile.x + x, currentPlayerTile.y + y, currentPlayerTile.z);
                    fogOfWarTilemap.SetTile(tilePosition, clearTile); // 设置为“已探索”Tile
                }
            }
        }
        fogOfWarTilemap.SetTile(currentPlayerTile, playerTile);
    }
}
