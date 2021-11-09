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
    public class SkillConverter
    {
        public SkillConverter(string rootDirectory)
        {
            const string skillStb = "list_skill.stb";
            const string skillStl = "list_skill_s.stl";
            LoadAndConvert(rootDirectory + "\\3DDATA\\STB\\" + skillStb, rootDirectory + "\\3DDATA\\STB\\" + skillStl);
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

            List<Skill> skillList = new List<Skill>();
            for (var i = 0; i < dataFile.RowCount; i++)
            {
                StringTableRow strTableRow;
                var curRow = dataFile[i];
                try
                {
                    strTableRow = stringFile[curRow[(dataFile.ColumnCount - 1)]];
                }
                catch (ArgumentException)
                {
                    continue;
                }

                var skill = new Skill(i);
                skill.Load(curRow, strTableRow);

                skillList.Add(skill);
            }

            var jsonString = JsonConvert.SerializeObject(skillList, Formatting.Indented,
                new JsonSerializerSettings {DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate});
            
            (new FileInfo("srv_data\\skill_db.json")).Directory.Create();
            var sqlFile = new System.IO.StreamWriter("srv_data\\skill_db.json", false);
            using (sqlFile)
            {
                sqlFile.WriteLine(jsonString);
            }
        }
    }
}