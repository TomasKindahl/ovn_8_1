using System.Formats.Asn1;
using System.Globalization;

namespace ovn_8_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string directoryPath = @"..\..\..";
            string fileName = "nations.csv";
            string filePath = Path.Combine(directoryPath, fileName);

            // QUERY SYNTAX
            IEnumerable<Nation> nations =
                from nation in ReadAllNations(filePath)
                where nation.Continent == "Europe"
                   && nation.Population > 10000000
                select nation;
            Console.WriteLine("name                      population");
            Console.WriteLine("------------------------------------");
            foreach (Nation nation in nations)
            {
                Console.WriteLine($"{nation.Name,-25} {nation.Population,10}");
            }
            Console.WriteLine("------------------------------------");

            // METHOD SYNTAX
            nations = ReadAllNations(filePath)
                .Where(n => n.Continent == "Europe" && n.Population > 10000000);
            Console.WriteLine("name                      population");
            Console.WriteLine("------------------------------------");
            foreach (Nation nation in nations)
            {
                Console.WriteLine($"{nation.Name,-25} {nation.Population,10}");
            }
            Console.WriteLine("------------------------------------");
        }
        class Nation
        {
            [Index(0)]
            public string Name { get; set; }
            [Index(1)]
            public long Population { get; set; }
            [Index(2)]
            public string Continent { get; set; }
            [Index(3)]
            public string Capital { get; set; }
        }
        static IEnumerable<Nation> ReadAllNations(string filePath)
        {
            IEnumerable<Nation> records = null;
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                BadDataFound = null,
                MissingFieldFound = null,
                Delimiter = ";"
            };
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                using (CsvReader csvReader = new CsvReader(streamReader, config))
                {

                    // Read records from the CSV file
                    records = csvReader.GetRecords<Nation>();
                    foreach (var record in records)
                    {
                        yield return record;
                        // `yield` saves the entire environment and
                        //         returns it to be used as a stream
                    }
                }
            }
        }
    }
}
