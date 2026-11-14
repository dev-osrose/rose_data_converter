using System.Collections.Generic;

namespace rose_data
{
    /// <summary>
    /// Small tables the server needs as they are:
    ///
    /// - skill_points.json (LIST_SKILL_P.STB): the skill points reaching a
    ///   level gives (SP_LEVEL, SP_POINT).
    /// - grade_db.json (LIST_GRADE.STB): what each refine grade of an item adds
    ///   (ITEMGRADE_ATK, _HIT, _DEF, _RES, _AVOID, _GLOW_COLOR).
    /// - drop_db.json (ITEM_DROP.STB): drop tables, by drop type (npc_db.json
    ///   DropType) or zone. Values[c] is the game's column c, so its
    ///   DROPITEM_ITEMNO(row, n) is Values[1 + n].
    /// </summary>
    public class TableConverter
    {
        public class SkillPoints
        {
            public int Level { get; set; }
            public int Points { get; set; }
        }

        public class Grade
        {
            public int Id { get; set; }
            public int Attack { get; set; }
            public int Hit { get; set; }
            public int Defense { get; set; }
            public int Resistance { get; set; }
            public int Avoid { get; set; }
            public int GlowColor { get; set; }
        }

        public class DropTable
        {
            public int Id { get; set; }
            public List<int> Values { get; set; }
        }

        public TableConverter(string rootDirectory)
        {
            // Loaded with a name column: the game's column 0 is cell 1.
            var file = JsonOutput.LoadTable(rootDirectory, "LIST_SKILL_P.STB");
            if (file != null)
            {
                var skillPoints = new List<SkillPoints>();
                for (var row = 0; row < file.RowCount; row++)
                {
                    var level = JsonOutput.Int(file[row][1]);
                    if (level > 0)
                        skillPoints.Add(new SkillPoints { Level = level, Points = JsonOutput.Int(file[row][2]) });
                }
                JsonOutput.Write("skill_points.json", skillPoints);
            }

            file = JsonOutput.LoadTable(rootDirectory, "LIST_GRADE.STB");
            if (file != null)
            {
                var grades = new List<Grade>();
                for (var row = 0; row < file.RowCount; row++)
                {
                    var values = JsonOutput.Row(file, row);
                    int Cell(int column) => column < values.Count ? values[column] : 0;
                    grades.Add(new Grade
                    {
                        Id = row, Attack = Cell(0), Hit = Cell(1), Defense = Cell(2),
                        Resistance = Cell(3), Avoid = Cell(4), GlowColor = Cell(5)
                    });
                }
                JsonOutput.Write("grade_db.json", grades);
            }

            file = JsonOutput.LoadTable(rootDirectory, "ITEM_DROP.STB");
            if (file != null)
            {
                var drops = new List<DropTable>();
                for (var row = 0; row < file.RowCount; row++)
                    drops.Add(new DropTable { Id = row, Values = JsonOutput.Row(file, row) });
                JsonOutput.Write("drop_db.json", drops);
            }
        }
    }
}
