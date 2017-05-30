﻿using Engine.Builder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Score
    {
        public ScoreInfo Info { get; set; } = new ScoreInfo();

        public static Score CreateFromMusicXml(Stream stream)
        {
            var builder = new ScoreBuilder(stream);
            return builder.BuildScore();
        }
    }
}