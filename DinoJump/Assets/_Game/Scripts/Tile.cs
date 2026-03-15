using UnityEngine;

namespace DinoGame
{
    public class Tile : MonoBehaviour
    {
        protected GameManager _gameManager;
        bool _spawned;
        
        public void Init(GameManager gameManager)
        {
            _gameManager = gameManager;
            _spawned = false;
        }

        void FixedUpdate()
        {
            transform.Translate(new Vector3(GetSpeed() * -1, 0, 0));
        }

        protected virtual float GetSpeed() => _gameManager.CurrentGameSpeed * Time.deltaTime;

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_spawned && collision.CompareTag(Tags.T_Spawn))
            {
                _spawned = true;
                _gameManager.SpawnTile();
            }

            else if (collision.CompareTag(Tags.T_Despawn))
            {
                _gameManager.DestroyTile(this);
                Destroy(gameObject);
            }
        }
    }
}
