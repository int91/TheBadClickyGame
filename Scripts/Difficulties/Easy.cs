using Godot;
using System;

namespace TheBadClickyGame
{
    public class Easy : Difficulty
    {

        public Easy(Texture[] smallSprites, Texture[] medSprites, Texture[] largeSprites, Texture[] xLargeSprites) : base (smallSprites, medSprites, largeSprites, xLargeSprites)
        {
            this._position = 0;
        }

        public override Enemy SpawnEnemy()
        {
            Enemy enem = (Enemy)GetEnemyScene().Instance();
            int shipSize = _rng.RandiRange(0, 2);
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
            }    
            float posX = _rng.Randf() * (1024-(32 * enem.Scale.x));
            if (posX < (64/2) * enem.Scale.x)
            {
                posX = (64/2) * enem.Scale.x;
            }
            float posY = _rng.Randf() * -64;
            enem.Position = new Vector2(posX, posY);
            GD.Print("I'm returning properly");
            //return enem;
            return base.SpawnEnemy();
        }
    }
}