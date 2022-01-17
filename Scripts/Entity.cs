using Godot;
using System;

namespace TheBadClickyGame
{
    public class Entity : Area2D
    {
        public Game gameController;
        public float spd;
        public override void _Ready()
        {
            this.Connect("area_entered", this, "_on_CollisionArea_entered");
            this.Connect("area_exited", this, "_on_CollisionArea_exited");
        }

        public override void _PhysicsProcess(float delta)
        {
            this.Position = new Vector2(this.Position.x, this.Position.y + (this.spd * delta));
        }

        public void Kill()
        {
            this.QueueFree();
        }

        public void _on_CollisionArea_entered(Area2D area)
        {
            if (area.Name is "Crosshair")
            {
                gameController.hoveredEntity = this;
            }
        }

        public void _on_CollisionArea_exited(Area2D area)
        {
            if (area.Name is "Crosshair")
            {
                gameController.hoveredEntity = null;
            }
        }
    }
}
