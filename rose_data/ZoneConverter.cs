#region License

//Copyright (C) 2020 Chirstopher Torres (Raven)
//
//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.
//
//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.
//
//You should have received a copy of the GNU General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Revise.STB;
using rose_data.Data;

namespace rose_data
{
    public class ZoneConverter
    {
        public ZoneConverter(string rootDirectory)
        {
            const string skillStb = "list_zone.stb";
            LoadAndConvert(rootDirectory + "\\3DDATA\\STB\\" + skillStb );
        }

        public void LoadAndConvert(string stbPath = null)
        {
            if (stbPath == null)
                return;
            
            var dataFile = new DataFile();
            try
            {
                dataFile.Load(stbPath);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e);
                return;
            }

            List<Zone> zoneList = new List<Zone>();
            for (var i = 0; i < dataFile.RowCount; i++)
            {
                var curRow = dataFile[i];
                if (!curRow[2].Contains(".zon")) continue;
                
                Console.Write("Attempting to load \"" + curRow[1] + "\" - ");
                var zone = new Zone(i);
                zone.Load(curRow);

                zoneList.Add(zone);
            }

            var jsonString = JsonConvert.SerializeObject(zoneList, Formatting.Indented,
                new JsonSerializerSettings {DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate});
            (new FileInfo("srv_data\\zone_data.json")).Directory.Create();
            var sqlFile = new System.IO.StreamWriter("srv_data\\zone_data.json", false);
            using (sqlFile)
            {
                sqlFile.WriteLine(jsonString);
            }
        }
    }
}