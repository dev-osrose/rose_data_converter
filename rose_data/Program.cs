using System;

namespace rose_data
{
    class Program
    {
        private const string StbRoot = "./3DDATA/stb/";
        
        static void Main(string[] args)
        {
            const string skillStb = "list_skill.stb";
            const string skillStl = "list_skill_s.stl";
            var skillConverter = new SkillConverter();
            skillConverter.LoadAndConvert(StbRoot + skillStb, StbRoot + skillStl);
        }
    }
}