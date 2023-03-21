using Interfaces;
using UnityEngine;

namespace ChunkGen
{
    public class SimpleObstacle : MonoBehaviour, IObstacle
    {
        public void OnCollision()
        {
            Destroy(gameObject);
        }
    }
}
