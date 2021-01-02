using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Phychips.Helper
{
    public class SerializeHelper
    {
        static bool Serializer(object o)
        {
            try
            {
                Type t = o.GetType();
                XmlSerializer xmlSerializer = new XmlSerializer(t);
                StreamWriter writer = new StreamWriter(t.Name + ".xml");
                xmlSerializer.Serialize(writer, o);
                writer.Close();
            }
            catch
            {
                return false;
            }
            return true;            
        }

        static bool Deserializer(ref object o)
        {
            if (o == null)
                return false;

            try
            {
                Type t = o.GetType();
                XmlSerializer xmlSerializer = new XmlSerializer(t);
                StreamReader reader = new StreamReader(t.Name + ".xml");
                o = xmlSerializer.Deserialize(reader);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
