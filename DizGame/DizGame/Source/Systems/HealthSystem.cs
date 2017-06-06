using Microsoft.Xna.Framework;
using DizGame.Source.Components;
using GameEngine.Source.Components;
using GameEngine.Source.Managers;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace DizGame.Source.Systems
{
    /// <summary>
    ///  Default score for hit =  5 
    ///  Default score for kill = 100
    /// </summary>
    public class HealthSystem : IObserver<Tuple<object, object>>
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

            var temp = ComponentManager.Instance.GetAllEntityComponents(id1);
            var temp2 = ComponentManager.Instance.GetAllEntityComponents(id2);
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

        /// <summary>
        /// Logic for handling Resource/player and resource/AI collisions
        /// </summary>
        /// <param name="HelathID"> ID of Player/AI to pick up Resource</param>
        /// <param name="ResourceID"> ID of the resource </param>

        private void PlayerResorceColision(int HelathID, int ResourceID)
        {
            var hel = ComponentManager.Instance.GetEntityComponent<HealthComponent>(HelathID);
            var res = ComponentManager.Instance.GetEntityComponent<ResourceComponent>(ResourceID);
            if (res != null)
            {
                if (res.thisType == ResourceComponent.ResourceType.Health)
                {
                    if (ComponentManager.Instance.CheckIfEntityHasComponent<PlayerComponent>(HelathID) || ComponentManager.Instance.CheckIfEntityHasComponent<AIComponent>(HelathID))
                    {
                        if (hel.Health + hel.HealthOnPickup >= 100)
                        {
                            ComponentManager.Instance.GetEntityComponent<HealthComponent>(HelathID).Health = ComponentManager.Instance.GetEntityComponent<HealthComponent>(HelathID).MaxHealth;
                            ComponentManager.Instance.RemoveEntity(ResourceID);
                            ComponentManager.Instance.RecycleID(ResourceID);

                        }
                        else
                        {
                            ComponentManager.Instance.GetEntityComponent<HealthComponent>(HelathID).Health += ComponentManager.Instance.GetEntityComponent<HealthComponent>(HelathID).HealthOnPickup;
                            ComponentManager.Instance.RemoveEntity(ResourceID);
                            ComponentManager.Instance.RecycleID(ResourceID);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handle logic for bullet/player and bullet/AI collisions. 
        /// </summary>
        /// <param name="BulletID">ID of bullet.</param>
        /// <param name="HitID">ID of collision target</param>
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
                            ComponentManager.Instance.AddComponentToEntity(HitID, new TextComponent("You Are Dead", new Vector2(GameOne.Instance.GraphicsDevice.Viewport.Width / 3 - 90, GameOne.Instance.GraphicsDevice.Viewport.Height / 2 - 50), Color.Red, GameOne.Instance.Content.Load<SpriteFont>("Fonts/Death"), true));
                        }
                        else if (ComponentManager.Instance.CheckIfEntityHasComponent<AIComponent>(HitID))
                        {
                            var comp = ComponentManager.Instance.GetEntityComponent<AIComponent>(HitID);
                            ComponentManager.Instance.RemoveComponentFromEntity(HitID, comp);
                        }
                        ComponentManager.Instance.RemoveComponentFromEntity(HitID, ComponentManager.Instance.GetEntityComponent<ModelComponent>(HitID));

                        var score = ComponentManager.Instance.GetEntityComponent<ScoreComponent>(ComponentManager.Instance.GetEntityComponent<BulletComponent>(BulletID).Owner);
                        score.Kills += 1;
                        score.Hits += 1;
                        score.Score += 100;
                    }
                    else
                    {
                        ComponentManager.Instance.GetEntityComponent<HealthComponent>(HitID).Health -= bullet.Damage;
                        var score = ComponentManager.Instance.GetEntityComponent<ScoreComponent>(bullet.Owner);
                        score.Hits += 1;
                        score.Score += 5;
                    }
                }
            }
        }
    }
}
