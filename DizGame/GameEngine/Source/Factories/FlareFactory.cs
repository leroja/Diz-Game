using GameEngine.Source.Components;
using GameEngine.Source.Managers;
using GameEngine.Source.RandomStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Factories
{
    /// <summary>
    /// A factory for creating flares
    /// </summary>
    public static class FlareFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="device"></param>
        /// <param name="entityID"></param>
        /// <param name="flares"></param>
        /// <param name="lightDirection"></param>
        /// <param name="isActive"></param>
        /// <param name="glowSpritePath"></param>
        public static void CreateFlare(ContentManager Content, GraphicsDevice device, int entityID = -1, Flare[] flares = null, Vector3 lightDirection = new Vector3(), bool isActive = true, string glowSpritePath = "Flare/glow")
        {
            FlareComponent flare = new FlareComponent();
            if (entityID == -1)
                entityID = ComponentManager.Instance.CreateID();
            if (flares == null)
                flare.Flares = SetupDefaultFlares();
            else
                flare.Flares = flares;

            if (lightDirection != new Vector3())
                flare.LightDirection = lightDirection;

            flare.OcclusionQuery = new OcclusionQuery(device);
            flare.GlowSprite = Content.Load<Texture2D>(glowSpritePath);
            flare.IsActive = isActive;

            foreach (Flare flareEff in flare.Flares)
                flareEff.Texture = Content.Load<Texture2D>(flareEff.TextureName);


            ComponentManager.Instance.AddComponentToEntity(entityID, flare);
        }

        // Array describes the position, size, color, and texture for each individual
        // flare graphic. The position value lies on a line between the sun and the
        // center of the screen. Zero places a flare directly over the top of the sun,
        // one is exactly in the middle of the screen, fractional positions lie in
        // between these two points, while negative values or positions greater than
        // one will move the flares outward toward the edge of the screen. Changing
        // the number of flares, or tweaking their positions and colors, can produce
        // a wide range of different lensflare effects without altering any other code.
        private static Flare[] SetupDefaultFlares()
        {
            Flare[] flares =
            {
            new Flare(-0.5f, 0.7f, new Color( 50,  25,  50), "Flare/flare1"),
            new Flare( 0.3f, 0.4f, new Color(100, 255, 200), "Flare/flare1"),
            new Flare( 1.2f, 1.0f, new Color(100,  50,  50), "Flare/flare1"),
            new Flare( 1.5f, 1.5f, new Color( 50, 100,  50), "Flare/flare1"),

            new Flare(-0.3f, 0.7f, new Color(200,  50,  50), "Flare/flare2"),
            new Flare( 0.6f, 0.9f, new Color( 50, 100,  50), "Flare/flare2"),
            new Flare( 0.7f, 0.4f, new Color( 50, 200, 200), "Flare/flare2"),

            new Flare(-0.7f, 0.7f, new Color( 50, 100,  25), "Flare/flare3"),
            new Flare( 0.0f, 0.6f, new Color( 25,  25,  25), "Flare/flare3"),
            new Flare( 2.0f, 1.4f, new Color( 25,  50, 100), "Flare/flare3"),
        };
            return flares;
        }
    }
}
