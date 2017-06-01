﻿using DizGame.Source.Components;
using GameEngine.Source.Components;
using GameEngine.Source.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DizGame.Source.Factories
{
    /// <summary>
    /// A factory for creation HUD related things
    /// </summary>
    public class HudFactory
    {
        private ContentManager Content;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Content"></param>
        public HudFactory(ContentManager Content)
        {
            this.Content = Content;
        }

        /// <summary>
        /// Function to create gaming hud.
        /// </summary>
        /// <param name="healthPosition"></param>
        /// <param name="AmmunitionPosition"></param>
        /// <param name="PlayersRemainingPosition"></param>
        /// <param name="SlotPositions"></param>
        /// <param name="id"> The id of the Entity that the hud is tracking </param>
        public int CreateHud(Vector2 healthPosition, Vector2 AmmunitionPosition, Vector2 PlayersRemainingPosition, List<Vector2> SlotPositions, int id)
        {
            int HudID = ComponentManager.Instance.CreateID();
            SpriteFont font = Content.Load<SpriteFont>("Fonts\\Font");


            HealthComponent health = ComponentManager.Instance.GetEntityComponent<HealthComponent>(id);
            AmmunitionComponent ammo = ComponentManager.Instance.GetEntityComponent<AmmunitionComponent>(id);
            Texture2DComponent slot1 = new Texture2DComponent(Content.Load<Texture2D>("Icons\\squareTest"))
            {
                Scale = new Vector2(0.2f, 0.2f),
            };
            slot1.Position = new Vector2(GameOne.Instance.GraphicsDevice.Viewport.Width / 2 - ((slot1.Width * slot1.Scale.X) / 2), GameOne.Instance.GraphicsDevice.Viewport.Height);

            List<TextComponent> textComponents = new List<TextComponent>
            {
                new TextComponent(health.Health.ToString(), healthPosition, Color.Pink, font, true, Color.WhiteSmoke, true, 0.3f), // health
                new TextComponent(ammo.CurrentAmmoInMag+"/" + ammo.MaxAmmoInMag + " " + " Clips left: " + ammo.AmmountOfActiveMagazines , AmmunitionPosition, Color.DeepPink, font, true, Color.WhiteSmoke, true, 0.3f), // ammo
            };
            List<string> names = new List<string>
            {
                "Health",
                "Ammunition",

            };
            foreach (Vector2 slots in SlotPositions)
            {
            }

            List<IComponent> components = new List<IComponent>
            {
                slot1,
                new TextComponent(names, textComponents),
                new HudComponent{ TrackedEntity = id },
            };
            ComponentManager.Instance.AddAllComponents(HudID, components);

            return HudID;
        }
    }
}
