using Microsoft.Xna.Framework;
using DizGame.Source.Components;
using GameEngine.Source.Components;
using GameEngine.Source.Managers;
using System;
using Microsoft.Xna.Framework.Graphics;
using DizGame.Source.Factories;

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
                    ComponentManager.Instance.RemoveEntity(id1);
                    ComponentManager.Instance.RecycleID(id1);
                }
                else
                {
                    BulletPlayerColision(id2, id1);
                    ComponentManager.Instance.RemoveEntity(id2);
                    ComponentManager.Instance.RecycleID(id2);
                }
            }
        }

        /// <summary>
        /// Logic for handling Resource/player and resource/AI collisions
        /// </summary>
        /// <param name="HealthID"> ID of Player/AI to pick up Resource</param>
        /// <param name="ResourceID"> ID of the resource </param>

        private void PlayerResorceColision(int HealthID, int ResourceID)
        {
            var hel = ComponentManager.Instance.GetEntityComponent<HealthComponent>(HealthID);
            var res = ComponentManager.Instance.GetEntityComponent<ResourceComponent>(ResourceID);
            if (res != null)
            {
                if (res.thisType == ResourceType.Health)
                {
                    if (ComponentManager.Instance.CheckIfEntityHasComponent<PlayerComponent>(HealthID) || ComponentManager.Instance.CheckIfEntityHasComponent<AIComponent>(HealthID))
                    {
                        if (hel.Health + hel.HealthOnPickup >= 100)
                        {
                            ComponentManager.Instance.GetEntityComponent<HealthComponent>(HealthID).Health = ComponentManager.Instance.GetEntityComponent<HealthComponent>(HealthID).MaxHealth;
                            ComponentManager.Instance.RemoveEntity(ResourceID);
                            ComponentManager.Instance.RecycleID(ResourceID);

                        }
                        else
                        {
                            ComponentManager.Instance.GetEntityComponent<HealthComponent>(HealthID).Health += ComponentManager.Instance.GetEntityComponent<HealthComponent>(HealthID).HealthOnPickup;
                            ComponentManager.Instance.RemoveEntity(ResourceID);
                            ComponentManager.Instance.RecycleID(ResourceID);
                        }
                        var sound = ComponentManager.Instance.GetEntityComponent<_3DSoundEffectComponent>(HealthID);
                        sound.SoundEffectsToBePlayed.Add(Tuple.Create("HealthPickUp", 1f));
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
                            ComponentManager.Instance.AddComponentToEntity(HitID, new SpectatingComponent());
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

                        var sound = ComponentManager.Instance.GetEntityComponent<SoundEffectComponent>(ComponentManager.Instance.GetEntityComponent<BulletComponent>(BulletID).Owner);
                        sound.SoundEffectsToBePlayed.Add(Tuple.Create("Kill-Sound", 0f, 1f));
                        var _3Dsound = ComponentManager.Instance.GetEntityComponent<_3DSoundEffectComponent>(HitID);
                        _3Dsound.SoundEffectsToBePlayed.Add(Tuple.Create("DeathSound", 1f));
                    }
                    else
                    {
                        ComponentManager.Instance.GetEntityComponent<HealthComponent>(HitID).Health -= bullet.Damage;
                        var score = ComponentManager.Instance.GetEntityComponent<ScoreComponent>(bullet.Owner);
                        var t = ComponentManager.Instance.GetEntityComponent<TransformComponent>(BulletID);
                        Vector3 pos = new Vector3(t.Position.X, t.Position.Y + 1, t.Position.Z);
                        EntityFactory.Instance.CreateParticleEmitter(pos, "Blood", 5);
                        score.Hits += 1;
                        score.Score += 5;
                    }
                }
            }
        }
    }
}