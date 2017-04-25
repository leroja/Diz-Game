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
       
        public void CreateChuckGreen()
        {
            int entityID = ComponentManager.Instance.CreateID();
            Model chuck = Content.Load<Model>("chuck/DR3_Chuck_Greene");

            KeyBoardComponent keys = new KeyBoardComponent();
            keys.KeyBoardActions.Add("Forward", Keys.W);
            keys.KeyBoardActions.Add("Backwards", Keys.S);
            keys.KeyBoardActions.Add("Right", Keys.D);
            keys.KeyBoardActions.Add("Left", Keys.A);
            keys.KeyBoardActions.Add("Up", Keys.Space);


            List<IComponent> components = new List<IComponent>
            {
                new TransformComponent(new Vector3(0,0,20), new Vector3(0.135f,0.135f,0.135f), Matrix.CreateRotationY(-MathHelper.PiOver2)),
                new ModelComponent(chuck),
                new WorldComponent(Matrix.Identity),
                //new CameraComponent(CameraType.Chase),
                keys,
            };

            
            ComponentManager.Instance.AddAllComponents(entityID, components);

        }

        public int CreateKitana()
        {
            int entityID = ComponentManager.Instance.CreateID();
            Model kitana = Content.Load<Model>("Kitana/Kitana");

            KeyBoardComponent keys = new KeyBoardComponent();
            keys.KeyBoardActions.Add("Forward", Keys.Up);
            keys.KeyBoardActions.Add("Backwards", Keys.Down);
            keys.KeyBoardActions.Add("Right", Keys.Right);
            keys.KeyBoardActions.Add("Left", Keys.Left);
            keys.KeyBoardActions.Add("Up", Keys.Space);


            List<IComponent> components = new List<IComponent>
            {
                new TransformComponent(new Vector3(5,0,20), new Vector3(0.05f,0.05f,0.05f), Matrix.CreateRotationY(-MathHelper.PiOver2)),
                new ModelComponent(kitana),
                new WorldComponent(Matrix.Identity),
                //new CameraComponent(CameraType.Chase),
                //new PhysicsComponent()
                //{
                //    Mass = 60,
                //    PhysicsType = PhysicsType.Rigid,
                //    MaterialType = MaterialType.Skin
                //},
                //new CameraComponent(CameraType.Chase),
                keys,
                new BulletComponent(),
                new MouseComponent(){
                    //MouseSensitivity = 1.9f
                },
            };


            ComponentManager.Instance.AddAllComponents(entityID, components);

            return entityID;
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

        public void AddChaseCamToEntity(int EntityId, Vector3 Offset)
        {
            CameraComponent chaseCam = new CameraComponent(CameraType.Chase)
            {
                Offset = Offset
            };
            ComponentManager.Instance.AddComponentToEntity(EntityId, chaseCam);
        }

        // roation är inte riktigt det jag vill, oriantaion är nog mer det jag vill ha
        // plus att den model jag har laddat in är en hel patron, inte en kula som jag vill ha
        public int CreateBullet(Model model, Vector3 pos, Vector3 rotation, Vector3 scale)
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
                //new BulletComponent(),
                //new CameraComponent(CameraType.Chase)
                //{
                //    Offset = new Vector3(0,0,15)
                //    //Offset = new Vector3(0,5,15)
                //},
            };

            ComponentManager.Instance.AddAllComponents(BulletEntity, componentList);

            return BulletEntity;
        }
    }
}
