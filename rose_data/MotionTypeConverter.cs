using System.Collections.Generic;

namespace rose_data
{
    /// <summary>
    /// TYPE_MOTION.STB to motion_types.json: which motion (a row of
    /// FILE_MOTION.STB, i.e. an id of motion_ref.json) an avatar plays for an
    /// action with a kind of weapon. `Motions[t]` is the motion for weapon
    /// motion type t (the weapons' MotionType in item_db.json), as the game's
    /// FILE_MOTION(t, action) reads it. Actions follow AVT_ANI_*: 8 is attack.
    /// </summary>
    public class MotionTypeConverter
    {
        public class Action
        {
            public int Id { get; set; }
            public List<int> Motions { get; set; }
        }

        public MotionTypeConverter(string rootDirectory)
        {
            var file = JsonOutput.LoadTable(rootDirectory, "TYPE_MOTION.STB");
            if (file == null) return;

            // Loaded without a name column, so the first cell is column 0.
            var actions = new List<Action>();
            for (var i = 0; i < file.RowCount; i++)
                actions.Add(new Action { Id = i, Motions = JsonOutput.Row(file, i) });
            JsonOutput.Write("motion_types.json", actions);
        }
    }
}
