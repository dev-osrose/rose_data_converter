using System;

namespace rose_data
{
    class Program
    {
        private const string StbRoot = "./3DDATA/stb/";
        
        static void Main(string[] args)
        {
            var skillConverter = new SkillConverter(StbRoot);
            var itemConverter = new ItemConverter(StbRoot);
        }
    }
}