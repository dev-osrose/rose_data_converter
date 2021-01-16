using System;
using System.IO;
using CommandLine;

namespace rose_data
{
    public class Options
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }
        
        [Option('r', "root", Required = true, HelpText = "Set 3DDATA root directory.")]
        public string Root { get; set; }
    }
    
    class Program
    {

        static void Main(string[] args)
        {
            var StbRoot = "";
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o =>
                {
                    if (o.Root.Length > 0)
                    {
                        StbRoot = o.Root;
                    }
                    
                    if (o.Verbose)
                    {
                        Console.WriteLine($"Verbose output enabled. Current Arguments: -v {o.Verbose}");
                    }
                });
            
            if (StbRoot.Length == 0)
            {
                Console.Write("Root directory required\n");
                return;
            }
            
            var skillConverter = new SkillConverter(StbRoot);
            var itemConverter = new ItemConverter(StbRoot);
            // (new FileInfo("srv_data\\npc_db.json")).Directory.Create();
            // var npcConverter = new NpcConverter(StbRoot);
            // (new FileInfo("srv_data\\zone_db.json")).Directory.Create();
            // var zoneConverter = new ZoneConverter(StbRoot);
            
#if RELEASE
            Console.Write("Done extracting. Press any key to exit...\n");
            Console.ReadLine();
#endif
        }
    }
}