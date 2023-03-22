using Interfaces;
using UnityEngine;

namespace ChunkGen
{
    public class SimpleObstacle : MonoBehaviour, IObstacle
    {
        public float height;
        public void OnCollision()
        {
            Destroy(gameObject);
            
            //fire event to let the chunk know it's gone.
        }
    }
}
