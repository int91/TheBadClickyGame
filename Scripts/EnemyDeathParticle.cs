using Godot;
using System;

namespace TheBadClickyGame
{
    public class EnemyDeathParticle : CPUParticles2D
    {
        Timer lifeTimer;
        public override void _Ready()
        {
            lifeTimer = new Timer();
            lifeTimer.WaitTime = 0.35f;
            lifeTimer.ProcessMode = Timer.TimerProcessMode.Physics;
            lifeTimer.OneShot = true;
            lifeTimer.Name = "LifeTimer";
            AddChild(lifeTimer);
            lifeTimer.Connect("timeout", this, "on_lifeTimer_timeout");
            lifeTimer.Start();
        }

        public void on_lifeTimer_timeout()
        {
            this.QueueFree();
        }
    }

}