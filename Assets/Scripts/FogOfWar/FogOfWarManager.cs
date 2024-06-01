using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogOfWarManager : MonoBehaviour
{
    public Tilemap fogOfWarTilemap; // ����Tilemap
    public Tile clearTile;          // ���ڱ�ʾ"��̽������"��Tile
    public Tile playerTile;//�����ҵ�λ��
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
                if (x * x + y * y <= radius * radius) // �ж��Ƿ���ָ���ġ���Ұ����Χ��
                {
                    Vector3Int tilePosition = new Vector3Int(currentPlayerTile.x + x, currentPlayerTile.y + y, currentPlayerTile.z);
                    fogOfWarTilemap.SetTile(tilePosition, clearTile); // ����Ϊ����̽����Tile
                }
            }
        }
        fogOfWarTilemap.SetTile(currentPlayerTile, playerTile);
    }
}
