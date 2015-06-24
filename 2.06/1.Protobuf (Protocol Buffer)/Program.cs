using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace _1.Protobuf__Protocol_Buffer_
{
    public enum SexEn
    {
        Male,
        Female
    }

    [Serializable]
    [ProtoContract]
    public class Child
    {
        [ProtoMember(1)]
        public int Age { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public string LastName { get; set; }
    }

    [Serializable]
    [ProtoContract]
    public class Person
    {
        [ProtoMember(1)]
        public int Age { get; set; }

        [ProtoMember(2)]
        public DateTime Birth { get; set; }

        [ProtoMember(3)]
        public string Name { get; set; }

        [ProtoMember(4)]
        public string LastName { get; set; }

        [ProtoMember(5)]
        public SexEn Sex { get; set; }

        [ProtoMember(6)]
        public Child children { get; set; }
    }

    class Program
    {
        private static void Main(string[] args)
        {
            var persons = new List<Person>
                            {
                                new Person
                                    {
                                        Age = 31,
                                        Birth = DateTime.Now,
                                        Name = "Jason",
                                        LastName = "Derulo",
                                        Sex = SexEn.Male,
                                        children = new Child {Name = "child", LastName = "Derulo", Age=11}
                                    },
                                new Person
                                    {
                                        Age = 26,
                                        Birth = DateTime.Now,
                                        Name = "Jessika",
                                        LastName = "State",
                                        Sex = SexEn.Female,
                                        children = new Child {Name = "child", LastName = "State", Age=9}
                                    }
                            };

            const string file1 = "D:\\personsBinary1.bin";
            Console.WriteLine("Test of BinaryFormatter");
            BinaryFormatterSer(persons, file1, 1000);
            BinaryFormatterSer(persons, file1, 2000);
            BinaryFormatterSer(persons, file1, 5000);

            const string file2 = "D:\\personsProtoBuf1.bin";
            Console.WriteLine("Test of ProtoBuf");
            ProtoBufSer(persons, file2, 1000);
            ProtoBufSer(persons, file2, 2000);
            ProtoBufSer(persons, file2, 5000);

            const string file3 = "D:\\personsJSon1.bin";
            Console.WriteLine("Test of JSon");
            JsonSer(persons, file3, 1000);
            JsonSer(persons, file3, 2000);
            JsonSer(persons, file3, 5000);

        }

        private static void BinaryFormatterSer(IList<Person> tasks, string fileName, int iterationCount)
        {
            var stopwatch = new Stopwatch();
            var formatter = new BinaryFormatter();
            using (var file = File.Create(fileName))
            {
                stopwatch.Restart();

                for (var i = 0; i < iterationCount; i++)
                {
                    file.Position = 0;
                    formatter.Serialize(file, tasks);
                    file.Position = 0;
                    var restoredTasks = (List<Person>)formatter.Deserialize(file);
                }

                stopwatch.Stop();

                Console.WriteLine("{0} iterations in {1} ms  - {2} size", iterationCount, stopwatch.ElapsedMilliseconds, file.Length);
            }
        }

        public static void ProtoBufSer(IList<Person> tasks, string fileName, int iterationCount)
        {
            var stopwatch = new Stopwatch();

            using(var file = File.Create(fileName))
            {
                stopwatch.Restart();

                for (var i = 0; i < iterationCount; i++)
                {
                    file.Position = 0;
                    Serializer.Serialize(file, tasks);
                    file.Position = 0;
                    var restoredTasks = Serializer.Deserialize<List<Person>>(file);
                    //Console.WriteLine("{0} {1} {2} {3} with child : {4} {5} {6}",
                    //    ((restoredTasks as List<Person>)[0] as Person).Name,
                    //    ((restoredTasks as List<Person>)[0] as Person).LastName,
                    //    ((restoredTasks as List<Person>)[0] as Person).Birth,
                    //    ((restoredTasks as List<Person>)[0] as Person).Sex.ToString(),
                    //    (((restoredTasks as List<Person>)[0] as Person).children as Child).Name,
                    //    (((restoredTasks as List<Person>)[0] as Person).children as Child).LastName,
                    //    (((restoredTasks as List<Person>)[0] as Person).children as Child).Age);
                }

                stopwatch.Stop();

                Console.WriteLine("{0} iterations in {1} ms  - {2} size", iterationCount, stopwatch.ElapsedMilliseconds, file.Length);
            }
        }

        private static void JsonSer(IList<Person> tasks, string fileName, int iterationCount)
        {
            var stopwatch = new Stopwatch();

            using (var file = File.Create(fileName))
            {
                stopwatch.Restart();
                for (var i = 0; i < iterationCount; i++)
                {
                    var str = JsonConvert.SerializeObject(tasks);

                    var bytes = Encoding.UTF8.GetBytes(str);
                    file.Write(bytes, 0, bytes.Length);
                    file.Read(bytes, 0, bytes.Length);
                    file.Position = 0;
                    var restoredTasks = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(bytes));
                }

                stopwatch.Stop();

                Console.WriteLine("{0} iterations in {1} ms  - {2} size", iterationCount, stopwatch.ElapsedMilliseconds, file.Length);
            }
        }
    }
}
