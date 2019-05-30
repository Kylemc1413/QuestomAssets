﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BeatmapAssetMaker.AssetsChanger
{
    public class AssetsMetadata
    {
        public AssetsMetadata(AssetsReader reader)
        {
            Types = new List<AssetsType>();
            ObjectInfos = new List<ObjectInfo>();
            Adds = new List<PPtr>();
            ExternalFiles = new List<ExternalFile>();
            Parse(reader);
        }
        public string Version { get; set; }
        public Int32 Platform { get; set; }
        public bool HasTypeTrees { get; set; }
        public List<AssetsType> Types { get; set; }
        public List<ObjectInfo> ObjectInfos { get; set; }
        public List<PPtr> Adds { get; set; }
        public List<ExternalFile> ExternalFiles { get; set; }

        public void Parse(AssetsReader reader)
        {
            Version = reader.ReadCStr();
            Platform = reader.ReadInt32();
            HasTypeTrees = reader.ReadBoolean();
            int numTypes = reader.ReadInt32();
            for (int i = 0; i < numTypes; i++)
            {
                Types.Add(new AssetsType(reader, HasTypeTrees));
            }
            int numObj = reader.ReadInt32();
            for (int i = 0; i < numObj; i++)
            {
                reader.AlignTo(4);
                ObjectInfos.Add(new ObjectInfo(reader));
            }
            int numAdds = reader.ReadInt32();
            for (int i = 0; i < numAdds; i++)
            {
                reader.AlignTo(4);
                Adds.Add(new PPtr(reader));
            }
            int numExt = reader.ReadInt32();
            for (int i = 0; i < numExt; i++)
            {
                ExternalFiles.Add(new ExternalFile(reader));
            }
            reader.ReadCStr();
        }

        public void Write(AssetsWriter writer)
        {
            writer.WriteCString(Version);
            writer.Write(Platform);
            writer.Write(HasTypeTrees);
            writer.Write(Types.Count());
            Types.ForEach(x => x.Write(writer));
            writer.Write(ObjectInfos.Count());
            ObjectInfos.ForEach(x => {
                writer.AlignTo(4);
                x.Write(writer);
                });
            writer.Write(Adds.Count());
            Adds.ForEach(x => x.Write(writer));
            writer.Write(ExternalFiles.Count());
            ExternalFiles.ForEach(x => x.Write(writer));
            writer.WriteCString("");
        }
        public int GetTypeIndexFromClassID(int classID)
        {
            var type = Types.FirstOrDefault(x => x.ClassID == classID);
            if (type == null)
                throw new ArgumentException("ClassID was not found in metadata.");

            return Types.IndexOf(type);
        }

        public int GetTypeIndexFromScriptHash(Guid hash)
        {
            var type = Types.FirstOrDefault(x => x.ScriptHash == hash);
            if (type == null)
                throw new ArgumentException("Script hash was not found in metadata.");
            return Types.IndexOf(type);
        }

        public int GetClassIDFromTypeIndex(int typeIndex)
        {
            if (typeIndex < 1 || typeIndex > Types.Count() - 1)
                throw new ArgumentException("There is no type at this index.");
            return Types[typeIndex].ClassID;
        }
    }
}

