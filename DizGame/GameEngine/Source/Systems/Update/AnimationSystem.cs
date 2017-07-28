using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using ContentProject;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// Class for handling components of the type AnimationComponent
    /// Each component is supposed to be updated to run different kinds of animations,
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
        /// <param name="gameTime">Takes a GameTime object which should represent the current elapsed gameTime</param>
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

            if (anc.CurrentClipValue == null)
                throw new InvalidOperationException(
                            "AnimationPlayer.Update was called before StartClip");

            // Update the animation position.
            if (relativeToCurrentTime)
            {
                //time += anc.CurrentTimeValue;
                time = anc.CurrentTimeValue;

                // If we reached the end, loop back to the start.
                while (time >= anc.CurrentClipValue.Duration)
                    time -= anc.CurrentClipValue.Duration;
            }

            if ((time < TimeSpan.Zero) || (time >= anc.CurrentClipValue.Duration))
                throw new ArgumentOutOfRangeException("time");

            // If the position moved backwards, reset the keyframe index.
            if (time < anc.CurrentTimeValue)
            {
                anc.CurrentKeyframe = 0;
                anc.SkinningDataValue.BindPose.CopyTo(anc.BoneTransforms, 0);
            }

            anc.CurrentTimeValue = time;

            // Read keyframe matrices.
            IList<KeyFrame> keyframes = anc.CurrentClipValue.KeyFrames;

            while (anc.CurrentKeyframe < keyframes.Count)
            {
                KeyFrame keyframe = keyframes[anc.CurrentKeyframe];

                // Stop when we've read up to the current time position.
                if (keyframe.Time > anc.CurrentTimeValue)
                    break;

                // Use this keyframe.
                anc.BoneTransforms[keyframe.BoneIndex] = keyframe.Transform;

                anc.CurrentKeyframe++;
            }
        }

        /// <summary>
        /// Help functions used by the update function to update the bones matrices
        /// </summary>
        /// <param name="tcp">Takes the transformComponent as a parameter to get the entities transformation matrices</param>
        public void UpdateWorldTransforms(TransformComponent tcp)
        {
            // Root bone.
            anc.WorldTransforms[0] = anc.BoneTransforms[0] * tcp.ObjectMatrix;

            // Child bones.
            for (int bone = 1; bone < anc.WorldTransforms.Length; bone++)
            {
                int parentBone = anc.SkinningDataValue.SkeletonHierarchy[bone];

                anc.WorldTransforms[bone] = anc.BoneTransforms[bone] *
                                             anc.WorldTransforms[parentBone];
            }
        }

        /// <summary>
        /// Helper used by the Update method to refresh the SkinTransforms data.
        /// </summary>
        public void UpdateSkinTransforms()
        {
            for (int bone = 0; bone < anc.SkinTransforms.Length; bone++)
            {
                anc.SkinTransforms[bone] = anc.SkinningDataValue.InverseBindPose[bone] *
                                            anc.WorldTransforms[bone];
            }
        }

    }
}