using Godot;
using System;

namespace TheBadClickyGame
{
    public class BulletCrate : Entity
    {
        //public Game gameController;
        //public float spd;
        //private Vector2 dir;
        public Sprite spriteNode;
        public Texture sprite;
        public override void _Ready()
        {
            this.spd = 270f;
            this.sprite = (Texture)GD.Load("res://Sprites/bullet_crate_placeholder.png");
            //this.dir = new Vector2(0, 1);
            this.spriteNode = GetNode<Sprite>("Sprite");
            if (this.sprite != null)
            {
                this.spriteNode.Texture = this.sprite;
            } else 
            {
                GD.Print("Null Sprite on " + this);
            }
            this.Connect("area_entered", this, "_on_CollisionArea_entered");
            this.Connect("area_exited", this, "_on_CollisionArea_exited");
        }

        // public override void _PhysicsProcess(float delta)
        // {
        //     this.Position = new Vector2(this.Position.x, this.Position.y + (this.spd * delta));
        // }

        // public void Kill()
        // {
        //     this.QueueFree();
        // }

        // public void _on_CollisionArea_entered(Area2D area)
        // {
        //     if (area.Name is "Crosshair")
        //     {
        //         gameController.hoveredEntity = this;
        //     }
        // }

        // public void _on_CollisionArea_exited(Area2D area)
        // {
        //     if (area.Name is "Crosshair")
        //     {
        //         gameController.hoveredEntity = null;
        //     }
        // }
    }
}
