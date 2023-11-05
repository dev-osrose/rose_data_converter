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
            public NpcSpawner(int npcId, int dialogId, Vector3 pos) : this()
            {
                Set(npcId, dialogId, pos);
            }
            
            public void Set(int npcId, int dialogId, Vector3 pos)
            {
                this.Id = npcId;
                this.DialogId = dialogId;
                this.Position = pos;
            }
            
            public int Id { get; set; }
            public int DialogId { get; set; }
            public Vector3 Position { get; set; }
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
        
        private const int PostionModifier = 100;
        public int Id { get; set; }
        
        public List<SpawnPoint> SpawnPoints { get; set; }
        public List<NpcSpawner> NpcSpawnPoints { get; set; }
        public List<MapMonsterSpawn> MobSpawnPoints { get; set; }

        public Zone(int id)
        {
            Id = id;
        }
        
        public bool Load(DataRow row)
        {
            var zoneName = row[1];
            var zonePath = row[2];

            if (zonePath.Contains(".zon") == false)
                return false;

            var directoryPath = Path.GetDirectoryName(zonePath);
            var fileName = Path.GetFileNameWithoutExtension(zonePath);
            var zoneFile = new ZoneFile();

            try
            {
                zoneFile.Load(Config.RootDirectory + zonePath);
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
                        mapDataFile[x, y].Load(Config.RootDirectory + directoryPath + "\\" + y + "_" + x + ".ifo");
                    }
                    catch (FileNotFoundException)
                    { }
                }
            }

            ExtractSpawnPoints(zoneFile);

            NpcSpawnPoints = new List<NpcSpawner>();
            MobSpawnPoints = new List<MapMonsterSpawn>();
            foreach (var ifo in mapDataFile)
            {
                if (ifo == null) continue;

                var blockX = ifo.ZonePosition.X * PostionModifier;
                var blockY = ifo.ZonePosition.Y * PostionModifier;

                ExtractNpcs(ifo);
                ExtractMobs(ifo);
                // ExtractWarpGates(ifo);
            }

            return true;
        }

        private void ExtractSpawnPoints(ZoneFile zoneFile)
        {
            var zoneDataFile = new DataFile();
            zoneDataFile.Load(Config.RootDirectory + "\\3DDATA\\STB\\" + "list_zone.stb");

            var curMapRow = zoneDataFile[Id];

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
                
                var destCoords = new Vector3(((spawnPoint.Position.X + 520000.00f) / 100.0f), ((spawnPoint.Position.Z + 520000.00f) / 100.0f), ((spawnPoint.Position.Y) / 100.0f));

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

        private void ExtractNpcs(MapDataFile ifo)
        {
            foreach (var npc in ifo.NPCs)
            {
                var eventDataFile = new DataFile();
                eventDataFile.Load(Config.RootDirectory + "\\3DDATA\\STB\\" + "list_event.stb");
                
                int dialogId = 0;
                for (int i = 0; i < eventDataFile.RowCount; i++)
                {
                    if (eventDataFile[i][0] == npc.ConversationFile)
                    {
                        dialogId = i;
                        break;
                    }
                }
                
                var adjPosCoords = new Vector3(((npc.Position.X + 520000.00f) / 100.0f), ((npc.Position.Y + 520000.00f) / 100.0f), ((npc.Position.Z) / 100.0f));
                NpcSpawnPoints.Add(new NpcSpawner(npc.ObjectID, dialogId, adjPosCoords));
            }
        }

        private void ExtractMobs(MapDataFile ifo)
        {
            foreach (var mobSpawns in ifo.MonsterSpawns)
            {
                var adjPosCoords = new Vector3(((mobSpawns.Position.X + 520000.00f) / 100.0f), ((mobSpawns.Position.Y + 520000.00f) / 100.0f), ((mobSpawns.Position.Z) / 100.0f));
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
            zoneDataFile.Load(Config.RootDirectory + "\\3DDATA\\STB\\" + "list_zone.stb");

            var warpDataFile = new DataFile();
            warpDataFile.Load(Config.RootDirectory + "\\3DDATA\\STB\\" + "warp.stb");
            var destCoords = Vector3.Zero;
            
            var modelListFile = new ModelListFile();
            modelListFile.Load(Config.RootDirectory + "\\3DDATA\\special\\" + "list_deco_special.zsc");

            var modelFile = new ModelFile();
            modelFile.Load(Config.RootDirectory + "\\3DDATA\\special\\warp_gate01\\" + "warp.zms");
            var vertices = modelFile.Vertices;

            foreach (var warpGate in ifo.WarpPoints)
            {
                var destMapId = int.Parse(warpDataFile[warpGate.WarpID][2]);
                if (zoneDataFile[destMapId][2].ToString().Contains(".zon"))
                {
                    ZoneFile zoneFile = new ZoneFile();
                    zoneFile.Load(Config.RootDirectory + zoneDataFile[destMapId][2].ToString()); // Load the zon file

                    foreach (var spawnPoint in zoneFile.SpawnPoints)
                    {
                        if (spawnPoint.Name != warpDataFile[warpGate.WarpID][3].ToString()) continue;

                        // rose is stupid and we need to do this to get the right coords
                        destCoords = new Vector3(((spawnPoint.Position.X + 520000.00f) / 100.0f), ((spawnPoint.Position.Z + 520000.00f) / 100.0f), ((spawnPoint.Position.Y) / 100.0f));
                        break;
                    }
                }
                
                var position = new Vector3(((warpGate.Position.X + 520000.00f) / 100.0f), ((warpGate.Position.Y + 520000.00f) / 100.0f), ((warpGate.Position.Z) / 100.0f));

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
                
                // warpList.Add("warp_gate(\"\", " 
                //              + warpDataFile[warpGate.WarpID][2].ToString("G", CultureInfo.InvariantCulture) + ", "
                //              + (destCoords.X).ToString("G", CultureInfo.InvariantCulture) + ", "
                //              + (destCoords.Y).ToString("G", CultureInfo.InvariantCulture) + ", "
                //              + (destCoords.Z).ToString("G", CultureInfo.InvariantCulture) + ", "
                //              + mapId.ToString("G", CultureInfo.InvariantCulture) + ", "
                //              + (boundingBox.Minimum.X).ToString("G", CultureInfo.InvariantCulture) + ", "
                //              + (boundingBox.Minimum.Y).ToString("G", CultureInfo.InvariantCulture) + ", "
                //              + (boundingBox.Minimum.Z).ToString("G", CultureInfo.InvariantCulture) + ", "
                //              + (boundingBox.Maximum.X).ToString("G", CultureInfo.InvariantCulture) + ", "
                //              + (boundingBox.Maximum.Y).ToString("G", CultureInfo.InvariantCulture) + ", "
                //              + (boundingBox.Maximum.Z).ToString("G", CultureInfo.InvariantCulture) + ");\n");
            }
        }
    }
}