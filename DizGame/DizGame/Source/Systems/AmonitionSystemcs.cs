using DizGame.Source.Components;
using GameEngine.Source.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizGame.Source.Systems
{
    class AmonitionSystemcs : IObserver<Tuple<object, object>>
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
                       // amo.Magazines.Add(amo.Magazines.Add()
                            }
                }
            }
            else if (ComponentManager.Instance.CheckIfEntityHasComponent<ResourceComponent>(id2))
            {

            }
        }
    }
}
