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
using Revise.STL;
using rose_data.Data;

namespace rose_data
{
    public class NpcConverter
    {
        public NpcConverter(string rootDirectory)
        {
            const string npcStb = "list_npc.stb";
            const string npcStl = "list_npc_s.stl";
            LoadAndConvert(rootDirectory + "\\3DDATA\\STB\\" + npcStb, rootDirectory + "\\3DDATA\\STB\\" + npcStl);
        }

        public void LoadAndConvert(string stbPath = null, string stlPath = null)
        {
            if (stbPath == null || stlPath == null)
                return;

            var stringFile = new StringTableFile();
            var dataFile = new DataFile();

            try
            {
                dataFile.Load(stbPath);
                stringFile.Load(stlPath);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e);
                return;
            }

            List<Npc> sqlFileList = new List<Npc>();
            for (var i = 0; i < dataFile.RowCount; i++)
            {
                StringTableRow strTableRow;
                var curRow = dataFile[i];
                try
                {
                    strTableRow = stringFile[curRow[41]];
                }
                catch (ArgumentException)
                {
                    continue;
                }

                var npc = new Npc(i);
                npc.Load(curRow, strTableRow);

                sqlFileList.Add(npc);
            }

            var jsonString = JsonConvert.SerializeObject(sqlFileList, Formatting.Indented,
                new JsonSerializerSettings {DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate});
            
            (new FileInfo("srv_data\\npc_db.json")).Directory.Create();
            var sqlFile = new System.IO.StreamWriter("srv_data\\npc_db.json", false);
            using (sqlFile)
            {
                sqlFile.WriteLine(jsonString);
            }
        }
    }
}