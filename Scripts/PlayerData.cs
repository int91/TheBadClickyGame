using Godot;
using System;

namespace TheBadClickyGame
{
    public class PlayerData : Node
    {
        public enum Difficulty
        {
            EASY = 0,
            NORMAL = 1,
            HARD = 2,
            ONESHOT = 3
        };
        public Difficulty diff;
        public int diffInt = 0;
        public Vector2 sensitivity= new Vector2(0.4f, 0.4f);
        //bool relativeSens = false;
        //float relativeFormula = 0.58f;
        public float gameoverVolume = -14.0f;
        public float killVolume = -20.0f;
        public float shootVolume = -8.0f;
        public float musicVolume = -10.0f;
        public int[] highScores;

        String _settingsFile = "user://settings.json";
        String _highScoresFile = "user://scores.json";
        public override void _Ready()
        {
            highScores = new int[4];
            highScores[0] = 0;
            highScores[1] = 0;
            highScores[2] = 0;
            highScores[3] = 0;
            diff = Difficulty.EASY;
            //SaveSettings();
            //LoadSettings(); //This is to test
        }

        public int GetCurrentHighscore()
        {
            return highScores[(int)diff];
        }

        public void EndGame(int endScore)
        {
            if (highScores[(int)diff] < endScore)
            {
                highScores[(int)diff] = endScore;
                SaveHighScores();
                //return true;
            }
            //return false;
        }
        Godot.Collections.Dictionary<string, object> Settings()
        {
            return new Godot.Collections.Dictionary<string, object>()
            {
                {"gameoverVolume", gameoverVolume},
                {"killEnemyVolume", killVolume},
                {"shootVolume", shootVolume},
                {"musicVolume", musicVolume},
                {"sensX", sensitivity.x},
                {"sensY", sensitivity.y},
                {"diff", (int)diff}
            };
        }

        Godot.Collections.Dictionary<string, int> HighScores()
        {
            return new Godot.Collections.Dictionary<string, int>()
            {
                {"easyHighscore", highScores[0]},
                {"normalHighscore", highScores[1]},
                {"hardHighscore", highScores[2]},
                {"oneshotHighscore", highScores[3]}
            };
        }

        public void SaveHighScores()
        {
            var saveFile = new File();
            saveFile.Open(_highScoresFile, File.ModeFlags.Write);

            var data = HighScores();

            saveFile.StoreLine(JSON.Print(data));
            saveFile.Close();
        }

        public void SaveSettings()
        {
            var saveFile = new File();
            saveFile.Open(_settingsFile, File.ModeFlags.Write);

            var data = Settings();

            saveFile.StoreLine(JSON.Print(data));
            saveFile.Close();
        }

        public void LoadSettings()
        {
            var saveFile = new File();
            if (!saveFile.FileExists(_settingsFile))
                return;
            
            saveFile.Open(_settingsFile, File.ModeFlags.Read);
            
            var data = new Godot.Collections.Dictionary<string, object>((Godot.Collections.Dictionary)JSON.Parse(saveFile.GetLine()).Result);

            gameoverVolume = (float)data["gameoverVolume"];
            killVolume = (float)data["killEnemyVolume"];
            shootVolume = (float)data["shootVolume"];
            musicVolume = (float)data["musicVolume"];
            sensitivity = new Vector2((float)data["sensX"], (float)data["sensY"]);
            String diffX = data["diff"].ToString();

            switch (diffX)
            {
                case "0":
                    diff = Difficulty.EASY;
                    diffInt = 0;
                break;
                case "1":
                    diff = Difficulty.NORMAL;
                    diffInt = 1;
                break;
                case "2":
                    diff = Difficulty.HARD;
                    diffInt = 2;
                break;
                case "3":
                    diff = Difficulty.ONESHOT;
                    diffInt = 3;
                break;
            }

            saveFile.Close();

        }

        public void LoadHighScores()
        {
            var saveFile = new File();
            if (!saveFile.FileExists(_highScoresFile))
                return;
            
            saveFile.Open(_highScoresFile, File.ModeFlags.Read);
            
            var data = new Godot.Collections.Dictionary<string, int>((Godot.Collections.Dictionary)JSON.Parse(saveFile.GetLine()).Result);

            highScores[0] = data["easyHighscore"];
            highScores[1] = data["normalHighscore"];
            highScores[2] = data["hardHighscore"];
            highScores[3] = data["oneshotHighscore"];

            saveFile.Close();

        }
    }

}