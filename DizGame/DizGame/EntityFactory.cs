using DizGame.Source.Components;
using GameEngine.Source.Components;
using GameEngine.Source.Enums;
using GameEngine.Source.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizGame
{
    public class EntityFactory
    {
        private ContentManager Content;
        public EntityFactory(ContentManager Content)
        {
            this.Content = Content;
        }
        /// <summary>
        /// Method for creating one of the many (maybe) player models. 
        /// </summary>
        public void CreateSexyWomanSoldier()
        {
            
            int entityID = ComponentManager.Instance.CreateID();
            Model sonya = Content.Load<Model>("sonya/Sonya");

            KeyBoardComponent keys = new KeyBoardComponent();
            keys.KeyBoardActions.Add("Forward", Keys.Up);
            keys.KeyBoardActions.Add("Backwards", Keys.Down);
            keys.KeyBoardActions.Add("Right", Keys.Right);
            keys.KeyBoardActions.Add("Left", Keys.Left);

            List<IComponent> components = new List<IComponent>
            {
                new TransformComponent(new Vector3(0,0,-20), new Vector3(1,1,1)),
                new ModelComponent(sonya),
                new WorldComponent(Matrix.Identity),
                //new CameraComponent(CameraType.Chase),
                keys,
            };

            //TODO: need to add keyboard components and such to assign controllers to the model.

            ComponentManager.Instance.AddAllComponents(entityID, components);
        }

        public void CreateStaticCam(Vector3 CameraPosition, Vector3 lookAt)
        {
            ComponentManager.Instance.AddAllComponents(ComponentManager.Instance.CreateID(), new List<IComponent>() {
                new TransformComponent(CameraPosition, Vector3.One),
                new CameraComponent(CameraType.StaticCam)
                {
                    LookAt = lookAt
                }
            });
        }

        // roation är inte riktigt det jag vill, oriantaion är nog mer det jag vill ha
        // plus att den model jag har laddat in är en hel patron, inte en kula som jag vill ha
        public void CreateBullet(Model model, Vector3 pos, Vector3 rotation, Vector3 scale)
        {
            int BulletEntity = ComponentManager.Instance.CreateID();

            List<IComponent> componentList = new List<IComponent>()
            {
                new TransformComponent(pos, scale)
                {
                    Rotation = rotation
                },
                new  ModelComponent(model),

                //temp
                new MouseComponent(){
                    //MouseSensitivity = 1.9f
                },
                new BulletComponent(),
                new CameraComponent(CameraType.Chase)
                {
                    Offset = new Vector3(0,0,10)
                    //Offset = new Vector3(0,5,15)
                },
            };

            ComponentManager.Instance.AddAllComponents(BulletEntity, componentList);
        }
    }
}
