using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentProject
{
    /// <summary>
    /// SkinningData class is a class used to store information regarding a models animations.
    /// An object of this class should contain all information that you need to animate one 
    /// single model, if the model contains multiple animations then there's multiple animationclips stored in
    /// it's dictionary.
    /// </summary>
    public class SkinningData
    {
        /// <summary>
        /// Dictionary containing all separate AnimationClips (Which contains data for each separate animation) 
        /// the key is a string which represents the name of the animation and the key is the AnimationClip object itself.
        /// </summary>
        public Dictionary<string, AnimationClip> AnimationClips { get; private set; }

        /// <summary>
        /// You store the skeleton’s bones in its bind pose configuration. The bind pose is the pose in
        /// which the bones were linked to the model’s mesh and is the starting pose of any animation.
        /// </summary>
        public List<Matrix> BindPose { get; private set; }

        /// <summary>
        /// The bonesInverseBindPose attribute stores an array containing the inverse absolute configuration
        /// (absolute meaning defined in world 3D space and not related to its ancestor) of each skeleton’s 
        /// bone in its bind pose
        /// </summary>
        public List<Matrix> InverseBindPose { get; private set; }

        /// <summary>
        /// For each bone, stores the index of the parent bone
        /// </summary>
        public List<int> SkeletonHierarchy { get; private set; }

        /// <summary>
        /// This class stores all AnimationClips for one animated object in order to 
        /// create different animations for different bones within the object.
        /// </summary>
        /// <param name="AnimationClips">Dictionary which contains all animation clips for all the bones</param>
        /// <param name="BindPose">Bind pose matrices for each bone in the object</param>
        /// <param name="InverseBindPose">Vertex to bonespace transforms for each bone in the object</param>
        /// <param name="SkeletonHierarchy">For each bone in the object, stores the index of the parent bone</param>
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
