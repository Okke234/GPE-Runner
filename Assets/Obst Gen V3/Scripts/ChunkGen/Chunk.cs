using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Obst_Gen_V3.Scripts.ChunkGen
{
    public class Chunk : MonoBehaviour
    {
        [Header("Obstacle Prefabs")] [SerializeField]
        private List<SimpleObstacle> obstaclePrefabs;

        [Space(10)] private int _numberOfObstacles;
        private const float EdgeOffset = 1.0f;
        private const float MinDistBetweenObstacles = 10.0f;
        private const int MaxSpawnAttempts = 5;
        private MeshCollider _collider;
        private Chunk _precedingChunk;
        private List<Vector3> _chunkBounds;
        public List<SimpleObstacle> obstacles;

        public void Initialize(int obstaclesToSpawn = ChunkManager.MaxObstacles)
        {
            _collider = GetComponent<MeshCollider>();
            _chunkBounds = new List<Vector3> { _collider.bounds.min, _collider.bounds.max };
            _numberOfObstacles = obstaclesToSpawn;
            if (ChunkManager.Instance.Chunks.Count > 0)
            {
                _precedingChunk = ChunkManager.Instance.Chunks.Last();
            }

            OffsetTexture();
            SpawnObstacles(_numberOfObstacles);
        }

        private void SpawnObstacles(int amount)
        {
            var minBounds = _chunkBounds[0];
            var maxBounds = _chunkBounds[1];
            for (int i = 0; i < amount; i++)
            {
                var attempts = 0;
                var nearbyObstacles = GetNearbyObstacles();
                while (attempts < MaxSpawnAttempts)
                {
                    var xInsideBounds = Random.Range(minBounds.x + EdgeOffset, maxBounds.x - EdgeOffset);
                    var zInsideBounds = Random.Range(minBounds.z + EdgeOffset, maxBounds.z - EdgeOffset);
                    var distances = new List<float>();
                    if (nearbyObstacles.Count < 1)
                    {
                        SpawnObstacle(xInsideBounds, zInsideBounds);
                        goto AttemptsSuccessful;
                    }
                    
                    foreach (var obstacle in nearbyObstacles)
                    {
                        distances.Add(Vector3.Distance(
                            new Vector3(xInsideBounds, obstaclePrefabs[0].height, zInsideBounds),
                            obstacle.transform.position));
                    }

                    if (DistanceCheck(distances))
                    {
                        SpawnObstacle(xInsideBounds, zInsideBounds);
                        goto AttemptsSuccessful;
                    }
                        
                    attempts++;
                }
                AttemptsSuccessful:;
            }
        }

        private List<SimpleObstacle> GetNearbyObstacles()
        {
            var availableChunks = ChunkManager.Instance.Chunks.ToList();
            var obstaclesToCheck = new List<SimpleObstacle>();
            foreach (var obstacle in availableChunks[^1].obstacles)
            {
                obstaclesToCheck.Add(obstacle);
            }
            foreach (var obstacle in obstacles)
            {
                obstaclesToCheck.Add(obstacle);
            }
            
            return obstaclesToCheck;
        }

        private bool DistanceCheck(List<float> distances)
        {
            foreach (var distance in distances)
            {
                if (distance < MinDistBetweenObstacles)
                {
                    return false;
                }
            }

            return true;
        }

        private void SpawnObstacle(float x, float z)
        {
            SimpleObstacle newObst = Instantiate(obstaclePrefabs[0],
                new Vector3(x, obstaclePrefabs[0].height, z), Quaternion.identity);
            newObst.transform.SetParent(transform);
            obstacles.Add(newObst);
        }

        public void Delete()
        {
            foreach (var obstacle in obstacles)
            {
                if (obstacle != null) Destroy(obstacle.gameObject);
            }

            Destroy(gameObject);
        }
        
        private void OffsetTexture()
        {
            var mat = GetComponent<MeshRenderer>().material;
            var x = Random.Range(0f, 1f);
            var y = Random.Range(0f, 1f);
            
            mat.SetTextureOffset("_MainTex", new Vector2(x, y));
            mat.SetTextureOffset("_BumpMap", new Vector2(x, y));
        }
    }
}