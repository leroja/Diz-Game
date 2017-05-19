using DizGame.Source.Components;
using DizGame.Source.Enums;
using GameEngine.Source.Components;
using GameEngine.Source.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizGame.Source.Factories
{
    public class HudFactory
    {
        private ContentManager Content;

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
        public int CreateHud(Vector2 healthPosition, Vector2 AmmunitionPosition, Vector2 PlayersRemainingPosition, List<Vector2> SlotPositions)
        {
            int HudID = ComponentManager.Instance.CreateID();
            SpriteFont font = Content.Load<SpriteFont>("Fonts\\Font");

            HealthComponent health = new HealthComponent();
            AmmunitionComponent ammo = new AmmunitionComponent()
            {
                ActiveMagazine = new Tuple<AmmunitionType, int, int>(AmmunitionType.AK_47, 30, Magazine.GetSize(AmmunitionType.AK_47))
            };
            Texture2DComponent slot1 = new Texture2DComponent(Content.Load<Texture2D>("Icons\\squareTest"))
            {
                Scale = new Vector2(0.2f, 0.2f),
            };
            slot1.Position = new Vector2(GameOne.Instance.GraphicsDevice.Viewport.Width / 2 - ((slot1.Width * slot1.Scale.X) / 2), GameOne.Instance.GraphicsDevice.Viewport.Height);


            List<TextComponent> textComponents = new List<TextComponent>
            {
                new TextComponent(health.Health.ToString(), healthPosition, Color.Pink, font, true, Color.WhiteSmoke, true, 0.3f), // health
                new TextComponent(ammo.ActiveMagazine.Item2 + "/" + ammo.ActiveMagazine.Item3 + " " + ammo.ActiveMagazine.Item1 + " Clips left: " + ammo.AmmountOfActiveMagazines , AmmunitionPosition, Color.DeepPink, font, true, Color.WhiteSmoke, true, 0.3f), // ammo
                // TODO: en player remaining vet inte om vi skall göra en komponent för det? :) <- Det är väl bara att plocka ut typ alla "health component" o kolla hur många som har mer än 0 i hälsa?
            };
            List<string> names = new List<string>
            {
                "Health",
                "Ammunition",

                // TODO: en player remaining vet inte om vi skall göra en komponent för det? :)
            };
            foreach (Vector2 slots in SlotPositions)
            {

            }


            List<IComponent> components = new List<IComponent>
            {
                health,
                ammo,
                slot1,
                new TextComponent(names, textComponents)
            };
            ComponentManager.Instance.AddAllComponents(HudID, components);

            return HudID;
        }
    }
}
