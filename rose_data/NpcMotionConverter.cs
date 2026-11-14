using System;
using System.Collections.Generic;
using System.IO;
using Revise.CHR;
using rose_data.Data;

namespace rose_data
{
    /// <summary>
    /// LIST_NPC.CHR to npc_motions.json: the motions of every NPC and monster
    /// (the character index is the npc_db.json id), with their length and the
    /// frames an attack lands on. The server needs the attack motion for the
    /// time between two swings, and the death motion for how long a killed
    /// monster lies there.
    /// </summary>
    public class NpcMotionConverter
    {
        public class NpcMotions
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<Animation> Animations { get; set; } = new List<Animation>();
        }

        public class Animation
        {
            public AnimationType Type { get; set; }
            public MotionTiming Motion { get; set; }
        }

        public NpcMotionConverter(string rootDirectory)
        {
            var file = new CharacterFile();
            try
            {
                file.Load(GamePath.Resolve(rootDirectory, "3DDATA/NPC/LIST_NPC.CHR"));
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            var timings = new Dictionary<int, MotionTiming>();
            var list = new List<NpcMotions>();
            for (var id = 0; id < file.Characters.Count; id++)
            {
                var character = file.Characters[id];
                if (!character.IsEnabled) continue;
                var npc = new NpcMotions { Id = id, Name = character.Name };
                foreach (var animation in character.Animations)
                {
                    if (animation.Animation < 0 || animation.Animation >= file.MotionFiles.Count) continue;
                    if (!timings.TryGetValue(animation.Animation, out var timing))
                    {
                        timing = MotionTiming.Load(file.MotionFiles[animation.Animation]);
                        timings[animation.Animation] = timing;
                    }
                    if (timing == null) continue;
                    npc.Animations.Add(new Animation { Type = animation.Type, Motion = timing });
                }
                list.Add(npc);
            }
            JsonOutput.Write("npc_motions.json", list);
        }
    }
}
