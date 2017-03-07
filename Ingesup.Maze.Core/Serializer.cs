using System;
using System.IO;
using System.Runtime.Serialization;

namespace Ingesup.Maze.Core
{
    public static class Serializer
    {
        public static void ToFile<T>(string path, T value)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            using (FileStream fs = File.Create(path))
            {
                serializer.WriteObject(fs, value);
            }
        }

        public static T FromFile<T>(string path)
        {
            T value = default(T);
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            using (FileStream fs = File.OpenRead(path))
            {
                value = (T)serializer.ReadObject(fs);
            }

            return value;
        }
    }
}