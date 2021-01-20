using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Engine.Engine_Settings
{
    [TestReminder("EngineSettingsLoader", "Test to make sure it saves and loads files correctly")]
    public static class EngineSettings
    {
        public static T LoadSettingsFile<T>() where T : new()
        {
            Type myType = typeof(T);

            if (File.Exists(myType.Name))
            {
                XmlSerializer mySerializer = new XmlSerializer(typeof(T));
               return (T)mySerializer.Deserialize(new FileStream(myType.Name + ".xml", FileMode.Open, FileAccess.ReadWrite, FileShare.None));
            }
            else
            {
                XmlSerializer mySerializer = new XmlSerializer(typeof(T));
                var myFileStream = new FileStream(myType.Name + ".xml", FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                T myNewSettingsType = new T();
                mySerializer.Serialize(myFileStream, myNewSettingsType);
                return myNewSettingsType;
            }
        }






    }
}
