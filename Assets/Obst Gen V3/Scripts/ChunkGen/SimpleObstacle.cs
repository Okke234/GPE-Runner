using Obst_Gen_V3.Scripts.Interfaces;
using UnityEngine;

namespace Obst_Gen_V3.Scripts.ChunkGen
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
