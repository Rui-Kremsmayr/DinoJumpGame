using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DinoGame
{
    public class GameManager : MonoBehaviour
    {

        [Header("General")]
        [SerializeField] float _initialGameSpeed = 5;
        [SerializeField] float _gameSpeedMultiplier = 0.2f;
        [SerializeField] GameObject _gameOverScreen;
        [SerializeField] TextMeshProUGUI _scoreText;

        [Header("Tile Spawning")]    
        [SerializeField] Transform _tileSpawnPos;
        [SerializeField] Transform _tilesParent;
        [SerializeField] Tile[] _tilePrefabs;

        [Header("Bird Spawning")]
        [SerializeField] Transform _birdSpawnPos;
        [SerializeField] Bird _birdPrefab;
        [SerializeField] float _birdSpawnChance = 0.01f;
        [SerializeField] float _birdSpawnCooldown = 20; // in seconds
        float _birdCooldown;

        List<Tile> _activeTiles;
        bool _gameRunning = false;
        float _currentGameSpeed;
        public float CurrentGameSpeed => _currentGameSpeed;
        float _score;

        public static System.Action<bool> OnSetGameRunning;

        void Start()
        {
            _currentGameSpeed = _initialGameSpeed;

            _gameRunning = false;
            _gameOverScreen.SetActive(false);

            _activeTiles = new();
        }

        void OnEnable()
        {
            OnSetGameRunning += SetGameRunning;
        }

        void OnDisable()
        {
            OnSetGameRunning -= SetGameRunning;
        }

        void FixedUpdate()
        {
            if (!_gameRunning)
                return;

            _currentGameSpeed += Time.deltaTime * _gameSpeedMultiplier;
            _birdCooldown += Time.deltaTime;

            if (_birdCooldown > _birdSpawnCooldown && Random.Range(0f, 1f) < _birdSpawnChance)
                SpawnBird();  

            _score += Time.deltaTime;
            _scoreText.text = "Score: " + (int)_score;
        }

        void SetGameRunning(bool running)
        {
            _gameRunning = running;
            _currentGameSpeed = running ? _initialGameSpeed : 0;

            if (running)
            {
                _score = 0;
                _birdCooldown = 0;

                foreach (Tile tile in _activeTiles)
                {
                    Destroy(tile.gameObject);
                }
                _activeTiles.Clear();
                SpawnTile();
            }

            _gameOverScreen.SetActive(!running);
        }

        public void SpawnTile()
        {
            int len = _tilePrefabs.Length - (_currentGameSpeed > 20 ? 0 : 3); // avoids spawning of the bigger obstacles (we can't jump over them with little speed)
            Tile tile = Instantiate(_tilePrefabs[Random.Range(0, len)], _tileSpawnPos.position, Quaternion.identity, _tilesParent);
            tile.Init(this);

            _activeTiles.Add(tile);
        }

        void SpawnBird()
        {
            Bird bird = Instantiate(_birdPrefab, _birdSpawnPos.position, Quaternion.identity, _tilesParent);
            bird.Init(this);

            _activeTiles.Add(bird);

            _birdCooldown = 0;
        }

        public void DestroyTile(Tile tile) => _activeTiles.Remove(tile);

    }
}

