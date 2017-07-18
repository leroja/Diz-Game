using DizGame.Source.Components;
using GameEngine.Source.Managers;
using GameEngine.Source.Systems;
using System;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;

namespace DizGame.Source.Systems
{
    /// <summary>
    /// 
    /// </summary>
    public class AmmunitionSystem : IUpdate, IObserver<Tuple<object, object>>
    {
        /// <summary>
        /// 
        /// </summary>
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Function called from Observable class. used for collisions
        /// </summary>
        /// <param name="value"></param>
        public void OnNext(Tuple<object, object> value)
        {
            int id1 = (int)value.Item1;
            int id2 = (int)value.Item2;

            if (ComponentManager.Instance.CheckIfEntityHasComponent<ResourceComponent>(id1))
            {
                var res = ComponentManager.Instance.GetEntityComponent<ResourceComponent>(id1);
                if (res.thisType == ResourceType.Ammo)
                {
                    if (ComponentManager.Instance.CheckIfEntityHasComponent<AmmunitionComponent>(id2))
                    {
                        var amo = ComponentManager.Instance.GetEntityComponent<AmmunitionComponent>(id2);
                        ComponentManager.Instance.RemoveEntity(id1);
                        ComponentManager.Instance.RecycleID(id1);
                        amo.AmmountOfActiveMagazines++;

                        var sound = ComponentManager.GetEntityComponent<_3DSoundEffectComponent>(id2);
                        sound.SoundEffectsToBePlayed.Add(Tuple.Create("Ammo-Pickup", 1f));
                    }
                }
            }
            else if (ComponentManager.Instance.CheckIfEntityHasComponent<ResourceComponent>(id2))
            {
                var res = ComponentManager.Instance.GetEntityComponent<ResourceComponent>(id2);
                if (res.thisType == ResourceType.Ammo)
                {
                    if (ComponentManager.Instance.CheckIfEntityHasComponent<AmmunitionComponent>(id1))
                    {
                        var amo = ComponentManager.Instance.GetEntityComponent<AmmunitionComponent>(id1);
                        ComponentManager.Instance.RemoveEntity(id2);
                        ComponentManager.Instance.RecycleID(id2);
                        amo.AmmountOfActiveMagazines++;

                        var sound = ComponentManager.GetEntityComponent<_3DSoundEffectComponent>(id1);
                        sound.SoundEffectsToBePlayed.Add(Tuple.Create("Ammo-Pickup", 1f));
                    }
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            var list = ComponentManager.Instance.GetAllEntitiesWithComponentType<AmmunitionComponent>();
            foreach (var ammoID in list)
            {
                var ammoComp = ComponentManager.Instance.GetEntityComponent<AmmunitionComponent>(ammoID);
                if (ammoComp.CurrentAmmoInMag <= 0)
                {
                    if (ammoComp.AmmountOfActiveMagazines > 0)
                    {
                        ammoComp.CurrentAmmoInMag = ammoComp.MaxAmmoInMag;
                        ammoComp.AmmountOfActiveMagazines--;

                        //var sound = ComponentManager.Instance.GetEntityComponent<_3DSoundEffectComponent>(ammoID);
                        //sound.SoundEffectsToBePlayed.Add(Tuple.Create("Reload", 1f));
                    }
                }
            }
        }
    }
}