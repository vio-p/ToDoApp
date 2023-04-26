using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ToDoApp.Services
{
    public static class SerializationService
    {
        public static void Serialize<T>(T item, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            FileStream file = new FileStream(filePath, FileMode.Create);
            serializer.Serialize(file, item);
            file.Dispose();
        }

        public static T Deserialize<T>(string filePath) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            FileStream file = new FileStream(filePath, FileMode.Open);
            T deserializedItem = serializer.Deserialize(file) as T;
            file.Dispose();
            return deserializedItem;
        }
    }
}
