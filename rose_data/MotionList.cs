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
    public class MotionList
    {
        public List<Motion> AnimationList { get; set; } = new List<Motion>();
        
        public MotionList(string rootDirectory)
        {
            const string motionStb = "FILE_MOTION.stb";
            LoadAndConvert(rootDirectory + "\\3DDATA\\STB\\" + motionStb);
        }

        public Motion GetById(int id)
        {
            return AnimationList.Find(motion => motion.Id == id);
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
            
            for (var i = 1; i < dataFile.RowCount; i++)
            {
                var motion = new Motion(i);
                var valid = motion.Load(dataFile[i]);
                if (valid) AnimationList.Add(motion);
            }

            var jsonString = JsonConvert.SerializeObject(AnimationList, Formatting.Indented,
                new JsonSerializerSettings {DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate});
            
            (new FileInfo("srv_data\\motion_db.json")).Directory.Create();
            var sqlFile = new System.IO.StreamWriter("srv_data\\motion_db.json", false);
            using (sqlFile)
            {
                sqlFile.WriteLine(jsonString);
            }
        }
    }
}