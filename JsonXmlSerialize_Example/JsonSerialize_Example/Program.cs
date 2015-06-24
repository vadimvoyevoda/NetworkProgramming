using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace JsonSerialize_Example
{
    public class ClassFrom
    {
        public string name;
        public string surname;
        public int age;
        public DateTime birth;
        public string[] hobbies;

        public ClassFrom()
        {
        }

        public ClassFrom(string Name, string Surname, int Age, DateTime Birth, string[] Hobbies)
        {
            name = Name;
            surname = Surname;
            age = Age;
            birth = Birth;
            hobbies = Hobbies;
        }
    }

    [XmlRoot("ClassFrom")]
    public class ClassTo
    {
        public string name { get; set; }
        public string surname { get; set; }
        public short age { get; set; }
        public string birth { get; set; }
        public List<string> hobbies { get; set; }
        public string empty { get; set; }

        public void Print()
        {
            Console.WriteLine("My info");
            Console.WriteLine("Name : {0}", name);
            Console.WriteLine("Surname : {0}", surname);
            Console.WriteLine("Age : {0}", age);
            Console.WriteLine("Birthday : {0}", birth);
            Console.WriteLine("Hobbies : ");
            for (int i = 0; i < hobbies.Count; i++)
            {
                Console.WriteLine("- {0}", hobbies[i]);
            }
            Console.WriteLine("Add : {0}", empty);

        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ClassFrom from = new ClassFrom("Vadim", "Voievoda", 23, new DateTime(1991, 10, 16), new string[] {"dance", "poem", "programming" });

            Console.WriteLine("Serialization JSon:");
            string strRes = JsonConvert.SerializeObject(from);
            
            Console.WriteLine("Serialized string : ");
            Console.WriteLine(strRes);
            Console.WriteLine();

            ClassTo to = JsonConvert.DeserializeObject<ClassTo>(strRes);
            to.Print();

            Console.WriteLine("\n\nSerialization Xml");
            XmlSerializer ser = new XmlSerializer(typeof(ClassFrom));
            ser.Serialize(Console.Out, from);
            Console.WriteLine();

            ClassTo xmlTo = new ClassTo();
            using(FileStream xmlFile = new FileStream("xmlFile.txt", FileMode.OpenOrCreate))
            {
                ser.Serialize(xmlFile, from);
            }

            using(FileStream xmlFile = new FileStream("xmlFile.txt", FileMode.OpenOrCreate))
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(ClassTo));
                xmlTo = (ClassTo)xmlSer.Deserialize(xmlFile);
            }
            Console.WriteLine("\nDeserialization Class info");
            xmlTo.Print();
        }
    }
}
