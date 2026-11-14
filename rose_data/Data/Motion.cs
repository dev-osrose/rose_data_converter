using System;
using System.Collections.Generic;
using System.IO;
using Revise.STB;
using Revise.ZMO;

namespace rose_data.Data
{
    public class Motion
    {
        private Animation _male;
        private Animation _female;

        public struct Animation
        {
            public Animation(int frameCount, int framesPerSecond) : this()
            {
                Set(frameCount, framesPerSecond);
            }

            public void Set(int frameCount, int framesPerSecond)
            {
                FrameCount = frameCount;
                FramesPerSecond = framesPerSecond < 1 ? 1 : framesPerSecond;
                TotalFrameTime = FrameCount / FramesPerSecond;
            }

            /// <summary>Takes the length, speed and frame events of a motion.</summary>
            public void Set(MotionFile motion)
            {
                Set(motion.FrameCount, motion.FramesPerSecond);
                var timing = MotionTiming.From(motion, null);
                AttackFrames = timing.AttackFrames;
                Events = timing.Events;
            }

            /// <summary>Frames at which an attack lands.</summary>
            public int AttackFrames { get; set; }
            /// <summary>Every frame with an event, and the event.</summary>
            public List<MotionTiming.FrameEvent> Events { get; set; }
            
            public float FrameCount { get; set; }
            public float FramesPerSecond { get; set; }
            public float TotalFrameTime { get; set; }
        }
        
        public int Id { get; set; }

        public Animation Male
        {
            get => _male;
            set => _male = value;
        }

        public Animation Female
        {
            get => _female;
            set => _female = value;
        }


        public Motion(int id)
        {
            Id = id;
        }
        
        public bool Load(DataRow row)
        {
            var maleZMOPath = row[1];
            var femaleZMOPath = row[2];

            if (maleZMOPath == "" && femaleZMOPath == "")
                return false;

            var maleZMO = new MotionFile();
            var femaleZMO = new MotionFile();

            try
            {
                if (maleZMOPath != "")
                    maleZMO.Load(GamePath.Resolve(maleZMOPath));
                // Without a motion of its own, the female uses the male one.
                if (femaleZMOPath != "")
                    femaleZMO.Load(GamePath.Resolve(femaleZMOPath));
                else
                    femaleZMO = maleZMO;
            }
            catch (FileNotFoundException)
            {
            }

            if (maleZMO.FrameCount == 0 && femaleZMO.FrameCount == 0)
                return false;

            if (maleZMO.FrameCount > 0)
                _male.Set(maleZMO);

            if (femaleZMO.FrameCount > 0)
                _female.Set(femaleZMO);
            
            return true;
        }
    }
}