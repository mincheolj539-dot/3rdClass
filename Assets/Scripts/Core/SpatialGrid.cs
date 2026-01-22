using System.Collections.Generic;
using UnityEngine;

public class SpatialGrid : MonoBehaviour
{
    public static SpatialGrid Instance;

    public int CellSize = 2; 
    public int GridWidth = 100;
    public int GridHeight = 100;

    private Dictionary<int, List<int>> _cells = new Dictionary<int, List<int>>();

    private void Awake()
    {
        Instance = this;
    }

    public void ClearGrid()
    {
        foreach (var list in _cells.Values)
        {
            list.Clear();
        }
    }

    public void RegisterEnemy(int enemyIndex, Vector2 position)
    {
        int cellKey = GetCellKey(position);

        if (!_cells.ContainsKey(cellKey))
        {
            _cells[cellKey] = new List<int>(50); 
        }

        _cells[cellKey].Add(enemyIndex);
    }

    private int GetCellKey(Vector2 position)
    {
        int x = Mathf.FloorToInt(position.x / CellSize);
        int y = Mathf.FloorToInt(position.y / CellSize);
        return x + y * 10000; 
    }

    public void GetNearbyEnemiesBroad(Vector2 position, List<int> resultBuffer)
    {
        resultBuffer.Clear();
        
        int cx = Mathf.FloorToInt(position.x / CellSize);
        int cy = Mathf.FloorToInt(position.y / CellSize);

        for (int x = cx - 1; x <= cx + 1; x++)
        {
            for (int y = cy - 1; y <= cy + 1; y++)
            {
                int key = x + y * 10000;
                if (_cells.TryGetValue(key, out var list))
                {
                    resultBuffer.AddRange(list);
                }
            }
        }
    }
}
