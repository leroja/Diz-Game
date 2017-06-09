using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentProject
{
    class TempModel
    {
        public List<BoundingSphere> Spheres { get; }
        public Model Model { get; set; }

        public TempModel(Model model, List<BoundingSphere> spheres)
        {
            Spheres = spheres;
            Model = model;
        }

    }
}
