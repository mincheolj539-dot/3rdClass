using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public EnemyData[] Enemies = new EnemyData[5000];
    public int ActiveEnemyCount { get; private set; }

    public Transform PlayerTransform;
    public string EnemyTag = "Enemy";
    public float SpawnInterval = 0.05f; 
    private float _spawnTimer;

    private void Start()
    {
        if (PlayerTransform == null)
        {
            var p = GameObject.Find("Player");
            if (p != null) PlayerTransform = p.transform;
        }
    }

    private void Update()
    {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= SpawnInterval)
        {
            _spawnTimer = 0f;
            SpawnEnemy();
        }

        if (SpatialGrid.Instance != null)
            SpatialGrid.Instance.ClearGrid();

        float dt = Time.deltaTime;
        Vector2 playerPos = (PlayerTransform != null) ? (Vector2)PlayerTransform.position : Vector2.zero;
        int activeCount = 0;

        for (int i = 0; i < Enemies.Length; i++)
        {
            if (!Enemies[i].IsActive) continue;
            activeCount++;

            Vector2 dir = (playerPos - Enemies[i].Position).normalized;
            Enemies[i].Position += dir * 2f * dt;

            if (SpatialGrid.Instance != null)
                SpatialGrid.Instance.RegisterEnemy(i, Enemies[i].Position);

            if (Enemies[i].ObjTransform != null)
                Enemies[i].ObjTransform.position = Enemies[i].Position;
        }
        
        ActiveEnemyCount = activeCount;
    }

    private void SpawnEnemy()
    {
        for (int i = 0; i < Enemies.Length; i++)
        {
            if (!Enemies[i].IsActive)
            {
                Enemies[i].IsActive = true;
                Enemies[i].Health = 10f;
                
                Vector2 rnd = Random.insideUnitCircle.normalized * 10f;
                Enemies[i].Position = (PlayerTransform != null) ? (Vector2)PlayerTransform.position + rnd : Vector2.zero;

                GameObject visual = PoolManager.Instance.SpawnFromPool(EnemyTag, Enemies[i].Position, Quaternion.identity);
                Enemies[i].ObjTransform = visual.transform;
                
                return;
            }
        }
    }

    public ref EnemyData GetEnemyData(int index)
    {
        return ref Enemies[index];
    }

    public void TakeDamage(int index, float damage)
    {
        Enemies[index].Health -= damage;
        if (Enemies[index].Health <= 0)
        {
            DespawnEnemy(index);
        }
    }

    private void DespawnEnemy(int index)
    {
        Enemies[index].IsActive = false;
        GameManager.Instance.AddKillCount();
        
        if (Enemies[index].ObjTransform != null)
        {
            PoolManager.Instance.ReturnToPool(EnemyTag, Enemies[index].ObjTransform.gameObject);
            Enemies[index].ObjTransform = null;
        }
    }

    public bool GetNearestEnemyPosition(Vector2 fromPos, float range, out Vector2 targetPos)
    {
        float minDistSq = range * range;
        int nearestIndex = -1;

        for (int i = 0; i < Enemies.Length; i++)
        {
            if (!Enemies[i].IsActive) continue;

            float distSq = (Enemies[i].Position - fromPos).sqrMagnitude;
            if (distSq < minDistSq)
            {
                minDistSq = distSq;
                nearestIndex = i;
            }
        }

        if (nearestIndex != -1)
        {
            targetPos = Enemies[nearestIndex].Position;
            return true;
        }

        targetPos = Vector2.zero;
        return false;
    }
}
