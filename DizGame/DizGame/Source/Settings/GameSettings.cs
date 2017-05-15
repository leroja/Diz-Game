using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DizGame.Source.Enums;

namespace DizGame.Source.Settings
{
    public class GameSettings
    {
        private static readonly GameSettings instance = new GameSettings();

        private static Dictionary<GameSettingsType, Dictionary<GameSettingsType, string>> settings;

        private GameSettings()
        {
            settings = new Dictionary<GameSettingsType, Dictionary<GameSettingsType, string>>();

            InitialiseGameSettings0();
        }


        public static string GetGameSettings(GameSettingsType gameSettingsNumber, GameSettingsType name)
        {
            if (settings.ContainsKey(gameSettingsNumber))
                if (settings[gameSettingsNumber].ContainsKey(name))
                    return settings[gameSettingsNumber][name];

            return "";
        }

        private void InitialiseGameSettings0()
        {
            GameSettingsType type = GameSettingsType.GameSettings0;

            if (!settings.ContainsKey(type))
                settings.Add(type, new Dictionary<GameSettingsType, string>());

            settings[type].Add(GameSettingsType.HeightMapName, "canyonHeightMap");
            settings[type].Add(GameSettingsType.HeightMapTexture, "BetterGrass");
            settings[type].Add(GameSettingsType.HeightMapChunksPerSide, "10");
            settings[type].Add(GameSettingsType.CountOfHouses, "10");
            settings[type].Add(GameSettingsType.CountOfStaticObjects, "100");
        }
    }
}
