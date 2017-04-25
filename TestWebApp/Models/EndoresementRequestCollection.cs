using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace Frost.Models
{
    public class EndorsementRequestCollection : ICollection<EndorsementRequest>
    {
        public static EndorsementRequestCollection Current { get; set; }

        public EndorsementRequestCollection(IHostingEnvironment env)
        {
            FilePath = Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "requests.json");

            Read();

        }

        protected List<EndorsementRequest> Requests { get; } = new List<EndorsementRequest>();
        protected string FilePath { get; }

        public int Count => Requests.Count;

        public bool IsReadOnly => false;

        public void Add(EndorsementRequest item)
        {
            Requests.Add(item);

            Write();
        }

        public void Clear()
        {
            Requests.Clear();
            Write();
        }

        public bool Contains(EndorsementRequest item)
        {
            return Requests.Contains(item);
        }

        public void CopyTo(EndorsementRequest[] array, int arrayIndex)
        {
            Requests.CopyTo(array, arrayIndex);
        }

        public IEnumerator<EndorsementRequest> GetEnumerator()
        {
            return Requests.GetEnumerator();
        }

        public bool Remove(EndorsementRequest item)
        {
            return Requests.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Requests).GetEnumerator();
        }

        protected void Read()
        {
            Requests.Clear();

            if (System.IO.File.Exists(FilePath))
            {
                using (var f = System.IO.File.OpenText(FilePath))
                {
                    using (var reader = new JsonTextReader(f))
                    {
                        var serializer = new JsonSerializer();
                        Requests.AddRange(serializer.Deserialize<List<EndorsementRequest>>(reader));
                    }
                }
            }
        }

        public void Write()
        {
            //Directory.CreateDirectory(FilePath);

            using (var f = File.CreateText(FilePath))
            {
                using (var writer = new JsonTextWriter(f))
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(writer, Requests);
                }
            }
        }
    }
}
