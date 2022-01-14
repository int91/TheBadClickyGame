using Godot;
using System;

namespace TheBadClickyGame
{
    public class PlayerData : Node
    {
        public enum Difficulty
        {
            EASY,
            NORMAL,
            HARD,
            ONESHOT
        };
        public Difficulty diff;
        public Vector2 sensitivity= new Vector2(0.4f, 0.4f);
        bool relativeSens = false;
        float relativeFormula = 0.58f;
        public float gameoverVolume = -14.0f;
        public float killVolume = -20.0f;
        public float shootVolume = -8.0f;
        public float musicVolume = -10.0f;
        public override void _Ready()
        {
            diff = Difficulty.EASY;
        }
    }
}