using UnityEngine;
using System.Collections.Generic;

public class ProjectileManager : Singleton<ProjectileManager>
{
    public ParticleSystem ProjectilePS;
    
    private ParticleSystem.Particle[] _particles = new ParticleSystem.Particle[10000];
    
    public int Damage = 1;

    private List<int> _nearbyBuffer = new List<int>(100);

    private void Start()
    {
        if (ProjectilePS == null)
        {
            ProjectilePS = GetComponent<ParticleSystem>();
        }
    }

    private void LateUpdate()
    {
        if (ProjectilePS == null || SpatialGrid.Instance == null) return;

        int count = ProjectilePS.GetParticles(_particles);
        
        bool changes = false;

        for (int i = 0; i < count; i++)
        {
            if (_particles[i].remainingLifetime <= 0) continue;

            Vector2 pos = _particles[i].position;

            SpatialGrid.Instance.GetNearbyEnemiesBroad(pos, _nearbyBuffer);
            
            if (_nearbyBuffer.Count == 0) continue;

            bool hit = false;
            for (int k = 0; k < _nearbyBuffer.Count; k++)
            {
                int eIndex = _nearbyBuffer[k];
                ref EnemyData enemy = ref EnemyManager.Instance.GetEnemyData(eIndex);

                if (!enemy.IsActive) continue;

                float distSq = (enemy.Position - pos).sqrMagnitude;
                if (distSq < 1.0f)
                {
                    EnemyManager.Instance.TakeDamage(eIndex, Damage);
                    hit = true;
                    break;
                }
            }

            if (hit)
            {
                _particles[i].remainingLifetime = -1f;
                changes = true;
            }
        }

        if (changes)
        {
            ProjectilePS.SetParticles(_particles, count);
        }
    }

    public void FireParticle(Vector2 pos, Vector2 dir, float speed, float lifetime)
    {
        if (ProjectilePS == null) return;

        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
        emitParams.position = pos;
        emitParams.velocity = dir * speed;
        emitParams.startLifetime = lifetime;
        
        ProjectilePS.Emit(emitParams, 1);
    }
}
