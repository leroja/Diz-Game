using DizGame.Source.Components;
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
        /// Function to create gaming HUD.
        /// </summary>
        /// <param name="healthPosition"></param>
        /// <param name="AmmunitionPosition"></param>
        /// <param name="StaminaPosition"></param>
        /// <param name="id"> The id of the Entity that the HUD is tracking </param>
        public int CreateHud(Vector2 healthPosition, Vector2 AmmunitionPosition, Vector2 StaminaPosition, int id)
        {
            int HudID = ComponentManager.Instance.CreateID();
            var font = Content.Load<SpriteFont>("Fonts\\Font");


            var health = ComponentManager.Instance.GetEntityComponent<HealthComponent>(id);
            var ammo = ComponentManager.Instance.GetEntityComponent<AmmunitionComponent>(id);
            var stamina = ComponentManager.Instance.GetEntityComponent<StaminaComponent>(id);
            var slot2 = new Texture2DComponent(Content.Load<Texture2D>("Weapons\\AK47FPS"))
            {
                Scale = new Vector2(0.7f, 0.7f),
            };
            slot2.Position = new Vector2(GameOne.Instance.GraphicsDevice.Viewport.Width / 2 - ((slot2.Width * slot2.Scale.X) / 2) + 80, GameOne.Instance.GraphicsDevice.Viewport.Height);

            var textComponents = new List<TextComponent>
            {
                new TextComponent(health.Health.ToString(), healthPosition, Color.Pink, font, true, Color.WhiteSmoke, true, 0.3f), // health
                new TextComponent(ammo.CurrentAmmoInMag+"/" + ammo.MaxAmmoInMag + " " + " Clips left: " + ammo.AmmountOfActiveMagazines , AmmunitionPosition, Color.DeepPink, font, true, Color.WhiteSmoke, true, 0.3f), // ammo
                new TextComponent(stamina.CurrentStamina.ToString(), StaminaPosition, Color.Green, font, true, Color.WhiteSmoke, true, 0.3f),
            };
            List<string> names = new List<string>
            {
                "Health",
                "Ammunition",
                "Stamina",
            };
            List<IComponent> components = new List<IComponent>
            {
                slot2,
                new TextComponent(names, textComponents),
                new HudComponent{ TrackedEntity = id },
            };
            ComponentManager.Instance.AddAllComponents(HudID, components);

            return HudID;
        }
    }
}