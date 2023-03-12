using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Storage
{
    class Storage<T> where T : ISerializable, new()
    {
        private readonly string StoragePath;

        private readonly Serializer<T> _serializer;


        public Storage(string fileName)
        {
            StoragePath = fileName;
            _serializer = new Serializer<T>();
        }

        public List<T> Load()
        {
            return _serializer.FromCSV(StoragePath);
        }

        public void Save(List<T> list)
        {
            _serializer.ToCSV(StoragePath, list);
        }
    }
}
