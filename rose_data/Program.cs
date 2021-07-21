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

    public static class Config
    {
        public static bool Verbose { get; set; }
        public static string RootDirectory { get; set; }
        public static MotionList AnimationList { get; set; }
        public static AiList AiList { get; set; }
    }
    
    class Program
    {

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o =>
                {
                    if (o.Root.Length > 0)
                    {
                        Config.RootDirectory = o.Root;
                    }
                    
                    if (o.Verbose)
                    {
                        Console.WriteLine($"Verbose output enabled. Current Arguments: -v {o.Verbose}");
                    }
                });
            
            if (Config.RootDirectory.Length == 0)
            {
                Console.Write("Root directory required\n");
                return;
            }
            
            Config.AnimationList = new MotionList(Config.RootDirectory);
            Config.AiList = new AiList(Config.RootDirectory);
            var skillConverter = new SkillConverter(Config.RootDirectory);
            var itemConverter = new ItemConverter(Config.RootDirectory);
            var npcConverter = new NpcConverter(Config.RootDirectory);
            // (new FileInfo("srv_data\\zone_db.json")).Directory.Create();
            // var zoneConverter = new ZoneConverter(StbRoot);
            
#if RELEASE
            Console.Write("Done extracting. Press any key to exit...\n");
            Console.ReadLine();
#endif
        }
    }
}