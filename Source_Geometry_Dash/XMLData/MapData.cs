using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace XMLData
{
    public class MapData
    {
        public int amount;
        public int speed;
        public List<Rectangle> pos;
        public List<int> typeObj;
        public MapData()
        {
            pos = new List<Rectangle>();
            typeObj = new List<int>();
        }
    }
    public class MapContent
    {
        public int amount;
        public List<int> sta;
    }
}
