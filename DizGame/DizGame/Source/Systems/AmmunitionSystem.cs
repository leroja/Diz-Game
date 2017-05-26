using DizGame.Source.Components;
using GameEngine.Source.Managers;
using GameEngine.Source.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DizGame.Source.Systems
{
    class AmmunitionSystem : IUpdate ,IObserver<Tuple<object, object>>
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Funktion called from Observable class. used for kollisions
        /// </summary>
        /// <param name="value"></param>
        public void OnNext(Tuple<object, object> value)
        {
            int id1 = (int)value.Item1;
            int id2 = (int)value.Item2;

            if (ComponentManager.Instance.CheckIfEntityHasComponent<ResourceComponent>(id1))
            {
                var res = ComponentManager.Instance.GetEntityComponent<ResourceComponent>(id1);
                if (res.thisType == ResourceComponent.ResourceType.Ammo)
                {
                    if (ComponentManager.Instance.CheckIfEntityHasComponent<AmmunitionComponent>(id2)) {
                        var amo = ComponentManager.Instance.GetEntityComponent<AmmunitionComponent>(id2);
                        ComponentManager.Instance.RemoveEntity(id1);
                        ComponentManager.Instance.RecycleID(id1);
                        amo.AmmountOfActiveMagazines++;
                            }
                }
            }
            else if (ComponentManager.Instance.CheckIfEntityHasComponent<ResourceComponent>(id2))
            {
                var res = ComponentManager.Instance.GetEntityComponent<ResourceComponent>(id2);
                if (res.thisType == ResourceComponent.ResourceType.Ammo)
                {
                    if (ComponentManager.Instance.CheckIfEntityHasComponent<AmmunitionComponent>(id1))
                    {
                        var amo = ComponentManager.Instance.GetEntityComponent<AmmunitionComponent>(id1);
                        ComponentManager.Instance.RemoveEntity(id2);
                        ComponentManager.Instance.RecycleID(id2);
                        amo.AmmountOfActiveMagazines++;
                    }
                }

            }
        }

        public override void Update(GameTime gameTime)
        {
            var list = ComponentManager.Instance.GetAllEntitiesWithComponentType<AmmunitionComponent>();
            foreach (var ammoid in list)
            {
                var ammocomp = ComponentManager.Instance.GetEntityComponent<AmmunitionComponent>(ammoid);
                if(ammocomp.curentAmoInMag <= 0)
                {
                    if(ammocomp.AmmountOfActiveMagazines > 0)
                    {
                        ammocomp.curentAmoInMag = ammocomp.MaxAmoInMag;
                        ammocomp.AmmountOfActiveMagazines--;
                    }
                }
            }
        }
    }
}
