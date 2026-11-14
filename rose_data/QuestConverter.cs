using System;
using System.Collections.Generic;
using Revise.QSD;

namespace rose_data
{
    /// <summary>
    /// The quest scripts (.QSD, listed in LIST_QUESTDATA.STB) to quest_db.json.
    ///
    /// Each file has patterns of triggers. A trigger is found by its name (NPC
    /// dialogs and other triggers start it); when all its conditions hold, its
    /// rewards are given. If it fails and CheckNext is set, the next trigger
    /// of the same pattern is tried. Every condition and reward has its Type
    /// (the STR_COND_ / STR_REWD_ structure of the game's IO_Quest.h) and the
    /// fields of that structure.
    /// </summary>
    public class QuestConverter
    {
        public class QuestScript
        {
            public string File { get; set; }
            public List<List<Trigger>> Patterns { get; set; } = new List<List<Trigger>>();
        }

        public class Trigger
        {
            public string Name { get; set; }
            public bool CheckNext { get; set; }
            public List<Dictionary<string, object>> Conditions { get; set; }
            public List<Dictionary<string, object>> Rewards { get; set; }
        }

        public QuestConverter(string rootDirectory)
        {
            var list = JsonOutput.LoadTable(rootDirectory, "LIST_QUESTDATA.STB");
            if (list == null) return;

            var scripts = new List<QuestScript>();
            var failed = new List<string>();
            for (var row = 0; row < list.RowCount; row++)
            {
                // Cell 0 is the row's (Korean) name; the game's column 0, the
                // file, is cell 1.
                var path = list[row][1];
                if (string.IsNullOrWhiteSpace(path)) continue;
                var file = new QuestFile();
                try
                {
                    file.Load(GamePath.Resolve(path));
                }
                catch (Exception)
                {
                    failed.Add(path);
                    continue;
                }

                var script = new QuestScript { File = path };
                // Pattern names are Korean and of no use to a server.
                foreach (var pattern in file.Patterns)
                {
                    var triggers = new List<Trigger>();
                    foreach (var trigger in pattern.Triggers)
                        triggers.Add(new Trigger
                        {
                            Name = trigger.Name.TrimEnd('\0'),
                            CheckNext = trigger.CheckNext,
                            Conditions = trigger.Conditions.ConvertAll(Record),
                            Rewards = trigger.Rewards.ConvertAll(Record)
                        });
                    script.Patterns.Add(triggers);
                }
                scripts.Add(script);
            }
            if (failed.Count > 0)
                Console.Error.WriteLine("WARNING: " + failed.Count + " quest files could not be loaded, such as " +
                                        string.Join("; ", failed.GetRange(0, Math.Min(5, failed.Count))));
            JsonOutput.Write("quest_db.json", scripts);
        }

        private static Dictionary<string, object> Record(QuestRecord record)
        {
            var fields = new Dictionary<string, object> { ["Type"] = record.Type };
            foreach (var field in record.Fields)
                fields[field.Key] = field.Value;
            return fields;
        }
    }
}
