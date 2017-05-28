﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DizGame.Source.Enums;

namespace DizGame.Source.Settings
{
    /// <summary>
    /// These are the pre-defined games settings for the client to choose from.
    /// </summary>
    public class GameSettings
    {
        private static readonly GameSettings instance = new GameSettings();

        private static Dictionary<GameSettingsType, Dictionary<GameSettingsType, string>> settings;

        private GameSettings()
        {
            settings = new Dictionary<GameSettingsType, Dictionary<GameSettingsType, string>>();

            InitialiseGameSettings0();
        }


        /// <summary>
        /// This class is used for getting a specific gameSetting.
        /// </summary>
        /// <param name="gameSettingsNumber">The number of the gameSetting to retrieve.</param>
        /// <param name="name">Which type of gameSetting to retrieve.</param>
        /// <returns>The gameSetting retrieved.</returns>
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
