//! Copyright (C) 2007 Phychips
//! 
//! SerializeHelper.cs
//!
//! Description
//! 	SerializeHelper.cs
//!-------------------------------------------------------------------
//! History
//!-------------------------------------------------------------------
//! 1.0	2007/09/01	Jinsung Yi		Initial Release

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Soap;
using System.IO;
using System.IO.Ports;


namespace Phychips.Driver
{
    public class SioSerializeHelper
    {
        static readonly string fileName = "ComPortSettings" + ".xml";

        static public SioConfig LoadConfig()
        {
            SioConfig data = new SioConfig();

            try
            {
                using (Stream stream = new FileStream(fileName, FileMode.Open))
                {
                    // Deserialize SioData
                    IFormatter formatter = new SoapFormatter();
                    data = (SioConfig)formatter.Deserialize(stream);    
                }
            }
            catch(Exception e)
            {
                SaveConfig(data);
                System.Console.WriteLine(e.ToString());
            }

            return data;
        }

        static public void SaveConfig(SioConfig data)
        {
            using (Stream stream = new FileStream(fileName, FileMode.Create))
            {
                // Serialize SioData
                IFormatter formatter = new SoapFormatter();
                //formatter.Serialize(stream, new SioData(oData));
                formatter.Serialize(stream, data);
            }
        }
    }

}
