using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ChunkGen
{
    public class ChunkManager : MonoBehaviour
    {
        [SerializeField] private float maxSpawnDistance = 120f;
        [SerializeField] private float minDeleteDistance = 20f;
        [SerializeField] private Chunk chunkPrefab;
        [SerializeField] private SimpleObstacle obstacle;
        private const float ChunkLength = 10f;
        private const int InitialSpawnQuantity = 12;
        public const int MaxObstacles = 3;
        private readonly Quaternion _defaultRotation = Quaternion.Euler(90f, 0f, 0f);
        private Queue<Chunk> _chunks = new Queue<Chunk>();
        private Player.Player player;

        #region Singleton
        private static ChunkManager _instance;
        public static ChunkManager Instance { get { return _instance; } }
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        
            DontDestroyOnLoad(gameObject);
        }
        #endregion

        private void Start()
        {
            player = Player.Player.Instance;
            SpawnInitialChunks();
            
            // Test cubes
            for (var i = 0; i < 50; i++)
            {
                SimpleObstacle cube = Instantiate(obstacle, new Vector3(0,0,0), Quaternion.identity);
                cube.transform.position = new Vector3(2.0f, 1.0f, 10f * i);
            }
        }

        // Make the world move, rather than the player.
        void Update()
        {
        
        }

        private void LateUpdate()
        {
            if (ShouldSpawnChunk())
            {
                CreateChunk();
            }

            if (ShouldDeleteChunk())
            {
                DeleteChunk(_chunks.First());
            }
        }

        private bool ShouldSpawnChunk()
        {
            return Mathf.Abs((_chunks.Last().transform.position - player.transform.position).z)
                   <= maxSpawnDistance;
        }

        private bool ShouldDeleteChunk()
        {
            return Mathf.Abs((_chunks.First().transform.position - player.transform.position).z)
                   >= minDeleteDistance;
        }

        private void CreateChunk()
        {
            var zOffset = _chunks.Last().transform.position.z + ChunkLength;
            var chunk = Instantiate(chunkPrefab, new Vector3(0f, 0f, zOffset), _defaultRotation);
            chunk.Initialize();
            _chunks.Enqueue(chunk);
        }

        private void DeleteChunk(Chunk chunk)
        {
            _chunks.Dequeue();
            Destroy(chunk.gameObject);
        }

        /*private void MoveAllChunks(float speed)
    {
        
    }*/

        private void SpawnInitialChunks()
        {
            for (var i = 0; i < InitialSpawnQuantity; i++)
            {
                var zPos = 10f * i;
                var chunk = Instantiate(chunkPrefab, new Vector3(0, 0, zPos), _defaultRotation);
                chunk.Initialize(Mathf.Clamp(i, 0, MaxObstacles));
                _chunks.Enqueue(chunk);
            }
        }
    }
}
