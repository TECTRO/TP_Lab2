using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lab2
{
    class Program
    {
        static string sities_path = "cities_of_russia.csv";
        static string dictionaty_path = "словарь_русск_слов_2011.csv";
        static void Main(string[] args)
        {
            var cities = new List<string>();
            using (var fs = new StreamReader(new FileStream(sities_path, FileMode.OpenOrCreate)))
            {
                while (!fs.EndOfStream)
                    cities.Add(fs.ReadLine());
            }

            var dict = new List<city>();
            using (var fs = new StreamReader(new FileStream(dictionaty_path, FileMode.OpenOrCreate)))
            {
                while (!fs.EndOfStream)
                {
                    var queued = new Queue<string>(fs.ReadLine().Split('\t'));

                    try
                    {
                        dict.Add(new city { name = queued.Dequeue(), tag = queued.Dequeue(), freq = (float)Convert.ToDouble(queued.Dequeue().Replace('.', ',')) });
                    }
                    catch (Exception) { }
                }
            }
            Console.WriteLine(string.Join("\n\n", cities.GroupBy(city => city.First()).Select(gr => $"Городов на букву {gr.Key} - {gr.Count()} \n[{string.Join(", ", gr.ToList())}]")));

            Console.WriteLine();
            Console.WriteLine();

            var dictReady = dict.Where(tt => tt.tag.Equals("s")).Select(tt => tt.name.ToLower().Replace("ё", "е")).ToList();
            var simpleCities = cities.Where(t => dictReady.Contains(t.ToLower().Replace("ё", "е")));
            Console.WriteLine($"Города, которые называются обычными словами русского языка: ({simpleCities.Count()})\n {string.Join(", ", simpleCities)}");
            Console.ReadKey();
        }
    }

    class city
    {
        public string name { get; set; }
        public string tag { get; set; }
        public float freq { get; set; }
    }
}
