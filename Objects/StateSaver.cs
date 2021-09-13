using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace MeasureTape.Objects
{
    public class StateSaver
    {
        DataContractJsonSerializer _serializer = new DataContractJsonSerializer(typeof(State));
        public string FileDir { get; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\PauliusUrmonas\MeasureTape\";
        public string FileName { get; } = "state.json";
        public string Path { get { return System.IO.Path.Combine(FileDir, FileName); } }
        public void Save(State state)
        {
            FileInfo fi = new FileInfo(Path);
            if (!fi.Exists)
                Directory.CreateDirectory(FileDir);
            else fi.Delete();
            using (Stream stream = fi.OpenWrite())
                _serializer.WriteObject(stream, state);
        }
        public State Load()
        {
            FileInfo fi = new FileInfo(Path);
            if (fi.Exists)
                using (Stream stream = fi.OpenRead())
                    return (State)_serializer.ReadObject(stream);
            return new State();
        }
    }
}
