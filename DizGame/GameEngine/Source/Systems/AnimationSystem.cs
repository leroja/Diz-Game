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
        public override void Update(GameTime gameTime)
        {
            List<int> entitiesWithAnimation = ComponentManager.GetAllEntitiesWithComponentType<AnimationComponent>();

            foreach(int entity in entitiesWithAnimation)
            {
                AnimationComponent anmc = ComponentManager.GetEntityComponent<AnimationComponent>(entity);
                TransformComponent tfc = ComponentManager.GetEntityComponent<TransformComponent>(entity);

                anmc.ActiveAnimationTime = new TimeSpan((long)(gameTime.ElapsedGameTime.Ticks * anmc.AnimationSpeed));

                if(anmc.ActiveAnimation != null)
                {
                    //loop the animation
                    if (anmc.ActiveAnimationTime > anmc.ActiveAnimation.Duration && anmc.EnableAnimationLoop)
                    {
                        long elapsedTicks = anmc.ActiveAnimationTime.Ticks % anmc.ActiveAnimation.Duration.Ticks;
                        anmc.ActiveAnimationTime = new TimeSpan(elapsedTicks);
                        anmc.ActiveAnimationKeyFrame = 0;
                    }

                    if (anmc.ActiveAnimationKeyFrame == 0)
                    {
                        for (int i = 0; i < anmc.Bones.Length; i++)
                            anmc.Bones[i] = anmc.AnimationModelData.BindPose[i];
                    }

                    int index = 0;
                    KeyFrame[] keyframes = anmc.ActiveAnimation.KeyFrames;
                    while (index < keyframes.Length && keyframes[index].Time <= anmc.ActiveAnimationTime)
                    {
                        int boneIndex = keyframes[index].BoneIndex;
                        anmc.Bones[boneIndex] = keyframes[index].Transform * anmc.BonesTransforms[boneIndex];
                        index++;
                    }
                    anmc.ActiveAnimationKeyFrame = index - 1;
                }

                if(tfc.ObjectMatrix != null)
                {

                    anmc.BonesAbsolute[0] = anmc.Bones[0] * tfc.ObjectMatrix;
                    for (int i = 1; i < anmc.BonesAnimation.Length; i++)
                    {
                        int boneParent = anmc.AnimationModelData.BonesParent[i];
                        // Here we are transforming a child bone by its parent
                        anmc.BonesAbsolute[i] = anmc.Bones[i] * anmc.BonesAbsolute[boneParent];
                    }

                    for (int i = 0; i < anmc.BonesAnimation.Length; i++)
                    {
                        anmc.BonesAnimation[i] = anmc.AnimationModelData.InverseBindPose[i] * anmc.BonesAbsolute[i];
                    }

                }
                
                
            }
        }
    }
}
