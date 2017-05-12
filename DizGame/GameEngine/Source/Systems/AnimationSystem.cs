using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using AnimationContentClasses;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// Class for handling components of the type AnimationComponent
    /// Each component is suposed to be updated to run different kinds of animations,
    /// therefore this class contains logic to do the desired updates for the respective
    /// animations
    /// </summary>
    public class AnimationSystem : IUpdate
    {
        TimeSpan time;
        AnimationComponent anc;
        /// <summary>
        /// Update logic required for the AnimationComponents
        /// </summary>
        /// <param name="gameTime">Takes a GameTime object which should represent the current elapsed gametime</param>
        public override void Update(GameTime gameTime)
        {

            List<int> entityList = ComponentManager.GetAllEntitiesWithComponentType<AnimationComponent>();
            foreach (int entity in entityList)
            {
                time = gameTime.ElapsedGameTime;
                anc = ComponentManager.GetEntityComponent<AnimationComponent>(entity);
                TransformComponent tcp = ComponentManager.GetEntityComponent<TransformComponent>(entity);
                UpdateBoneTransforms(true);
                UpdateWorldTransforms(tcp);
                UpdateSkinTransforms();
            }

        }

        /// Helper used by the Update method to refresh the BoneTransforms data.

        /// <summary>
        /// Helper method used by the update method to update the data for which keyframes to play 
        /// if the last keyframe is reached, i.e the end of the animation the animation is 'restarted'
        /// from the beginning
        /// </summary>
        /// <param name="relativeToCurrentTime"></param>
        public void UpdateBoneTransforms(bool relativeToCurrentTime)
        {

            if (anc.currentClipValue == null)
                throw new InvalidOperationException(
                            "AnimationPlayer.Update was called before StartClip");

            // Update the animation position.
            if (relativeToCurrentTime)
            {
                time += anc.currentTimeValue;

                // If we reached the end, loop back to the start.
                while (time >= anc.currentClipValue.Duration)
                    time -= anc.currentClipValue.Duration;
            }

            if ((time < TimeSpan.Zero) || (time >= anc.currentClipValue.Duration))
                throw new ArgumentOutOfRangeException("time");

            // If the position moved backwards, reset the keyframe index.
            if (time < anc.currentTimeValue)
            {
                anc.currentKeyframe = 0;
                anc.SkinningDataValue.BindPose.CopyTo(anc.boneTransforms, 0);
            }

            anc.currentTimeValue = time;

            // Read keyframe matrices.
            IList<KeyFrame> keyframes = anc.currentClipValue.KeyFrames;

            while (anc.currentKeyframe < keyframes.Count)
            {
                KeyFrame keyframe = keyframes[anc.currentKeyframe];

                // Stop when we've read up to the current time position.
                if (keyframe.Time > anc.currentTimeValue)
                    break;

                // Use this keyframe.
                anc.boneTransforms[keyframe.BoneIndex] = keyframe.Transform;

                anc.currentKeyframe++;
            }
        }

       /// <summary>
       /// Help functions used by the update function to update the bones matrices
       /// </summary>
       /// <param name="tcp">Takes the transformComponent as a parameter to get the entities transformation matrices</param>
        public void UpdateWorldTransforms(TransformComponent tcp)
        {
            // Root bone.
            anc.worldTransforms[0] = anc.boneTransforms[0] * tcp.ObjectMatrix;

            // Child bones.
            for (int bone = 1; bone < anc.worldTransforms.Length; bone++)
            {
                int parentBone = anc.SkinningDataValue.SkeletonHierarchy[bone];

                anc.worldTransforms[bone] = anc.boneTransforms[bone] *
                                             anc.worldTransforms[parentBone];
            }
        }

        /// <summary>
        /// Helper used by the Update method to refresh the SkinTransforms data.
        /// </summary>
        public void UpdateSkinTransforms()
        {
            for (int bone = 0; bone < anc.skinTransforms.Length; bone++)
            {
                anc.skinTransforms[bone] = anc.SkinningDataValue.InverseBindPose[bone] *
                                            anc.worldTransforms[bone];
            }
        }

    }
}

