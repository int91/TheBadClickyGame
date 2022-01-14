using Godot;
using System;

namespace TheBadClickyGame
{
    public class Crosshair : Area2D
    {
        PlayerData pd;
        Vector2 sensitivity;
        public override void _Ready()
        {
            pd = GetNode<PlayerData>("/root/PD");
            sensitivity = pd.sensitivity;
        }
        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouseMotion iemm)
            {
                this.Position += iemm.Relative * sensitivity;
            }
        }
    }
}
