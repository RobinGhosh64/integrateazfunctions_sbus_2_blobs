using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace FunctionApp1
{
    public class OurUtility
    {

        public static System.IO.Stream GenerateStreamFromString(string s)
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            StreamWriter writer = new System.IO.StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
