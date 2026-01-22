using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Settings")]
    public string ProjectilePoolTag = "Projectile";
    public float FireRate = 0.5f;
    public float DetectionRange = 10f;

    private float _fireTimer;

    private void Update()
    {
        _fireTimer += Time.deltaTime;

        if (_fireTimer >= FireRate)
        {
            FireAtNearestEnemy();
        }
    }

    private void FireAtNearestEnemy()
    {
        if (EnemyManager.Instance.GetNearestEnemyPosition(transform.position, DetectionRange, out Vector2 targetPos))
        {
            _fireTimer = 0f;
            Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
            
            ProjectileManager.Instance.FireParticle(transform.position, direction, 15f, 3f);
        }
    }
}
