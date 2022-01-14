using Godot;
using System;

namespace TheBadClickyGame
{
    public class Difficulty
    {
        private PackedScene _enemyScene;
        protected RandomNumberGenerator _rng;
        protected int _position;
        protected int _lives;
        protected int _bullets;
        protected float _startSpawnTime;
        protected float _spawnTimeChange;
        protected float _spawnTimeLimit;
        protected bool _removeBullets;
        protected Texture[] _enemySmallSprites;
        protected Texture[] _enemyMediumSprites;
        protected Texture[] _enemyLargeSprites;
        protected Texture[] _enemyXLargeSprites;

        public Difficulty(Texture[] smallSprites, Texture[] medSprites, Texture[] largeSprites, Texture[] xLargeSprites)
        {
            _enemyScene = (PackedScene)ResourceLoader.Load("res://Scenes/enemy.tscn");
            _rng = new RandomNumberGenerator();
            _enemySmallSprites = smallSprites;
            _enemyMediumSprites = medSprites;
            _enemyLargeSprites = largeSprites;
            _enemyXLargeSprites = xLargeSprites;
        }
        public virtual Enemy SpawnEnemy() 
        {
            Enemy enem = (Enemy)_enemyScene.Instance();
            int shipSize = _rng.RandiRange(0, 4);
            int spriteIndex = _rng.RandiRange(0, 1);
            switch (shipSize)
            {
                case 0:
                    enem.sprite = _enemyMediumSprites[spriteIndex];
                    enem.Scale = new Vector2(1f, 1f);
                break;
                case 1:
                    enem.sprite = _enemyMediumSprites[spriteIndex];
                    enem.Scale = new Vector2(1.25f, 1.25f);
                break;
                case 2:
                    enem.sprite = _enemyMediumSprites[spriteIndex];
                    enem.Scale = new Vector2(1.75f, 1.75f);
                break;
                case 3:
                    enem.sprite = _enemyMediumSprites[spriteIndex];
                    enem.Scale = new Vector2(1.5f, 1.5f);
                break;
                case 4:
                    enem.sprite = _enemyMediumSprites[spriteIndex];
                    enem.Scale = new Vector2(0.75f, 0.75f);
                break;
                default:
                    enem.sprite = _enemyMediumSprites[spriteIndex];
                    enem.Scale = new Vector2(1f, 1f);
                    GD.Print("Default being called - ShipSize: "+shipSize);
                break;
            }    
            float posX = _rng.Randf() * (1024-(32 * enem.Scale.x));
            if (posX < (64/2) * enem.Scale.x)
            {
                posX = (64/2) * enem.Scale.x;
            }
            float posY = _rng.Randf() * -64;
            enem.Position = new Vector2(posX, posY);
            return enem;
        }
            
        public virtual void ResetData()
        {

        }

        public virtual void SetResetData(int lives, int bullets, float startSpawnTime, float spawnTimeChange, float spawnTimeLimit, bool removeBullets)
        {
            _lives = lives;
            _bullets = bullets;
            _startSpawnTime = startSpawnTime;
            _spawnTimeChange = spawnTimeChange;
            _spawnTimeLimit = spawnTimeLimit;
            _removeBullets = removeBullets;
        }

        public int GetLives()
        {
            return _lives;
        }

        public int GetBullets()
        {
            return _bullets;
        }

        public float GetStartTime()
        {
            return _startSpawnTime;
        }

        public float GetTimeChange()
        {
            return _spawnTimeChange;
        }

        public float GetTimeLimit()
        {
            return _spawnTimeLimit;
        }

        public int GetPos()
        {
            return _position;
        }

        protected PackedScene GetEnemyScene()
        {
            return _enemyScene;
        }

        public bool IsRemovingBullets()
        {
            return _removeBullets;
        }
    }
}