using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Obst_Gen_V2.Scripts.ChunkGen
{
    public class Chunk : MonoBehaviour
    {
        [Header("Obstacle Prefabs")] [SerializeField]
        private List<SimpleObstacle> obstaclePrefabs;

        [Space(10)] private int _numberOfObstacles;
        private const float EdgeOffset = 1.0f;
        private const float MinDistBetweenObstacles = 1.0f;
        private MeshCollider _collider;
        private Chunk _precedingChunk;
        private List<Vector3> _chunkBounds;
        [HideInInspector] public List<SimpleObstacle> obstacles;

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
            //Split the chunk up into parts, based on the amount of obstacles that will be placed
            
            var minBounds = _chunkBounds[0];
            var maxBounds = _chunkBounds[1];
            var increment = (maxBounds - minBounds).z / amount;
            var minZ = minBounds.z;
            var maxZ = minBounds.z + increment;
            for (int i = 0; i < amount; i++)
            {
                var xInsideBounds = Random.Range(minBounds.x + EdgeOffset, maxBounds.x - EdgeOffset);
                var zInsideBounds = Random.Range(minZ + EdgeOffset, maxZ - EdgeOffset);
                minZ += increment;
                maxZ += increment;
                SimpleObstacle newObst = Instantiate(obstaclePrefabs[0],
                    new Vector3(xInsideBounds, obstaclePrefabs[0].height, zInsideBounds),
                    Quaternion.identity);
                newObst.transform.SetParent(transform);
                obstacles.Add(newObst);
            }
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