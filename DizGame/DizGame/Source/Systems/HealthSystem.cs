using GameEngine.Source.Systems;
using Microsoft.Xna.Framework;
using DizGame.Source.Components;
using GameEngine.Source.Components;
using GameEngine.Source.Managers;
using Microsoft.Xna;
using System;
using System.Collections.Generic;

namespace DizGame.Source.Systems
{
    /// <summary>
    ///  20 träff 
    ///  100 kill
    /// </summary>
    public class HealthSystem : IObserver<Tuple<object, object>>
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(Tuple<object, object> value)
        {
            int id1 = (int)value.Item1;
            int id2 = (int)value.Item2;

            if (ComponentManager.Instance.CheckIfEntityHasComponent<ResourceComponent>(id1) || ComponentManager.Instance.CheckIfEntityHasComponent<ResourceComponent>(id2))
            {
                if (ComponentManager.Instance.CheckIfEntityHasComponent<HealthComponent>(id1))
                {
                    PlayerResorceColision(id1, id2);
                }
                else
                {
                    PlayerResorceColision(id2, id1);
                }
            }
            else if (ComponentManager.Instance.CheckIfEntityHasComponent<BulletComponent>(id1) || ComponentManager.Instance.CheckIfEntityHasComponent<BulletComponent>(id2))
            {
                if (ComponentManager.Instance.CheckIfEntityHasComponent<BulletComponent>(id1))
                {
                    BulletPlayerColision(id1, id2);
                }
                else
                {
                    BulletPlayerColision(id2, id1);
                }
            }
        }

        private void PlayerResorceColision(int HelathID, int ResourceID)
        {
            var hel = ComponentManager.Instance.GetEntityComponent<HealthComponent>(HelathID);
            var res = ComponentManager.Instance.GetEntityComponent<ResourceComponent>(ResourceID);
            if (res.thisType == ResourceComponent.ResourceType.Health)
            {
                if (ComponentManager.Instance.CheckIfEntityHasComponent<PlayerComponent>(HelathID) || ComponentManager.Instance.CheckIfEntityHasComponent<AIComponent>(HelathID))
                {
                    if (hel.Health + hel.HealthOnPickup >= 100)
                    {
                        ComponentManager.Instance.GetEntityComponent<HealthComponent>(HelathID).Health = ComponentManager.Instance.GetEntityComponent<HealthComponent>(HelathID).MaxHealth;
                    }
                    else
                    {
                        ComponentManager.Instance.GetEntityComponent<HealthComponent>(HelathID).Health += ComponentManager.Instance.GetEntityComponent<HealthComponent>(HelathID).HealthOnPickup;
                    }
                }
            }
        }

        private void BulletPlayerColision(int BulletID, int HitID)
        {
            var bullet = ComponentManager.Instance.GetEntityComponent<BulletComponent>(BulletID);
            if (ComponentManager.Instance.CheckIfEntityHasComponent<PlayerComponent>(HitID) || ComponentManager.Instance.CheckIfEntityHasComponent<AIComponent>(HitID))
            {
                if (HitID != ComponentManager.Instance.GetEntityComponent<BulletComponent>(BulletID).Owner)
                {
                    var a = ComponentManager.Instance.GetEntityComponent<HealthComponent>(HitID);
                    if (a.Health - bullet.Damage <= 0)
                    {
                        ComponentManager.Instance.GetEntityComponent<HealthComponent>(HitID).Health -= bullet.Damage;
                        if (ComponentManager.Instance.CheckIfEntityHasComponent<PlayerComponent>(HitID))
                        {
                            var comp = ComponentManager.Instance.GetEntityComponent<PlayerComponent>(HitID);
                            ComponentManager.Instance.RemoveComponentFromEntity(HitID, comp);
                        }
                        else if (ComponentManager.Instance.CheckIfEntityHasComponent<AIComponent>(HitID))
                        {
                            var comp = ComponentManager.Instance.GetEntityComponent<AIComponent>(HitID);
                            ComponentManager.Instance.RemoveComponentFromEntity(HitID, comp);
                        }
                        ComponentManager.Instance.RemoveComponentFromEntity(HittID, ComponentManager.Instance.GetEntityComponent<ModelComponent>(HittID));
                        
                        ComponentManager.Instance.GetEntityComponent<ScoreComponent>(ComponentManager.Instance.GetEntityComponent<BulletComponent>(BulletID).Owner).Score += 100;

                    }
                    else
                    {
                        ComponentManager.Instance.GetEntityComponent<HealthComponent>(HitID).Health -= bullet.Damage;
                        var score = ComponentManager.Instance.GetEntityComponent<ScoreComponent>(bullet.Owner);
                        score.Score += 5;
                    }
                }
            }
        }
    }
}
