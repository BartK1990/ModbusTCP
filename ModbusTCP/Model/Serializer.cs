using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusTCP.Model
{
    using System.Runtime.Serialization;
    using System.IO;

    public class Serializer<T>
    {
        public void Serialize(T object_to_serialize, string filename)
        {
            var ds = new DataContractSerializer(typeof(T));
            using (Stream s = File.Create(filename))
            ds.WriteObject(s, object_to_serialize);
        }

        public void Deserialize(out T deserialize_dest, string filename)
        {
            var ds = new DataContractSerializer(typeof(T));
            using (Stream s = File.OpenRead(filename))
            {
                deserialize_dest = (T)ds.ReadObject(s);
            }
        }
    }
}
