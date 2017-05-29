using Engine.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Engine
{
    public class Song
    {
        public String FilePath { get; }

        public String Composer { get; set; }

        public Song(String filePath)
        {
            FilePath = filePath;
        }

        public List<Part> Parts { get; set; } = new List<Part>();
    }
}
