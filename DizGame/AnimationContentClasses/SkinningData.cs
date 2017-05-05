using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationContentClasses
{
    public class SkinningData
    {
        public Dictionary<string, AnimationClip> AnimationClips { get; private set; }

        //You store the skeleton’s bones in its bind pose configuration. The bind pose is the pose in
        //which the bones were linked to the model’s mesh and is the starting pose of any animation.
        public List<Matrix> BindPose { get; private set; }
        //The bonesInverseBindPose attribute stores an array containing the inverse absolute configuration
        //(absolute meaning defined in world 3D space and not related to its ancestor) of each skeleton’s
        //bone in its bind pose
        public List<Matrix> InverseBindPose { get; private set; }
        //For each bone, stores the index of the parent bone
        public List<int> SkeletonHierarchy { get; private set; }

        /// <summary>
        /// This class stores all AnimationClips for one animated object in order to 
        /// create different animations for different bones within the object.
        /// </summary>
        /// <param name="animationClips">Dictionary which contains all animation clips for all the bones</param>
        /// <param name="bindPose">Bind pose matrices for each bone in the object</param>
        /// <param name="inverseBindPose">Vertex to bonespace transforms for each bone in the object</param>
        /// <param name="boneHierarchy">For each bone in the object, stores the index of the parent bone</param>
        public SkinningData(Dictionary<string, AnimationClip> AnimationClips, List<Matrix> BindPose, List<Matrix> InverseBindPose,
                            List<int> SkeletonHierarchy)
        {
            this.BindPose = BindPose;
            this.InverseBindPose = InverseBindPose;
            this.AnimationClips = AnimationClips;
            this.SkeletonHierarchy = SkeletonHierarchy;
        }
        private SkinningData()
        {

        }
    }
}
