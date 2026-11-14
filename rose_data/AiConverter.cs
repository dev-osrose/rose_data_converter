using System;
using System.Collections.Generic;
using Revise.AIP;

namespace rose_data
{
    /// <summary>
    /// The monster AI scripts (.AIP, listed in FILE_AI.STB; npc_db.json's
    /// AiType is the index) to ai_db.json.
    ///
    /// Each file has patterns, one per event, in the order CREATED, STOP,
    /// ATTACKMOVE, DAMAGED, KILL, DEAD. A pattern's events are tried in order;
    /// the first whose conditions all hold runs its actions, and ends the
    /// pattern. IdleInterval is the seconds between two STOP checks, DamageRate
    /// the percent chance DAMAGED is checked while the monster already has a
    /// target. Distances are in metres as in the files (the game multiplies
    /// them by 100).
    /// </summary>
    public class AiConverter
    {
        /// <summary>What each pattern of a file reacts to, in file order.</summary>
        private static readonly string[] PatternEvents = { "Created", "Stop", "AttackMove", "Damaged", "Kill", "Dead" };

        public class Ai
        {
            public int Id { get; set; }
            public string File { get; set; }
            public int IdleInterval { get; set; }
            public int DamageRate { get; set; }
            public List<Pattern> Patterns { get; set; } = new List<Pattern>();
        }

        public class Pattern
        {
            /// <summary>The event the pattern reacts to.</summary>
            public string On { get; set; }
            /// <summary>Tried in order; the first whose conditions all hold runs.</summary>
            public List<Rule> Rules { get; set; } = new List<Rule>();
        }

        public class Rule
        {
            public List<object> Conditions { get; set; }
            public List<object> Actions { get; set; }
        }

        /// <param name="files">The AI file names by AiType, as FILE_AI.STB lists them.</param>
        public AiConverter(IList<string> files)
        {
            var list = new List<Ai>();
            var failed = new List<string>();
            for (var i = 0; i < files.Count; i++)
            {
                var path = files[i];
                if (string.IsNullOrWhiteSpace(path)) continue;
                var file = new ArtificialIntelligenceFile();
                try
                {
                    file.Load(GamePath.Resolve(path));
                }
                catch (Exception)
                {
                    failed.Add(i + ": " + path);
                    continue;
                }
                // The names in the files are Korean and of no use to a server.
                var ai = new Ai
                {
                    Id = i,
                    File = path,
                    IdleInterval = file.IdleInterval,
                    DamageRate = file.DamageRate
                };
                for (var p = 0; p < file.Patterns.Count; p++)
                {
                    var pattern = new Pattern { On = p < PatternEvents.Length ? PatternEvents[p] : "Pattern" + p };
                    foreach (var ev in file.Patterns[p].Events)
                        pattern.Rules.Add(new Rule
                        {
                            Conditions = ev.Conditions.ConvertAll(c => (object)c),
                            Actions = ev.Actions.ConvertAll(a => (object)a)
                        });
                    ai.Patterns.Add(pattern);
                }
                list.Add(ai);
            }
            if (failed.Count > 0)
                Console.Error.WriteLine("WARNING: " + failed.Count + " AI files could not be loaded: " + string.Join("; ", failed));
            JsonOutput.Write("ai_db.json", list);
        }
    }
}
