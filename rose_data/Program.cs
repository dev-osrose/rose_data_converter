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
            
            Console.WriteLine("Loading animations...");
            Config.AnimationList = new MotionList(Config.RootDirectory);
            Console.WriteLine("Loading ai lists...");
            Config.AiList = new AiList(Config.RootDirectory);
            Console.WriteLine("Loading skills...");
            var skillConverter = new SkillConverter(Config.RootDirectory);
            Console.WriteLine("Loading items...");
            var itemConverter = new ItemConverter(Config.RootDirectory);
            Console.WriteLine("Loading npcs...");
            var npcConverter = new NpcConverter(Config.RootDirectory);
            Console.WriteLine("Loading zones...");
            var zoneConverter = new ZoneConverter(Config.RootDirectory);
            Console.WriteLine("Loading weapon motion types...");
            new MotionTypeConverter(Config.RootDirectory);
            Console.WriteLine("Loading NPC motions...");
            new NpcMotionConverter(Config.RootDirectory);
            Console.WriteLine("Loading AI scripts...");
            new AiConverter(Config.AiList.AiFileList);
            Console.WriteLine("Loading skill points, grades and drop tables...");
            new TableConverter(Config.RootDirectory);
            Console.WriteLine("Loading quest scripts...");
            new QuestConverter(Config.RootDirectory);
            new StringConverter(Config.RootDirectory);
            Console.WriteLine("Loading conversations and client scripts...");
            new EventConverter(Config.RootDirectory);
            
#if RELEASE
            Console.Write("Done extracting. Press any key to exit...\n");
            Console.ReadLine();
#endif
        }
    }
}