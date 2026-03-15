using UnityEngine;

namespace DinoGame
{
    public class Bird : Tile
    {
        protected override float GetSpeed() => _gameManager.CurrentGameSpeed * Time.deltaTime * 1.5f;

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Tags.T_Despawn))
            {
                _gameManager.DestroyTile(this);
                Destroy(gameObject);
            }
        }
    }
}
