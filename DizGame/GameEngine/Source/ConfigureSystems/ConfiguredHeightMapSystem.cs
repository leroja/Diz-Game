using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Systems;
using GameEngine;
using GameEngine.Source.Objects;

namespace GameEngine.Source.ConfigureSystems
{
    public class ConfiguredHeightMapSystem
    {
        private static readonly ConfiguredHeightMapSystem Instance = new ConfiguredHeightMapSystem();

        private HeightMapSystem  hmSystem { get; set; }

        private ConfiguredHeightMapSystem()
        {
            //hmSystem = new HeightMapSystem();

        }

        private void /*HeightMapObject*/ generateHeighMapObjects()
        {

        }
    }
}
