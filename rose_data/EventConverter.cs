using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Revise.CON;

namespace rose_data
{
    /// <summary>
    /// event_db.json: which quest triggers the client can fire, and from where
    /// (vault: Server/Quest Trigger Audit). Each LIST_EVENT.STB row (an NPC's
    /// dialog id, an event item's conversation) with the trigger names its
    /// conversation's Lua passes to QF_doQuestTrigger; each function of the
    /// client's own scripts (3DDATA/../SCRIPTS) with the names it fires and
    /// the conversation it opens.
    /// </summary>
    public class EventConverter
    {
        public class Event
        {
            public int Id { get; set; }
            public string File { get; set; }
            public List<string> Triggers { get; set; } = new List<string>();
        }

        public class Script
        {
            public string Function { get; set; }
            public List<string> Triggers { get; set; } = new List<string>();
            /// <summary>The LIST_EVENT row of the conversation it opens, 0 none.</summary>
            public int Event { get; set; }
        }

        private static readonly HashSet<string> Fire = new HashSet<string> { "QF_doQuestTrigger", "SC_QF_doQuestTrigger" };

        public EventConverter(string rootDirectory)
        {
            var list = JsonOutput.LoadTable(rootDirectory, "LIST_EVENT.STB");
            if (list == null) return;

            var events = new List<Event>();
            var rowOf = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var failed = new List<string>();
            for (var row = 0; row < list.RowCount; row++)
            {
                // Game column 3: the conversation file.
                var path = list[row][4];
                if (string.IsNullOrWhiteSpace(path)) continue;
                rowOf.TryAdd(FileName(path), row);
                var file = new ConversationFile();
                try
                {
                    file.Load(GamePath.Resolve(rootDirectory, path));
                }
                catch (Exception)
                {
                    failed.Add(path);
                    continue;
                }
                var calls = Lua4Calls.Find(file.Script, Fire);
                if (calls.Any(c => c.Argument == null))
                    Console.Error.WriteLine("WARNING: " + path + " fires a trigger by a name that is not a literal");
                events.Add(new Event
                {
                    Id = row,
                    File = path,
                    Triggers = calls.Where(c => c.Argument != null).Select(c => c.Argument).Distinct().OrderBy(n => n, StringComparer.Ordinal).ToList()
                });
            }
            if (failed.Count > 0)
                Console.Error.WriteLine("WARNING: " + failed.Count + " conversation files could not be loaded, such as " +
                                        string.Join("; ", failed.GetRange(0, Math.Min(5, failed.Count))));

            JsonOutput.Write("event_db.json", new { Events = events, Scripts = Scripts(rootDirectory, rowOf) });
        }

        private static string FileName(string path)
        {
            return path.Replace('\\', '/').Split('/').Last();
        }

        /// <summary>The client's Lua sources (SCRIPTS/*.LUA, next to 3DDATA), function by function.</summary>
        private static List<Script> Scripts(string rootDirectory, Dictionary<string, int> rowOf)
        {
            var scripts = new List<Script>();
            var dir = GamePath.Resolve(rootDirectory, "SCRIPTS");
            if (!Directory.Exists(dir))
            {
                Console.Error.WriteLine("WARNING: no client scripts in " + dir);
                return scripts;
            }
            var function = new Regex(@"^\s*function\s+(\w+)");
            var fire = new Regex(@"SC_QF_doQuestTrigger\s*\(\s*""([^""]+)""");
            var run = new Regex(@"SC_RunEvent\w*\s*\(\s*[^,]+,\s*""([^""]+)""");
            foreach (var path in Directory.GetFiles(dir).Where(p => p.EndsWith(".lua", StringComparison.OrdinalIgnoreCase)).OrderBy(p => p, StringComparer.Ordinal))
            {
                Script current = null;
                foreach (var raw in File.ReadAllLines(path, Encoding.Latin1))
                {
                    var line = raw.Split("--")[0];
                    var m = function.Match(line);
                    if (m.Success)
                    {
                        current = new Script { Function = m.Groups[1].Value };
                        scripts.Add(current);
                    }
                    if (current == null) continue;
                    foreach (Match f in fire.Matches(line))
                        if (!current.Triggers.Contains(f.Groups[1].Value)) current.Triggers.Add(f.Groups[1].Value);
                    var r = run.Match(line);
                    if (r.Success && rowOf.TryGetValue(FileName(r.Groups[1].Value), out var row)) current.Event = row;
                }
            }
            return scripts.Where(s => s.Triggers.Count > 0 || s.Event != 0).ToList();
        }
    }
}
