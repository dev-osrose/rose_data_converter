using System;
using System.IO;

namespace rose_data
{
    class Program
    {
        private const string StbRoot = "./3DDATA/stb/";
        
        static void Main(string[] args)
        {
            (new FileInfo("srv_data\\skill_db.json")).Directory.Create();
            var skillConverter = new SkillConverter(StbRoot);
            (new FileInfo("srv_data\\item_db.json")).Directory.Create();
            var itemConverter = new ItemConverter(StbRoot);
            (new FileInfo("srv_data\\npc_db.json")).Directory.Create();
            // var npcConverter = new NpcConverter(StbRoot);
            (new FileInfo("srv_data\\zone_db.json")).Directory.Create();
            // var zoneConverter = new ZoneConverter(StbRoot);
            
            #if RELEASE
            Console.Write("Done extracting. Press any key to exit...\n");
            Console.ReadLine();
            #endif
        }
    }
}