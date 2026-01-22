using UnityEngine;

[System.Serializable]
public struct EnemyData
{
    public bool IsActive;
    public Vector2 Position;
    public float Health;
    public Transform ObjTransform;
}

[System.Serializable]
public struct ProjectileData
{
    public bool IsActive;
    public Vector2 Position;
    public Vector2 Direction;
    public float Timer;
    public Transform ObjTransform;
}
