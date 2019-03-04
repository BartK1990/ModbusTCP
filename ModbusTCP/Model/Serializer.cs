using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusTCP.Model
{
    using System.Runtime.Serialization;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    public class Serializer
    {
        public void DataContract_Serialize<T>(T object_to_serialize, string filename)
        {
            var ds = new DataContractSerializer(typeof(T));
            using (Stream s = File.Create(filename))
            ds.WriteObject(s, object_to_serialize);
        }

        public void DataContract_Deserialize<T>(out T deserialize_dest, string filename)
        {
            var ds = new DataContractSerializer(typeof(T));
            using (Stream s = File.OpenRead(filename))
            {
                deserialize_dest = (T)ds.ReadObject(s);
            }
        }

        public void Binary_Serialize<T>(T object_to_serialize, string filename)
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream s = File.Create(filename))
            {
                formatter.Serialize(s, object_to_serialize);
            }
        }

        public void Binary_Deserialize<T>(out T deserialize_dest, string filename)
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream s = File.OpenRead(filename))
            {
                deserialize_dest = (T)formatter.Deserialize(s);
            }
        }
    }
}
