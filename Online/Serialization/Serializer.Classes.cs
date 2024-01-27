﻿using System;
using System.Collections.Generic;

namespace RainMeadow
{
    public partial class Serializer
    {
        // todo make this load-order independent. How? map strings to indexes somewhere else don't rely on the .Index
        public void SerializeExtEnum<T>(ref T extEnum) where T : ExtEnum<T>
        {
            if (IsWriting)
            {
                writer.Write((byte)extEnum.Index);
                if (IsWriting) RainMeadow.Trace(1);
            }
            if (IsReading)
            {
                extEnum = (T)Activator.CreateInstance(typeof(T), new object[] { ExtEnum<T>.values.GetEntry(reader.ReadByte()), false });
            }
        }

        public void SerializeExtEnums<T>(ref T[] extEnum) where T : ExtEnum<T>
        {
            if (IsWriting)
            {
                writer.Write((byte)extEnum.Length);
                if (IsWriting) RainMeadow.Trace(1);
                for (int i = 0; i < extEnum.Length; i++)
                {
                    writer.Write((byte)extEnum[i].Index);
                }
            }
            if (IsReading)
            {
                extEnum = new T[reader.ReadByte()];
                for (int i = 0; i < extEnum.Length; i++)
                {
                    extEnum[i] = (T)Activator.CreateInstance(typeof(T), new object[] { ExtEnum<T>.values.GetEntry(reader.ReadByte()), false });
                }
            }
        }

        public void SerializeExtEnums<T>(ref List<T> extEnum) where T : ExtEnum<T>
        {
            if (IsWriting)
            {
                writer.Write((byte)extEnum.Count);
                if (IsWriting) RainMeadow.Trace(1);
                for (int i = 0; i < extEnum.Count; i++)
                {
                    writer.Write((byte)extEnum[i].Index);
                }
            }
            if (IsReading)
            {
                var count = reader.ReadByte();
                extEnum = new(count);
                for (int i = 0; i < count; i++)
                {
                    extEnum.Add((T)Activator.CreateInstance(typeof(T), new object[] { ExtEnum<T>.values.GetEntry(reader.ReadByte()), false }));
                }
            }
        }
    }
}