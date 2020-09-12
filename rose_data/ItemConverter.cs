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
    public class ItemConverter
    {
        private struct ItemInfo
        {
            public string DataFile { get; private set; }
            public string StringFile { get; private set; }

            public ItemInfo(string dataFile, string stringFile)
            {
                this.DataFile = dataFile;
                this.StringFile = stringFile;
            }

            public void set(string dataFile, string stringFile)
            {
                this.DataFile = dataFile;
                this.StringFile = stringFile;
            }
        }

        private ItemInfo[] itemDataFiles = new ItemInfo[14];

        public ItemConverter(string rootDirectory)
        {
            itemDataFiles[00].set(rootDirectory + "list_faceitem.stb", rootDirectory + "list_faceitem_s.stl");
            itemDataFiles[01].set(rootDirectory + "list_cap.stb", rootDirectory + "list_cap_s.stl");
            itemDataFiles[02].set(rootDirectory + "list_body.stb", rootDirectory + "list_body_s.stl");
            itemDataFiles[03].set(rootDirectory + "list_arms.stb", rootDirectory + "list_arms_s.stl");
            itemDataFiles[04].set(rootDirectory + "list_foot.stb", rootDirectory + "list_foot_s.stl");
            itemDataFiles[05].set(rootDirectory + "list_back.stb", rootDirectory + "list_back_s.stl");
            itemDataFiles[06].set(rootDirectory + "list_jewel.stb", rootDirectory + "list_jewel_s.stl");
            itemDataFiles[07].set(rootDirectory + "list_weapon.stb", rootDirectory + "list_weapon_s.stl");
            itemDataFiles[08].set(rootDirectory + "list_subwpn.stb", rootDirectory + "list_subwpn_s.stl");
            itemDataFiles[09].set(rootDirectory + "list_useitem.stb", rootDirectory + "list_useitem_s.stl");
            itemDataFiles[10].set(rootDirectory + "list_jemitem.stb", rootDirectory + "list_jemitem_s.stl");
            itemDataFiles[11].set(rootDirectory + "list_natural.stb", rootDirectory + "list_natural_s.stl");
            itemDataFiles[12].set(rootDirectory + "list_questitem.stb", rootDirectory + "list_questitem_s.stl");
            itemDataFiles[13].set(rootDirectory + "list_pat.stb", rootDirectory + "list_pat_s.stl");

            LoadAndConvert();
        }

        public void LoadAndConvert()
        {
            int typeIdx = 0;
            List<Item> sqlFileList = new List<Item>();

            foreach (var itemDataFile in itemDataFiles)
            {
                ++typeIdx;
                var stringFile = new StringTableFile();
                var dataFile = new DataFile();

                try
                {
                    dataFile.Load(itemDataFile.DataFile);
                    stringFile.Load(itemDataFile.StringFile);
                }
                catch (FileNotFoundException)
                {
                    continue;
                }
                catch (ArgumentNullException)
                {
                    continue;
                }

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

                    var itemData = new Item(i, typeIdx);
                    itemData.Load(curRow, strTableRow);

                    sqlFileList.Add(itemData);
                }
            }

            var jsonString = JsonConvert.SerializeObject(sqlFileList, Formatting.Indented,
                new JsonSerializerSettings {DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate});
            
            var sqlFile = new System.IO.StreamWriter("srv_data\\item_db.json", false);
            using (sqlFile)
            {
                sqlFile.WriteLine(jsonString);
            }
        }
    }
}