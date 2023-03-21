using System.Collections.Generic;
using UnityEngine;

namespace ChunkGen
{
    public class Chunk : MonoBehaviour
    {
        private int _numberOfObstacles;
        private Chunk _precedingChunk;
        public List<SimpleObstacle> obstacles;

        public void Initialize(int obstaclesToSpawn = ChunkManager.MaxObstacles)
        {
            _numberOfObstacles = obstaclesToSpawn;
        }
    }
}
