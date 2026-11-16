using System.Collections.Generic;
using Revise.STB;

namespace rose_data
{
    /// <summary>
    /// What NPCs and monsters say: QUEST_S.STB (quest reward 012 and 018) and
    /// AI_S.STB (AI action 28) to quest_strings.json and ai_strings.json, as
    /// [{Id, Text}] in English. Id is the data's StrId: the game reads row
    /// StrId - 1 of the language column (English, game column 1).
    /// </summary>
    public class StringConverter
    {
        private const int English = 1;

        public StringConverter(string rootDirectory)
        {
            Export(rootDirectory, "3DDATA/QUESTDATA/QUEST_S.STB", "quest_strings.json");
            Export(rootDirectory, "3DDATA/AI/AI_S.STB", "ai_strings.json");
        }

        private static void Export(string rootDirectory, string path, string output)
        {
            var file = new DataFile { CellEncoding = System.Text.Encoding.Unicode };
            try
            {
                file.Load(GamePath.Resolve(rootDirectory, path));
            }
            catch (System.IO.FileNotFoundException e)
            {
                System.Console.WriteLine(e.Message);
                return;
            }
            var rows = new List<object>();
            for (var row = 0; row < file.RowCount; row++)
            {
                // Cell 0 is the row's name: game column c is cell c + 1.
                var text = file[row][English + 1].TrimEnd('\0').Trim();
                if (!string.IsNullOrWhiteSpace(text)) rows.Add(new { Id = row + 1, Text = text });
            }
            JsonOutput.Write(output, rows);
        }
    }
}
