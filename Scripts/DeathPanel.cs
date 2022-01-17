using Godot;
using System;

namespace TheBadClickyGame
{
    public class DeathPanel : Panel
    {
        public Game gameController;
        PlayerData pd;
        RichTextLabel statsLabel;
        Button restartButton;
        Button mainMenuButton;
        public override void _Ready()
        {
            pd = GetNode<PlayerData>("/root/PD");
            statsLabel = GetNode<RichTextLabel>("StatsLabel");
            restartButton = GetNode<Button>("ButtonRestart");
            mainMenuButton = GetNode<Button>("ButtonMainMenu");

            restartButton.Connect("pressed", this, "on_RestartButton_pressed");
            mainMenuButton.Connect("pressed", this, "on_MainMenuButton_pressed");
        }

        public void SetStatsLabel(int p, int l, int b)
        {
            if (p > pd.GetCurrentHighscore())
            {
                statsLabel.BbcodeText = ("[center]" + "NEW HIGHSCORE\nPoints: " + p + "\nHighscore: " + pd.GetCurrentHighscore() + "\nLives: " + l + "\nBullets: " + b + "[/center]");
            } else 
            {
                statsLabel.BbcodeText = ("[center]" + "Points: " + p + "\nHighscore: " + pd.GetCurrentHighscore() + "\nLives: " + l + "\nBullets: " + b + "[/center]");
            }
            
        }

        public void on_RestartButton_pressed()
        {
            gameController.Restart();
        }

        public void on_MainMenuButton_pressed()
        {
            GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
        }

    }

}