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
            itemDataFiles[00].set(GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_FACEITEM.STB"), GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_FACEITEM_S.STL"));
            itemDataFiles[01].set(GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_CAP.STB"), GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_CAP_S.STL"));
            itemDataFiles[02].set(GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_BODY.STB"), GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_BODY_S.STL"));
            itemDataFiles[03].set(GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_ARMS.STB"), GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_ARMS_S.STL"));
            itemDataFiles[04].set(GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_FOOT.STB"), GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_FOOT_S.STL"));
            itemDataFiles[05].set(GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_BACK.STB"), GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_BACK_S.STL"));
            itemDataFiles[06].set(GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_JEWEL.STB"), GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_JEWEL_S.STL"));
            itemDataFiles[07].set(GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_WEAPON.STB"), GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_WEAPON_S.STL"));
            itemDataFiles[08].set(GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_SUBWPN.STB"), GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_SUBWPN_S.STL"));
            itemDataFiles[09].set(GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_USEITEM.STB"), GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_USEITEM_S.STL"));
            itemDataFiles[10].set(GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_JEMITEM.STB"), GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_JEMITEM_S.STL"));
            itemDataFiles[11].set(GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_NATURAL.STB"), GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_NATURAL_S.STL"));
            itemDataFiles[12].set(GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_QUESTITEM.STB"), GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_QUESTITEM_S.STL"));
            itemDataFiles[13].set(GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_PAT.STB"), GamePath.Resolve(rootDirectory, "3DDATA/STB/" + "LIST_PAT_S.STL"));

            LoadAndConvert();
        }

        public void LoadAndConvert()
        {
            int typeIdx = 0;
            List<Item> itemList = new List<Item>();

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
                        strTableRow = null;
                        if (i != 0)
                            continue;
                    }

                    var itemData = new Item(i, typeIdx);
                    itemData.Load(curRow, strTableRow);

                    itemList.Add(itemData);
                }
            }

            var jsonString = JsonConvert.SerializeObject(itemList, Formatting.Indented,
                new JsonSerializerSettings {DefaultValueHandling = DefaultValueHandling.Populate});
            
            (new FileInfo("srv_data/item_db.json")).Directory.Create();
            var sqlFile = new System.IO.StreamWriter("srv_data/item_db.json", false);
            using (sqlFile)
            {
                sqlFile.WriteLine(jsonString);
            }
        }
    }
}