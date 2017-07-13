using GameEngine.Source.Components;

namespace DizGame.Source.Components
{
    /// <summary>
    /// Component that contains positions for objects in HUD.
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
        //public Tuple<AmmunitionType, int, int> ActiveMagazine { get; set; }
        /// <summary>
        /// Total amount of Magazines of the current ActiveMagazine.
        /// </summary>
        public int AmmountOfActiveMagazines { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int MaxAmmoInMag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int CurrentAmmoInMag { get; set; }

        /// <summary>
        /// Constructor which creates the Dictionary
        /// and sets attributes to default values.
        /// </summary>
        public AmmunitionComponent()
        {
            //Magazines = new Dictionary<AmmunitionType, List<Tuple<int, int>>>();
            //ActiveMagazine = new Tuple<AmmunitionType, int, int>(AmmunitionType.None, 0, 0);
            MaxAmmoInMag = 30;
            CurrentAmmoInMag = 30;
            AmmountOfActiveMagazines = 0;
        }
    }
}
