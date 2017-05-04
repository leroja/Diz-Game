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
    public class AnimationSystem : IUpdate
    {
        TimeSpan time;
        AnimationComponent anc;
        public override void Update(GameTime gameTime)
        {
            time = gameTime.ElapsedGameTime;

            List<int> entityList = ComponentManager.GetAllEntitiesWithComponentType<AnimationComponent>();
            foreach (int entity in entityList)
            {
                anc = ComponentManager.GetEntityComponent<AnimationComponent>(entity);
                TransformComponent tcp = ComponentManager.GetEntityComponent<TransformComponent>(entity);
                UpdateBoneTransforms(true);
                UpdateWorldTransforms(tcp);
                UpdateSkinTransforms();
            }

        }

        /// <summary>
        /// Helper used by the Update method to refresh the BoneTransforms data.
        /// </summary>
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
                anc.skinningDataValue.BindPose.CopyTo(anc.boneTransforms, 0);
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
        /// Helper used by the Update method to refresh the WorldTransforms data.
        /// </summary>
        public void UpdateWorldTransforms(TransformComponent tcp)
        {
            // Root bone.
            anc.worldTransforms[0] = anc.boneTransforms[0] * tcp.ObjectMatrix;

            // Child bones.
            for (int bone = 1; bone < anc.worldTransforms.Length; bone++)
            {
                int parentBone = anc.skinningDataValue.SkeletonHierarchy[bone];

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
                anc.skinTransforms[bone] = anc.skinningDataValue.InverseBindPose[bone] *
                                            anc.worldTransforms[bone];
            }
        }

    }
}

