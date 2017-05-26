using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizGame.Source.Enums
{
    public enum GameSettingsType : byte
    {
        GameSettings0=10,
        GameSettings1,
        GameSettings2,
        GameSettings3,
        GameSettings4,
        GameSettings5,
        GameSettings6,
        GameSettings7,

        //These are needed by the client to set variables for the game in advance.
        HeightMapName,
        HeightMapTexture,
        HeightMapChunksPerSide,

        CountOfHouses,
        CountOfStaticObjects,

        PlayerEntityId,

    }
}
