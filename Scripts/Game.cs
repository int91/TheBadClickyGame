using Godot;
using System;

namespace TheBadClickyGame 
{
    public class Game : Node2D
    {
        //TODO: Add The Kill & Death Sounds (From the purchased asset pack)
        //TODO: Save and Load Settings
        //TODO: Add Highscore System
        //TODO: Save and Load Highscore
        //TODO: Add the bullet crate that gives you bullets
        int points;
        int lives;
        int bullets;
        float spawnTime;
        float spawnTimeChange;
        float spawnTimeLimit;
        public Enemy hoveredEntity;
        Texture[] enemyMediumSprites;
        DeathPanel deathPanel;
        RichTextLabel statsLabel;
        RandomNumberGenerator rng;
        Timer enemySpawnTimer;
        PackedScene enemyScene;
        PackedScene particleScene;
        Area2D enemyKillArea;
        Crosshair crosshair;
        PlayerData pd;
        AudioStream backgroundMusic;
        AudioStream shootSound;
        AudioStream deathStream;
        AudioStream enemyKillStream;
        AudioStreamPlayer musicPlayer;
        AudioStreamPlayer killEnemyPlayer;
        AudioStreamPlayer shootStreamPlayer;
        AudioStreamPlayer deathStreamPlayer;
        public override void _Ready()
        {
            pd = GetNode<PlayerData>("/root/PD");
            rng = new RandomNumberGenerator();
            hoveredEntity = null;
            particleScene = (PackedScene)ResourceLoader.Load("res://Scenes/EnemyDeathParticle.tscn");
            enemyScene = (PackedScene)ResourceLoader.Load("res://Scenes/Enemy.tscn");
            statsLabel = GetNode<RichTextLabel>("GUI/StatsLabel");
            deathPanel = (DeathPanel)GetNode<Panel>("DeathPanel");
            deathPanel.gameController = this;
            enemyMediumSprites = new Texture[2];
            enemyMediumSprites[0] = (Texture)GD.Load("res://Sprites/space_ship_1.png");
            enemyMediumSprites[1] = (Texture)GD.Load("res://Sprites/space_ship_2.png");
            SetupAudio();
            SetupCrosshair();
            SetupKillArea();
            CheckDifficulty();
            CreateEnemySpawnTimer();
            Input.SetMouseMode(Input.MouseMode.Captured);
            musicPlayer.Play();
            rng.Randomize();
        }
        public override void _PhysicsProcess(float delta)
        {
            statsLabel.Text = ("Points: " + points + "\nLives: " + lives + "\nBullets: " + bullets);
            InputHandler();
        }

        void InputHandler()
        {
            if (Input.IsActionJustPressed("fire") && hoveredEntity != null && bullets > 0)
            {
                CPUParticles2D parts = (CPUParticles2D)particleScene.Instance();
                killEnemyPlayer.Play();
                shootStreamPlayer.Play();
                parts.Position = crosshair.Position;
                AddChild(parts);
                parts.Emitting = true;
                hoveredEntity.Kill();
                hoveredEntity = null;
                points++;
            } else if (Input.IsActionJustPressed("fire") && hoveredEntity == null && bullets > 0)
            {
                shootStreamPlayer.Play();
                if (pd.diff != PlayerData.Difficulty.EASY)
                {
                    bullets--;
                }
            }
            if (Input.IsActionJustPressed("ui_cancel"))
            {
                HandleDeath();
            }
        }

        void HandleDeath()
        {
            musicPlayer.Stop();
            Input.SetMouseMode(Input.MouseMode.Visible);
            deathPanel.Show();
            deathPanel.SetStatsLabel(points, lives, bullets);
            statsLabel.Hide();
            enemySpawnTimer.Stop();
            GetTree().CallGroup("Enemies", "Kill");
        }

        void CreateEnemySpawnTimer()
        {
            enemySpawnTimer = new Timer();
            enemySpawnTimer.Name = "EnemySpawnTimer";
            enemySpawnTimer.PauseMode = PauseModeEnum.Stop;
            enemySpawnTimer.ProcessMode = Timer.TimerProcessMode.Physics;
            enemySpawnTimer.WaitTime = spawnTime;
            AddChild(enemySpawnTimer);
            enemySpawnTimer.Connect("timeout", this, "_on_SpawnTimer_timeout");
            enemySpawnTimer.Start(spawnTime);
        }

        void SpawnEnemy()
        {   
            Enemy enem;
            switch (pd.diff)
            {
                case PlayerData.Difficulty.EASY:
                    enem = GetEasyEnemy();
                    break;
                case PlayerData.Difficulty.NORMAL:
                    enem = GetNormalEnemy();
                    break;
                case PlayerData.Difficulty.HARD:
                    enem = GetHardEnemy();
                    break;
                case PlayerData.Difficulty.ONESHOT:
                    enem = GetEasyEnemy();
                    break;
                default:
                    enem = GetEasyEnemy();
                    break;
            }
            enem.gameController = this;
            AddChild(enem);
            ChangeSpawnTime();
        }

        Enemy GetEasyEnemy()
        {
            Enemy enem = (Enemy)enemyScene.Instance();
            int shipSize = rng.RandiRange(1, 2);
            int spriteIndex = rng.RandiRange(0, 1);
            switch (shipSize)
            {
                case 0:
                    enem.sprite = enemyMediumSprites[spriteIndex];
                    enem.Scale = new Vector2(1.25f, 1.25f);
                break;
                case 1:
                    enem.sprite = enemyMediumSprites[spriteIndex];
                    enem.Scale = new Vector2(1.75f, 1.75f);
                break;
                case 2:
                    enem.sprite = enemyMediumSprites[spriteIndex];
                    enem.Scale = new Vector2(1.5f, 1.5f);
                break;
            }    
            float posX = rng.Randf() * (1024-(32 * enem.Scale.x));
            if (posX < (64/2) * enem.Scale.x)
            {
                posX = (64/2) * enem.Scale.x;
            }
            float posY = rng.Randf() * -64;
            enem.Position = new Vector2(posX, posY);
            return enem;
        }

        Enemy GetNormalEnemy()
        {
            Enemy enem = (Enemy)enemyScene.Instance();
            int shipSize = rng.RandiRange(0, 1);
            int spriteIndex = rng.RandiRange(0, 1);
            switch (shipSize)
            {
                case 0:
                    enem.sprite = enemyMediumSprites[spriteIndex];
                    enem.Scale = new Vector2(1f, 1f);
                break;
                
                case 1:
                    enem.sprite = enemyMediumSprites[spriteIndex];
                    enem.Scale = new Vector2(0.75f, 0.75f);
                break;

                case 2:
                    enem.sprite = enemyMediumSprites[spriteIndex];
                    enem.Scale = new Vector2(1.25f, 1.25f);
                break;

                default:
                    enem.sprite = enemyMediumSprites[spriteIndex];
                    enem.Scale = new Vector2(1f, 1f);
                    GD.Print("Default being called - ShipSize: "+shipSize);
                break;
            }    
            float posX = rng.Randf() * (1024-(32 * enem.Scale.x));
            if (posX < (64/2) * enem.Scale.x)
            {
                posX = (64/2) * enem.Scale.x;
            }
            float posY = rng.Randf() * -64;
            enem.Position = new Vector2(posX, posY);
            return enem;
        }

        Enemy GetHardEnemy()
        {
            Enemy enem = (Enemy)enemyScene.Instance();
            int shipSize = rng.RandiRange(0, 2);
            int spriteIndex = rng.RandiRange(0, 1);
            switch (shipSize)
            {
                case 0:
                    enem.sprite = enemyMediumSprites[spriteIndex];
                    enem.Scale = new Vector2(1f, 1f);
                break;
                case 1:
                    enem.sprite = enemyMediumSprites[spriteIndex];
                    enem.Scale = new Vector2(0.75f, 0.75f);
                break;
                case 2:
                    enem.sprite = enemyMediumSprites[spriteIndex];
                    enem.Scale = new Vector2(0.875f, 0.875f);
                break;
                default:
                    enem.sprite = enemyMediumSprites[spriteIndex];
                    enem.Scale = new Vector2(1f, 1f);
                    GD.Print("Default being called - ShipSize: "+shipSize);
                break;
            }    
            float posX = rng.Randf() * (1024-(32 * enem.Scale.x));
            if (posX < (64/2) * enem.Scale.x)
            {
                posX = (64/2) * enem.Scale.x;
            }
            float posY = rng.Randf() * -64;
            enem.Position = new Vector2(posX, posY);
            return enem;
        }
        
        void CheckDifficulty()
        {
            points = 0;
            switch (pd.diff)
            {
                case PlayerData.Difficulty.EASY:
                    points = 0;
                    lives = 3;
                    bullets = 1;
                    spawnTime = 1f; //Time it takes for enemies to spawn at game start
                    spawnTimeChange = 0.005f; //Amount of time the timer decreases after an enemy spawns
                    spawnTimeLimit = 0.5f; //Lowest the spawn time can be
                break;
                case PlayerData.Difficulty.NORMAL:
                    points = 0;
                    lives = 3;
                    bullets = 6;
                    spawnTime = 0.8f;
                    spawnTimeChange = 0.01f;
                    spawnTimeLimit = 0.42f;
                break;
                case PlayerData.Difficulty.HARD:
                    points = 0;
                    lives = 2;
                    bullets = 3;
                    spawnTime = 0.75f;
                    spawnTimeChange = 0.015f;
                    spawnTimeLimit = 0.325f;
                break;
                case PlayerData.Difficulty.ONESHOT:
                    points = 0;
                    lives = 1;
                    bullets = 1;
                    spawnTime = 0.605f;
                    spawnTimeChange = 0.021f;
                    spawnTimeLimit = 0.3f;
                    //spawnTimeLimit = 0.21f;
                break;
            }
        }

        void ChangeSpawnTime()
        {
            if (spawnTime > spawnTimeLimit)
            {
                spawnTime -= spawnTimeChange;
                enemySpawnTimer.WaitTime = spawnTime;
            }
        }

        void SetupAudio()
        {
            backgroundMusic = (AudioStream)GD.Load("res://Audio/ascending.mp3");
            shootSound = (AudioStream)GD.Load("res://Audio/shoot_sound.wav");
            enemyKillStream = (AudioStream)GD.Load("res://Audio/enemy_kill_1.wav");
            deathStream = (AudioStream)GD.Load("res://Audio/gameover.wav");
            shootStreamPlayer = new AudioStreamPlayer();
            killEnemyPlayer = new AudioStreamPlayer();
            deathStreamPlayer = new AudioStreamPlayer();
            musicPlayer = new AudioStreamPlayer();
            musicPlayer.VolumeDb = pd.musicVolume;
            killEnemyPlayer.VolumeDb = pd.killVolume;
            shootStreamPlayer.VolumeDb = pd.shootVolume;
            deathStreamPlayer.VolumeDb = pd.gameoverVolume;
            musicPlayer.Stream = backgroundMusic;
            killEnemyPlayer.Stream = enemyKillStream;
            shootStreamPlayer.Stream = shootSound;
            deathStreamPlayer.Stream = deathStream;
            AddChild(killEnemyPlayer);
            AddChild(shootStreamPlayer);
            AddChild(deathStreamPlayer);
            AddChild(musicPlayer);
        }

        void SetupKillArea()
        {
            enemyKillArea = GetNode<Area2D>("EnemyKillArea");
            enemyKillArea.Connect("area_entered", this, "_on_EnemyKillArea_entered");
        }

        void SetupCrosshair()
        {
            PackedScene c_scene = (PackedScene)ResourceLoader.Load("res://Scenes/Crosshair.tscn");
            crosshair = c_scene.Instance<Crosshair>();
            crosshair.Position = new Vector2(512, 300);
            AddChild(crosshair);
        }

        public void _on_EnemyKillArea_entered(Area2D area)
        {
            if (area.Name.Contains("Enemy"))
            {
                Enemy a = (Enemy)area;
                a.QueueFree();
                lives--;
                if (lives <= 0)
                {
                    HandleDeath();
                    deathStreamPlayer.Play();
                }
            }
        }

        public void _on_SpawnTimer_timeout()
        {
            SpawnEnemy();
        }

        public void Restart()
        {
            CheckDifficulty();
            enemySpawnTimer.Start(spawnTime);
            deathPanel.Hide();
            statsLabel.Show();
            Input.SetMouseMode(Input.MouseMode.Captured);
        }
    }
}
