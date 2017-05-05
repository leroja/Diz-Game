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

        // Information about the currently playing animation clip.
        public AnimationClip currentClipValue { get; private set; }
        public TimeSpan currentTimeValue { get; set; }
        public int currentKeyframe { get; set; }


        // Current animation transform matrices.
        public Matrix[] boneTransforms { get; set; }
        public Matrix[] worldTransforms { get; set; }
        public Matrix[] skinTransforms { get; set; }


        // Backlink to the bind pose and skeleton hierarchy data.
        public SkinningData skinningDataValue { get; set; }

        //public Effect AnimationEffect { get; set; }
        //AnimatedModelEffect AnimatedModelEffect {get; set;}
        #endregion

        public AnimationComponent(int AnimationEntityID)
        {
            this.AnimationEntityID = AnimationEntityID;

            ModelComponent mcp = ComponentManager.Instance.GetEntityComponent<ModelComponent>(AnimationEntityID);

            skinningDataValue = mcp.Model.Tag as SkinningData;
            // Default animation config
            //AnimationSpeed = 1.0f;
            //ActiveAnimationKeyFrame = 0;
            //ActiveAnimationTime = TimeSpan.Zero;

            if (skinningDataValue == null)
                throw new InvalidOperationException("This model does not contain a SkinningData tag.");



            boneTransforms = new Matrix[skinningDataValue.BindPose.Count];
            worldTransforms = new Matrix[skinningDataValue.BindPose.Count];
            skinTransforms = new Matrix[skinningDataValue.BindPose.Count];

        }

        /// <summary>
        /// Starts decoding the specified animation clip.
        /// </summary>
        public void StartClip(string clipName)
        {
            if (clipName == null)
                throw new ArgumentNullException("clipname is missing");
            
            currentClipValue = (skinningDataValue.AnimationClips[clipName]);
            
            currentTimeValue = TimeSpan.Zero;
            currentKeyframe = 0;

            // Initialize bone transforms to the bind pose.
            skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
        }


    }
}
