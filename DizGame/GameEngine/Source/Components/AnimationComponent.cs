using GameEngine.Source.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Components
{
    class AnimationComponent : IComponent
    {
        private static AnimationState DEFAULT_STATE = AnimationState.Idle;

        #region Properties
        public AnimationState State { get; set; }
        public bool IsActive { get; set; }
        //For every index for the bones in a modelmesh or whatever structure you have
        public Dictionary<int, TransformComponent> AnimationTransforms { get; set; }
        #endregion

        public AnimationComponent(Dictionary<int, TransformComponent> animationTransforms, AnimationState state) : this(animationTransforms)
        {
            State = state;
        }

        public AnimationComponent(Dictionary<int, TransformComponent> animationTransforms)
        {
            AnimationTransforms = animationTransforms;
            State = DEFAULT_STATE;
        }
    }
}
