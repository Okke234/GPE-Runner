using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ChunkGen
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

            SpawnObstacles(_numberOfObstacles);
        }

        private void SpawnObstacles(int amount)
        {
            var minBounds = _collider.bounds.min;
            var maxBounds = _collider.bounds.max;
            var xInsideBounds = Random.Range(minBounds.x + EdgeOffset, maxBounds.x - EdgeOffset);
            var zInsideBounds = Random.Range(minBounds.z + EdgeOffset, maxBounds.z - EdgeOffset);
            for (int i = 0; i < amount; i++)
            {
                if (obstacles.Count > 0)
                {
                    //nvm dit gaat ook niet zo werken...
                    //SimpleObstacle newObst = Instantiate(obstaclePrefabs[0], GeneratePointInsideChunk(_chunkBounds))
                }

                SimpleObstacle newObst = Instantiate(obstaclePrefabs[0],
                    new Vector3(xInsideBounds, obstaclePrefabs[0].height, zInsideBounds),
                    Quaternion.identity);
                newObst.transform.SetParent(transform);
                obstacles.Add(newObst);
            }

            //take obstacles from previous chunks into account
        }

        public void Delete()
        {
            foreach (var obstacle in obstacles)
            {
                if (obstacle != null) Destroy(obstacle.gameObject);
            }

            Destroy(gameObject);
        }

        private Vector3 GeneratePointInsideChunk(List<Vector3> chunkBounds)
        {
            Vector3 MinVec = MinPoint(chunkBounds);
            Vector3 MaxVec = MaxPoint(chunkBounds);
            Vector3 GenVector;

            float x = ((Random.value) * (MaxVec.x - MinVec.x)) + MinVec.x;
            float z = ((Random.value) * (MaxVec.z - MinVec.z)) + MinVec.z;
            GenVector = new Vector3(x, 0.0f, z);

            while (!IsPointInChunk(chunkBounds, GenVector))
            {
                x = ((Random.value) * (MaxVec.x - MinVec.x)) + MinVec.x;
                z = ((Random.value) * (MaxVec.z - MinVec.z)) + MinVec.z;
                GenVector.x = x;
                GenVector.z = z;
            }

            return GenVector;
        }

        private Vector3 MinPoint(List<Vector3> chunkBounds)
        {
            float minX = chunkBounds[0].x;
            float minZ = chunkBounds[0].z;
            for (int i = 1; i < chunkBounds.Count; i++)
            {
                if (minX > chunkBounds[i].x)
                {
                    minX = chunkBounds[i].x;
                }

                if (minZ > chunkBounds[i].z)
                {
                    minZ = chunkBounds[i].z;
                }
            }

            return new Vector3(minX, 0.0f, minZ);
        }

        private Vector3 MaxPoint(List<Vector3> chunkBounds)
        {
            float maxX = chunkBounds[0].x;
            float maxZ = chunkBounds[0].z;
            for (int i = 1; i < chunkBounds.Count; i++)
            {
                if (maxX < chunkBounds[i].x)
                {
                    maxX = chunkBounds[i].x;
                }

                if (maxZ < chunkBounds[i].z)
                {
                    maxZ = chunkBounds[i].z;
                }
            }

            return new Vector3(maxX, 0.0f, maxZ);
        }

        private bool IsPointInChunk(List<Vector3> chunkBounds, Vector3 point)
        {
            bool isInside = false;
            for (int i = 0, j = chunkBounds.Count - 1; i < chunkBounds.Count; j = i++)
            {
                if (((chunkBounds[i].x > point.x) != (chunkBounds[j].x > point.x)) &&
                    (point.z <
                     (chunkBounds[j].z - chunkBounds[i].z) * (point.x - chunkBounds[i].x) / (chunkBounds[j].x - chunkBounds[i].x) +
                     chunkBounds[i].z))
                {
                    isInside = !isInside;
                }
            }

            return isInside;
        }
    }
}