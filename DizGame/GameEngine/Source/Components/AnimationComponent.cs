using AnimationContentClasses;
using GameEngine.Source.Enums;
using GameEngine.Source.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Components
{
    public class AnimationComponent : IComponent
    {


        #region Properties
        private int AnimationEntityID;
        public AnimationModelData AnimationModelData { get; private set; }
        public AnimationData ActiveAnimation { get; set; }
        public int ActiveAnimationKeyFrame { get; set; }
        public bool EnableAnimationLoop { get; set; }
        public float AnimationSpeed { get; set; }
        public TimeSpan ActiveAnimationTime { get; set; }
        public Matrix[] Bones { get; set; }
        public Matrix[] BonesTransforms { get; private set; }
        public Matrix[] BonesAbsolute { get; set; }
        public Matrix[] BonesAnimation { get; set; }
        //AnimatedModelEffect AnimatedModelEffect {get; set;}
        #endregion

        public AnimationComponent(int AnimationEntityID)
        {
            this.AnimationEntityID = AnimationEntityID;
            
            // Default animation config
            AnimationSpeed = 1.0f;
            ActiveAnimationKeyFrame = 0;
            ActiveAnimationTime = TimeSpan.Zero;

        }

        public void CreateAnimationData()
        {
            //Bones = new List<Matrix>();
            //BonesTransforms = new List<Matrix>();
            //BonesAbsolute = new List<Matrix>();
            //BonesAnimation = new List<Matrix>();

            ModelComponent model = ComponentManager.Instance.GetEntityComponent<ModelComponent>(AnimationEntityID);
            Dictionary<string, object> modelTag = (Dictionary<string,object>) model.Model.Tag;
            if (modelTag == null)
                throw new InvalidOperationException("Oups!! Something went wrong here, this might not be an animation model");

            if (modelTag.ContainsKey("AnimationModelData"))
                AnimationModelData = (AnimationModelData)modelTag["AnimationModelData"];

            else
                throw new InvalidOperationException("This is not a valid animation model, please try again");

            if(AnimationModelData.AnimationData.Length > 0)
            {
                ActiveAnimation = AnimationModelData.AnimationData[0];
            }

            Bones = new Matrix[AnimationModelData.BindPose.Length];
            BonesAbsolute = new Matrix[AnimationModelData.BindPose.Length];
            BonesAnimation = new Matrix[AnimationModelData.BindPose.Length];
            // Used to apply custom transformation over the bones
            BonesTransforms = new Matrix[AnimationModelData.BindPose.Length];

            for(int i = 0; i < Bones.Length; i++)
            {
                Bones[i] = AnimationModelData.BindPose[i];
                BonesTransforms[i] = Matrix.Identity;
            }
            
            //AnimatedModelEffect = new AnimatedModelEffect(model.Model.Meshes[0].Effects[0]);
        }
    }
}
