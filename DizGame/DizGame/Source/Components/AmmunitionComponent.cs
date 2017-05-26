using DizGame.Source.Enums;
using GameEngine.Source.Components;
using System;
using System.Collections.Generic;

namespace DizGame.Source.Components
{
    /// <summary>
    /// Component that contains positions for objects in hud.
    /// </summary>
    public class AmmunitionComponent : IComponent
    {
        /// <summary>
        /// Dictionary for used as current magazines.
        /// Dictionary(AmmunitionType, List(Tuple(int, int))
        /// </summary>
        //public Dictionary<AmmunitionType, List<Tuple<int, int>>> Magazines;
        /// <summary>
        /// Tuple which is used as Tuple(AmmunitionType, Bullets left, MagasineSize)
        /// </summary>
       // public Tuple<AmmunitionType, int, int> ActiveMagazine { get; set; }
        /// <summary>
        /// Total amount of Magazines of the current ActiveMagazine.
        /// </summary>
        public int AmmountOfActiveMagazines { get; set; }
        /// <summary>
        /// Constructor which creates the Dictionary
        /// and sets attributes to default values.
        /// </summary>
        /// 

        public int MaxAmoInMag { get; set; }
        public int curentAmoInMag { get; set; }
        public AmmunitionComponent()
        {
            //Magazines = new Dictionary<AmmunitionType, List<Tuple<int, int>>>();
            //ActiveMagazine = new Tuple<AmmunitionType, int, int>(AmmunitionType.None, 0, 0);
            MaxAmoInMag = 30;
            curentAmoInMag = 30;
            AmmountOfActiveMagazines = 0;
        }

        

    }
}
