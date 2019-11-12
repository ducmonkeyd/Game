using System;
using System.Xml;

using Microsoft.Xna.Framework;

namespace MainGame
{
#if WINDOWS || LINUX
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        // test data map
        static void Main()
        {
            //XMLData.MapData datamap = new XMLData.MapData();
            //Rectangle temp = new Rectangle(100, 100, 100, 100);
            //int x = 50;
            //int y = 20;
            //for(int i=0;i<4;i++)
            //{
            //    temp.X += x;
            //    temp.Y += y;
            //    datamap.pos.Add(temp);
            //    datamap.typeObj.Add(i);
            //}

            //XmlWriterSettings setting = new XmlWriterSettings();
            //setting.Indent = true;


            //using (XmlWriter writer = XmlWriter.Create("test.xml", setting))
            //{
            //    Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate.IntermediateSerializer.Serialize(writer, datamap, null);
            //}
            using (var game = new MainGame())
                game.Run();
        }
    }
#endif
}
