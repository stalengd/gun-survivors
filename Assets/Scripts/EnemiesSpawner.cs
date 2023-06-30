using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Stalen.DI;
using Core.Data;

namespace Core
{
    public sealed class EnemiesSpawner : MonoService
    {
        [SerializeField] private GameModeSheet.Reference gameMode;
        [SerializeField] private int maxEnemies = 100;
        [SerializeField] private float rangeOffscreenMin = 1f;
        [SerializeField] private float rangeOffscreenMax = 2f;
        [SerializeField] private EnemyPreset[] enemiesPrefabs;

        [System.Serializable]
        private struct EnemyPreset
        {
            public GameObject prefab;
            public EnemiesSheet.Reference stats;
        }

        private struct EnemySpawner
        {
            public float spawnRate;
            public float spawnCooldown;
        }
        private EnemySpawner[] spawners;

        private new CameraController camera;
        private Player target;
        private PlayerLevel playerLevel;
        private int aliveEnemiesCount = 0;


        private void Start()
        {
            ConfigureDependecies();

            spawners = new EnemySpawner[enemiesPrefabs.Length];

            playerLevel.OnLevelUp.AddListener(OnLevelUp);
            OnLevelUp();
        }

        private void Update()
        {
            for (int i = 0; i < spawners.Length; i++)
            {
                var spawner = spawners[i];
                if (spawner.spawnRate <= 0f) continue;

                spawner.spawnCooldown -= Time.deltaTime;

                if (spawner.spawnCooldown < 0f && aliveEnemiesCount < maxEnemies)
                {
                    SpawnEnemy(enemiesPrefabs[i]);
                    spawner.spawnCooldown = 1f / spawner.spawnRate;
                }
                spawners[i] = spawner;
            }
        }


        public void EnemyKilled(Enemy enemy)
        {
            aliveEnemiesCount--;
            playerLevel.GainExperience(enemy.ExperienceReward);
        }


        private void ConfigureDependecies()
        {
            Inject.Out(out camera);
            Inject.Out(out target);
            Inject.Out(out playerLevel);
        }

        private void OnLevelUp()
        {
            RefreshSpawnRate();
            var singleSpawnEnemy = gameMode.Ref.SingleSpawn.ElementAtOrDefault(playerLevel.Level);
            if (!string.IsNullOrEmpty(singleSpawnEnemy))
            {
                SpawnEnemy(singleSpawnEnemy);
            }
        }

        private void RefreshSpawnRate()
        {
            var level = playerLevel.Level;
            for (int i = 0; i < spawners.Length; i++)
            {
                var spawnRate = enemiesPrefabs[i].stats.Ref.SpawnRate.GetOrLast(level);
                spawners[i].spawnRate = spawnRate;
                spawners[i].spawnCooldown = 1f / spawnRate;
            }
        }

        private void SpawnRandomEnemy()
        {
            var prefab = enemiesPrefabs.Random();
            SpawnEnemy(prefab);
        }

        private void SpawnEnemy(EnemyPreset preset)
        {
            var pos = GetRandomOffscreenPosition();
            SpawnEnemy(preset, pos);
        }

        private void SpawnEnemy(string id)
        {
            var pos = GetRandomOffscreenPosition();
            var prefab = enemiesPrefabs.First(p => p.stats.Id == id);
            SpawnEnemy(prefab, pos);
        }

        private void SpawnEnemy(EnemyPreset preset, Vector3 position)
        {
            var enemy = Instantiate(preset.prefab, position, Quaternion.identity)
                .GetComponent<Enemy>();
            enemy.Init(target, this);

            var level = playerLevel.Level;
            var stats = preset.stats.Ref;
            enemy.Stats.SetMaxHealth(stats.Health.GetOrLast(level), Stats.MaxHealthMigrationStrategy.FullHealth);
            enemy.MovingSpeed = stats.Speed.GetOrLast(level);

            aliveEnemiesCount++;
        }

        private Vector3 GetRandomOffscreenPosition()
        {
            Vector2 center = camera.transform.position;
            var viewSize = camera.GetViewSize();
            var offscreenRange = viewSize.magnitude;
            var spawnRange = offscreenRange + Random.Range(rangeOffscreenMin, rangeOffscreenMax);
            var angle = Random.Range(0f, 360f);
            return center + new Vector2(spawnRange, 0f).Rotate(angle);
        }
    }
}
