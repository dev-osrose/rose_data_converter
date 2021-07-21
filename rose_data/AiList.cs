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
    public class AiList
    {
        public List<string> AiFileList { get; set; } = new List<string>();
        
        public AiList(string rootDirectory)
        {
            const string aiStb = "FILE_AI.stb";
            LoadAndConvert(rootDirectory + "\\3DDATA\\STB\\" + aiStb);
        }

        // public Motion GetById(int id)
        // {
        //     return AiData.Find(ai => ai.Id == id);
        // }

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
            
            for (var i = 0; i < dataFile.RowCount; i++)
            {
                AiFileList.Add(dataFile[i][1]);
                
                //TODO: Load the AIP file data
                // var ai = new Ai(i);
                // var valid = ai.Load(dataFile[i]);
                // if (valid) AiData.Add(ai);
            }
            
            //TODO: Output the AI data in a scriptable format (LUA)

            var jsonString = JsonConvert.SerializeObject(AiFileList, Formatting.Indented,
                new JsonSerializerSettings {DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate});
            
            (new FileInfo("srv_data\\ai_db.json")).Directory.Create();
            var sqlFile = new System.IO.StreamWriter("srv_data\\ai_db.json", false);
            using (sqlFile)
            {
                sqlFile.WriteLine(jsonString);
            }
        }
    }
}