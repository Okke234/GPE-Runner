using Obst_Gen_V1.Scripts.Interfaces;
using UnityEngine;

namespace Obst_Gen_V1.Scripts.Player
{
    public class Player : MonoBehaviour
    {
        #region Singleton
        private static Player _instance;
        public static Player Instance { get { return _instance; } }
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

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            //Debug.Log("I am colliding!");
            IObstacle obstacle = hit.collider.GetComponent<IObstacle>();
            obstacle?.OnCollision();
        }
    }
}
