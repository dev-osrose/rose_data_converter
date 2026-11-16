using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Numerics;
using Revise;
using Revise.IFO;
using Revise.IFO.Blocks;
using Revise.STB;
using Revise.ZMO;
using Revise.ZMS;
using Revise.ZON;
using Revise.ZSC;

namespace rose_data.Data
{
    public class Zone
    {
        public struct SpawnPoint
        {
            public enum PointType
            {
                Respawn = 1,
                Revive,
                Start
            }
            
            public SpawnPoint(PointType type, Vector3 pos) : this()
            {
                Set(type, pos);
            }
            
            public void Set(PointType type, Vector3 pos)
            {
                this.Type = type;
                this.Position = pos;
            }

            public PointType Type { get; set; }
            public Vector3 Position { get; set; }
        }

        public struct NpcSpawner
        {
            public NpcSpawner(int npcId, int dialogId, Vector3 pos, float angle) : this()
            {
                Set(npcId, dialogId, pos, angle);
            }
            
            public void Set(int npcId, int dialogId, Vector3 pos, float angle)
            {
                this.Id = npcId;
                this.DialogId = dialogId;
                this.Position = pos;
                this.Angle = angle;
            }
            
            public int Id { get; set; }
            public int DialogId { get; set; }
            public Vector3 Position { get; set; }
            public float Angle { get; set; }
        }
        
        /// <summary>
        /// An event object (a map block IFO's lump 12 with a non-zero event
        /// id): the server keeps its variables and status; the client has its
        /// model and script. The map block (the IFO file's X and Y)
        /// and the event id are what quest condition 012 names it by.
        /// </summary>
        public class EventObject
        {
            public int EventId { get; set; }
            public int BlockX { get; set; }
            public int BlockY { get; set; }
            public Vector3 Position { get; set; }
            public string Trigger { get; set; }
            public string Script { get; set; }
        }

        public struct MobSpawner
        {
            public MobSpawner(int dialogId, Vector3 pos) : this()
            {
                Set(dialogId, pos);
            }
            
            public void Set(int dialogId, Vector3 pos)
            {
                this.DialogId = dialogId;
                this.Position = pos;
            }
            
            public int DialogId { get; set; }
            public Vector3 Position { get; set; }
        }
        
        public class WarpPoint
        {
            public WarpPoint(string alias, int warpId, int destinationGateId, Vector3 destination, int mapId, Vector3 minPos, Vector3 maxPos)
            {
                WarpId = warpId;
                Set(alias, destinationGateId, destination, mapId, minPos, maxPos);
            }

            public void Set(string alias, int destinationGateId, Vector3 destination, int mapId, Vector3 minPos, Vector3 maxPos)
            {
                this.Alias = alias;
                this.DestinationGateId = destinationGateId;
                this.Destination = destination;
                this.MapId = mapId;
                this.MinPosition = minPos;
                this.MaxPosition = maxPos;
            }
            
            
            public string Alias { get; set; }
            /// <summary>The row of WARP.STB the gate leads through: what the
            /// client sends in PAKCS_TELEPORT_REQ when it walks into it.</summary>
            public int WarpId { get; set; }
            public int DestinationGateId { get; set; }
            public Vector3 Destination { get; set; }
            public int MapId { get; set; }
            public Vector3 MinPosition { get; set; }
            public Vector3 MaxPosition { get; set; }
        }
        
        private const int PostionModifier = 100;
        public int Id { get; set; }

        /// <summary>
        /// ZONE_PARTY_EXP_A and _B (game columns 28 and 29): how a party's own
        /// exp grows with kills on this map.
        /// </summary>
        public int PartyExpA { get; set; }
        public int PartyExpB { get; set; }

        /// <summary>
        /// ZONE_RIDING_REFUSE_FLAG (game column 30): 1 no carts, 2 no castle
        /// gear, 3 neither.
        /// </summary>
        public int RidingRefused { get; set; }

        /// <summary>
        /// ZONE_PLANET_NO (game column 19): a saved revive place only counts
        /// on a map of the same planet.
        /// </summary>
        public int Planet { get; set; }

        /// <summary>
        /// ZONE_SECTOR_SIZE (game column 25): the original server's sector
        /// size here, which a shout reaches three of around the shouter.
        /// </summary>
        public int SectorSize { get; set; }

        /// <summary>
        /// ZONE_DAY_CYCLE and the starts of morning, day, evening and night
        /// (game columns 13-17), in world ticks of 10 s: the zone's time is
        /// the world time (within its month) modulo the cycle.
        /// </summary>
        public int DayCycle { get; set; }
        public int MorningTime { get; set; }
        public int DayTime { get; set; }
        public int EveningTime { get; set; }
        public int NightTime { get; set; }
        
        public List<SpawnPoint> SpawnPoints { get; set; }
        public List<NpcSpawner> NpcSpawnPoints { get; set; }
        public List<MapMonsterSpawn> MobSpawnPoints { get; set; }
        public List<WarpPoint> WarpPoints { get; set; }
        public List<EventObject> EventObjects { get; set; }

        public Zone(int id)
        {
            Id = id;
        }
        
        public bool Load(DataRow row)
        {
            var zoneName = row[1];
            // As the game names it: backslashes, any case, relative to the root.
            var zonePath = row[2];

            if (zonePath.Contains(".zon", StringComparison.OrdinalIgnoreCase) == false)
                return false;

            // The directory as it is on disk. Resolved once: the zone's map
            // blocks are looked up in it by name.
            var zoneFilePath = GamePath.Resolve(zonePath);
            var directoryPath = Path.GetDirectoryName(zoneFilePath);
            var zoneFile = new ZoneFile();

            try
            {
                Console.Write("Attempting to load \"" + zoneFilePath + "\" - ");
                zoneFile.Load(zoneFilePath);
            }
            catch (FileNotFoundException)
            {
                Console.Write("Failed!\n");
                return false;
            }
            Console.Write("Successful!\n");

            var mapDataFile = new MapDataFile[65, 65];
            for (var x = 0; x < zoneFile.Width; x++)
            {
                for (var y = 0; y < zoneFile.Height; y++)
                {
                    if (!zoneFile.Positions[x, y].IsUsed) continue;

                    mapDataFile[x, y] = new MapDataFile();
                    try
                    {
                        mapDataFile[x, y].Load(GamePath.Resolve(directoryPath, y + "_" + x + ".ifo"));
                    }
                    catch (FileNotFoundException)
                    { }
                }
            }

            ExtractSpawnPoints(zoneFile);

            NpcSpawnPoints = new List<NpcSpawner>();
            MobSpawnPoints = new List<MapMonsterSpawn>();
            WarpPoints = new List<WarpPoint>();
            EventObjects = new List<EventObject>();
            foreach (var ifo in mapDataFile)
            {
                if (ifo == null) continue;

                var blockX = ifo.ZonePosition.X * PostionModifier;
                var blockY = ifo.ZonePosition.Y * PostionModifier;

                ExtractNpcs(ifo);
                ExtractMobs(ifo);
                ExtractWarpGates(ifo);
                foreach (var e in ifo.EventObjects)
                {
                    if (e.EventID == 0) continue;
                    EventObjects.Add(new EventObject
                    {
                        EventId = e.EventID,
                        BlockX = ifo.ZonePosition.X,
                        BlockY = ifo.ZonePosition.Y,
                        Position = new Vector3(e.Position.X + 520000.00f, e.Position.Y + 520000.00f, e.Position.Z),
                        Trigger = e.FunctionName.TrimEnd('\0'),
                        Script = e.ConversationFile.TrimEnd('\0')
                    });
                }
            }

            return true;
        }

        private void ExtractSpawnPoints(ZoneFile zoneFile)
        {
            var zoneDataFile = new DataFile();
            zoneDataFile.Load(GamePath.Resolve("3DDATA/STB/LIST_ZONE.STB"));

            var curMapRow = zoneDataFile[Id];

            // Cell n + 1 is the game's column n.
            int.TryParse(curMapRow[29], out var partyExpA);
            int.TryParse(curMapRow[30], out var partyExpB);
            PartyExpA = partyExpA;
            PartyExpB = partyExpB;
            int.TryParse(curMapRow[31], out var ridingRefused);
            RidingRefused = ridingRefused;
            int.TryParse(curMapRow[20], out var planet);
            Planet = planet;
            int.TryParse(curMapRow[26], out var sectorSize);
            SectorSize = sectorSize;
            int.TryParse(curMapRow[14], out var dayCycle);
            int.TryParse(curMapRow[15], out var morning);
            int.TryParse(curMapRow[16], out var day);
            int.TryParse(curMapRow[17], out var evening);
            int.TryParse(curMapRow[18], out var night);
            (DayCycle, MorningTime, DayTime, EveningTime, NightTime) = (dayCycle, morning, day, evening, night);

            var reviveMap = curMapRow[32];
            double.TryParse(curMapRow[33], out var reviveX);
            double.TryParse(curMapRow[34], out var reviveY);

            SpawnPoints = new List<SpawnPoint>();
            if (reviveMap.Length > 0)
            {
                reviveX = (reviveX * 1000.0f);
                reviveY = (reviveY * 1000.0f);
                SpawnPoints.Add(new SpawnPoint(SpawnPoint.PointType.Revive, new Vector3((float)reviveX, (float)reviveY, 0)));
            }

            foreach (var spawnPoint in zoneFile.SpawnPoints)
            {
                if (spawnPoint.Name.Contains("WARP")) continue;
                
                var destCoords = new Vector3(((spawnPoint.Position.X + 520000.00f)), ((spawnPoint.Position.Z + 520000.00f)), ((spawnPoint.Position.Y)));

                if (spawnPoint.Name.Contains("start"))
                {
                    SpawnPoints.Add(new SpawnPoint(SpawnPoint.PointType.Start, destCoords));
                }
                else
                {
                    SpawnPoints.Add(new SpawnPoint(SpawnPoint.PointType.Respawn, destCoords));
                }
            }
        }
        
        private float GetAngleFromRotation(Quaternion rotation)
        {
            rotation = Quaternion.Normalize(rotation);
            double w = rotation.W;
            w = Math.Clamp(w, -1.0, 1.0);
            double angleRadians = Math.Acos(w) * 2.0;
            float angleDegrees = (float)(angleRadians * (180.0 / Math.PI));
            
            return angleDegrees;
        }

        private void ExtractNpcs(MapDataFile ifo)
        {
            foreach (var npc in ifo.NPCs)
            {
                var eventDataFile = new DataFile();
                eventDataFile.Load(GamePath.Resolve("3DDATA/STB/list_event.stb"));
                
                int dialogId = 0;
                for (int i = 0; i < eventDataFile.RowCount; i++)
                {
                    if (eventDataFile[i][0] == npc.ConversationFile)
                    {
                        dialogId = i;
                        break;
                    }
                }
                
                var angle = GetAngleFromRotation(npc.Rotation);
                var adjPosCoords = new Vector3(((npc.Position.X + 520000.00f)), ((npc.Position.Y + 520000.00f)), ((npc.Position.Z)));
                NpcSpawnPoints.Add(new NpcSpawner(npc.ObjectID, dialogId, adjPosCoords, angle));
            }
        }

        private void ExtractMobs(MapDataFile ifo)
        {
            foreach (var mobSpawns in ifo.MonsterSpawns)
            {
                var adjPosCoords = new Vector3(((mobSpawns.Position.X + 520000.00f)), ((mobSpawns.Position.Y + 520000.00f)), ((mobSpawns.Position.Z)));
                mobSpawns.Position = adjPosCoords;
                MobSpawnPoints.Add(mobSpawns);
                // foreach (var normalMobs in mobSpawns.NormalSpawnPoints)
                // {
                //     // MobSpawnPoints.Add(new MobSpawner(dialogId, adjPosCoords));
                // }
                //
                // foreach (var tacticalMobs in mobSpawns.TacticalSpawnPoints)
                // {
                //     // MobSpawnPoints.Add(new MobSpawner(dialogId, adjPosCoords));
                // }
            }
        }

        private void ExtractWarpGates(MapDataFile ifo)
        {
            
            var zoneDataFile = new DataFile();
            zoneDataFile.Load(GamePath.Resolve("3DDATA/STB/list_zone.stb"));

            var warpDataFile = new DataFile();
            warpDataFile.Load(GamePath.Resolve("3DDATA/STB/warp.stb"));
            var modelListFile = new ModelListFile();
            modelListFile.Load(GamePath.Resolve("3DDATA/special/list_deco_special.zsc"));

            var modelFile = new ModelFile();
            modelFile.Load(GamePath.Resolve("3DDATA/special/warp_gate01/warp.zms"));
            var vertices = modelFile.Vertices;

            foreach (var warpGate in ifo.WarpPoints)
            {
                // Each gate's own destination: one that cannot be found is
                // left out rather than given the previous gate's.
                Vector3? destCoords = null;
                var destMapId = int.Parse(warpDataFile[warpGate.WarpID][2]);
                if (zoneDataFile[destMapId][2].ToString().Contains(".zon", StringComparison.OrdinalIgnoreCase))
                {
                    ZoneFile zoneFile = new ZoneFile();
                    zoneFile.Load(GamePath.Resolve(zoneDataFile[destMapId][2].ToString())); // Load the zon file

                    foreach (var spawnPoint in zoneFile.SpawnPoints)
                    {
                        if (spawnPoint.Name != warpDataFile[warpGate.WarpID][3].ToString()) continue;

                        // rose is stupid and we need to do this to get the right coords
                        destCoords = new Vector3(((spawnPoint.Position.X + 520000.00f)), ((spawnPoint.Position.Z + 520000.00f)), ((spawnPoint.Position.Y) ));
                        break;
                    }
                }
                
                if (destCoords == null)
                {
                    Console.WriteLine($"warp {warpGate.WarpID} on map {Id}: destination \"{warpDataFile[warpGate.WarpID][3]}\" on map {destMapId} not found; gate left out");
                    continue;
                }

                var position = new Vector3(((warpGate.Position.X + 520000.00f)), ((warpGate.Position.Y + 520000.00f)), ((warpGate.Position.Z)));

                var rot = Matrix4x4.CreateFromQuaternion(modelListFile.Objects[1].Parts[0].Rotation);
                var scale = Matrix4x4.CreateScale(modelListFile.Objects[1].Parts[0].Scale);
                var trans = Matrix4x4.CreateTranslation(modelListFile.Objects[1].Parts[0].Position);
                
                var world = rot * scale * trans;
                var objRot = Matrix4x4.CreateFromQuaternion(warpGate.Rotation);
                var objScale = Matrix4x4.CreateScale(warpGate.Scale);
                var objTrans = Matrix4x4.CreateTranslation(position);
                
                var objectWorld = objRot * objScale * objTrans;

                Vector3[] vectorPositions = new Vector3[vertices.Count];
                for (int i = 0; i < vertices.Count; i++)
                    vectorPositions[i] = (Vector3)Vector3.Transform(vertices[i].Position, world * objectWorld);
                
                var boundingBox = BoundingBox.FromPoints(vectorPositions);
                
                WarpPoints.Add(new WarpPoint("", warpGate.WarpID, destMapId, destCoords.Value, Id, boundingBox.Minimum, boundingBox.Maximum));
            }
        }
    }
}