using System.Collections.Generic;
using Revise.ZMO;

namespace rose_data.Data
{
    /// <summary>
    /// How long a motion plays and where its events are, from its ZMO file.
    /// </summary>
    public class MotionTiming
    {
        public string File { get; set; }
        public int FrameCount { get; set; }
        public int FramesPerSecond { get; set; }

        /// <summary>
        /// Frames at which an attack lands. The original server deals the
        /// damage of a swing once per such frame (at least once); the client
        /// shows it at them.
        /// </summary>
        public int AttackFrames { get; set; }

        /// <summary>Every frame with an event, and the event.</summary>
        public List<FrameEvent> Events { get; set; } = new List<FrameEvent>();

        public struct FrameEvent
        {
            public int Frame { get; set; }
            public int Event { get; set; }
        }

        /// <summary>Reads a motion file; null when it is missing or broken.</summary>
        public static MotionTiming Load(string gamePath)
        {
            if (string.IsNullOrEmpty(gamePath))
                return null;
            var motion = new MotionFile();
            try
            {
                motion.Load(GamePath.Resolve(gamePath));
            }
            catch (System.Exception)
            {
                return null;
            }

            return From(motion, gamePath);
        }

        /// <summary>The timing of a loaded motion file.</summary>
        public static MotionTiming From(MotionFile motion, string gamePath)
        {
            var timing = new MotionTiming
            {
                File = gamePath,
                FrameCount = motion.FrameCount,
                FramesPerSecond = motion.FramesPerSecond,
                AttackFrames = motion.AttackFrameCount
            };
            if (motion.FrameEvents != null)
            {
                for (var frame = 0; frame < motion.FrameEvents.Length; frame++)
                {
                    var ev = motion.FrameEvents[frame];
                    if (ev == 0) continue;
                    timing.Events.Add(new FrameEvent { Frame = frame, Event = ev });
                }
            }
            return timing;
        }
    }
}
