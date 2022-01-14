using Godot;
using System;

namespace TheBadClickyGame
{
    public class MainMenu : Control
    {
        OptionButton difficultyButton;
        RichTextLabel xSens;
        RichTextLabel ySens;
        Button playButton;
        Button settingsButton;
        Button closeSettingsButton;
        Button quitButton;
        Panel settingsPanel;
        Panel menuPanel;
        HSlider xSensSlider;
        HSlider ySensSlider;
        HSlider killSlider;
        HSlider shootSlider;
        HSlider deathSlider;
        HSlider musicSlider;
        PlayerData pd;

        AudioStream menuMusicStream;
        AudioStreamPlayer musicPlayer;
        public override void _Ready()
        {
            pd = GetNode<PlayerData>("/root/PD");
            menuPanel = GetNode<Panel>("Panel/MainMenuPanel");
            playButton = GetNode<Button>("Panel/MainMenuPanel/ButtonContainer/Button");
            settingsButton = GetNode<Button>("Panel/MainMenuPanel/ButtonContainer/Button2");
            quitButton = GetNode<Button>("Panel/MainMenuPanel/ButtonContainer/Button3");
            difficultyButton = GetNode<OptionButton>("Panel/MainMenuPanel/DifficultyButton");
            closeSettingsButton = GetNode<Button>("Panel/SettingPanel/CloseButton");
            xSensSlider = GetNode<HSlider>("Panel/SettingPanel/HorizontalSensSlider");
            ySensSlider = GetNode<HSlider>("Panel/SettingPanel/VerticalSensSlider");
            settingsPanel = GetNode<Panel>("Panel/SettingPanel");
            xSens = GetNode<RichTextLabel>("Panel/SettingPanel/RichTextLabel2");
            ySens = GetNode<RichTextLabel>("Panel/SettingPanel/RichTextLabel3");
            killSlider = GetNode<HSlider>("Panel/SettingPanel/KillVolumeSlider");
            shootSlider = GetNode<HSlider>("Panel/SettingPanel/ShootVolumeSlider");
            deathSlider = GetNode<HSlider>("Panel/SettingPanel/DeathVolumeSlider");
            musicSlider = GetNode<HSlider>("Panel/SettingPanel/MusicVolumeSlider");

            menuMusicStream = (AudioStream)GD.Load("res://Audio/8_bit_menu.mp3");

            musicPlayer = new AudioStreamPlayer();
            musicPlayer.Stream = menuMusicStream;
            AddChild(musicPlayer);

            playButton.Connect("pressed", this, "_on_PlayButton_pressed");
            settingsButton.Connect("pressed", this, "_on_SettingsButton_pressed");
            quitButton.Connect("pressed", this, "_on_QuitButton_pressed");
            difficultyButton.Connect("item_selected", this, "_on_DifficultyButton_item_selected");
            closeSettingsButton.Connect("pressed", this, "on_CloseSettingsButton_pressed");
            xSensSlider.Connect("value_changed", this, "on_xSensSlider_value_changed");
            ySensSlider.Connect("value_changed", this, "on_ySensSlider_value_changed");
            killSlider.Connect("value_changed", this, "_on_KillSlider_value_changed");
            shootSlider.Connect("value_changed", this, "_on_ShootSlider_value_changed");
            deathSlider.Connect("value_changed", this, "_on_DeathSlider_value_changed");
            musicSlider.Connect("value_changed", this, "_on_MusicSlider_value_changed");

            xSensSlider.Value = pd.sensitivity.x;
            ySensSlider.Value = pd.sensitivity.y;
            xSens.Text = "X: " + pd.sensitivity.x;
            ySens.Text = "Y: " + pd.sensitivity.y;
            difficultyButton.Select((int)pd.diff);
            killSlider.Value = pd.killVolume;
            shootSlider.Value = pd.shootVolume;
            deathSlider.Value = pd.gameoverVolume;
            musicSlider.Value = pd.musicVolume;

            musicPlayer.Play();
        }
        public override void _Process(float delta)
        {
            
        }

        public void on_CloseSettingsButton_pressed()
        {
            menuPanel.Show();
            settingsPanel.Hide();
        }

        public void _on_MusicSlider_value_changed(float value)
        {
            pd.musicVolume = value;
            musicPlayer.VolumeDb = value;
        }

        public void _on_KillSlider_value_changed(float value)
        {
            pd.killVolume = value;
        }

        public void _on_ShootSlider_value_changed(float value)
        {
            pd.shootVolume = value;
        }

        public void _on_DeathSlider_value_changed(float value)
        {
            pd.gameoverVolume = value;
        }

        public void on_xSensSlider_value_changed(float value)
        {
            pd.sensitivity = new Vector2(value, pd.sensitivity.y);
            xSens.Text = "X: " + value;
        }

        public void on_ySensSlider_value_changed(float value)
        {
            pd.sensitivity = new Vector2(pd.sensitivity.x, value);
            ySens.Text = "Y: " + value;
        }

        public void _on_PlayButton_pressed()
        {
            GetTree().ChangeScene("res://Scenes/Game.tscn");
        }

        public void _on_SettingsButton_pressed()
        {
            settingsPanel.Show();
            menuPanel.Hide();
        }

        public void _on_QuitButton_pressed()
        {
            GetTree().Quit();
        }

        public void _on_DifficultyButton_item_selected(int index)
        {
            switch (index)
            {
                case 0:
                    pd.diff = PlayerData.Difficulty.EASY;
                break;
                case 1:
                    pd.diff = PlayerData.Difficulty.NORMAL;
                break;
                case 2:
                    pd.diff = PlayerData.Difficulty.HARD;
                break;
                case 3:
                    pd.diff = PlayerData.Difficulty.ONESHOT;
                break;
            }
        }
    }

}