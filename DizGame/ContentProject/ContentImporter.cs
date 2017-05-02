using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TInput = System.String;
using TOutput = System.String;

namespace ContentProject
{
    public class ContentImporter : ContentImporter<TInput>
    {
        [ContentImporter(".xyz", DisplayName = "XYZ Importer", DefaultProcessor = "AnimationProcessor")]
        public override TInput Import(string filename, ContentImporterContext context)
        {
            // TODO: process the input object, and return the modified data.
            throw new NotImplementedException();
        }
    }
}
