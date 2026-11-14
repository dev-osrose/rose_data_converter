using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Revise.STB;

namespace rose_data
{
    /// <summary>
    /// Writes an export to srv_data/. Enumerations are written by name.
    /// </summary>
    public static class JsonOutput
    {
        public static void Write(string fileName, object data)
        {
            var settings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Include,
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            };
            var path = Path.Combine("srv_data", fileName);
            new FileInfo(path).Directory.Create();
            File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented, settings) + "\n");
        }

        /// <summary>
        /// Loads 3DDATA/STB/<paramref name="name"/>; null, with a message, when
        /// it is missing.
        /// </summary>
        public static DataFile LoadTable(string rootDirectory, string name)
        {
            var file = new DataFile();
            try
            {
                file.Load(GamePath.Resolve(rootDirectory, "3DDATA/STB/" + name));
            }
            catch (FileNotFoundException e)
            {
                System.Console.WriteLine(e.Message);
                return null;
            }
            return file;
        }

        /// <summary>Every cell of a table row, as integers.</summary>
        public static List<int> Row(DataFile file, int row)
        {
            var values = new List<int>();
            for (var c = 0; c < file.ColumnCount; c++)
                values.Add(Int(file[row][c]));
            return values;
        }

        /// <summary>A table cell as an integer, 0 when it holds none.</summary>
        public static int Int(string cell)
        {
            return int.TryParse(cell, out var value) ? value : 0;
        }
    }
}
