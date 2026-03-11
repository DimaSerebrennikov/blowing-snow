// TexturePack.csC:\Users\DimaS\Desktop\FeebleSnowOriginal-master\Assets\Dima Serebrennikov\Fluffy snow\TexturePack.csTexturePack.cs
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace Serebrennikov {
    [Serializable]
    public class TexturePack {
        public Texture Albedo;
        public Texture Height;
        public float HeightScale;
        public float HeightWeight;
        public Color SnowColor; 
    }
}
