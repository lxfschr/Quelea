using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Tools.Point;
using Tools.Vector;

namespace Tools
{
    #region FastSerialization

    // Enum for the standard types handled by Read/WriteObject()
    internal enum ObjType : byte
    {
        NullType,
        BoolType,
        ByteType,
        Uint16Type,
        Uint32Type,
        Uint64Type,
        SbyteType,
        Int16Type,
        Int32Type,
        Int64Type,
        CharType,
        StringType,
        SingleType,
        DoubleType,
        DecimalType,
        DateTimeType,
        ByteArrayType,
        CharArrayType,
        Vector3DType,
        Vector3FType,
        OtherType
    }

    /// <summary> SerializationWriter.  Extends BinaryWriter to add additional data types,
    /// handle null strings and simplify use with ISerializable. </summary>
    public class SerializationWriter : BinaryWriter
    {
        private SerializationWriter(Stream s) : base(s) { }

        /// <summary> Static method to initialise the writer with a suitable MemoryStream. </summary>

        public static SerializationWriter GetWriter()
        {
            MemoryStream ms = new MemoryStream(1024);
            return new SerializationWriter(ms);
        }

        /// <summary> Writes a string to the buffer.  Overrides the base implementation so it can cope with nulls </summary>

        public override void Write(string str)
        {
            if (str == null)
            {
                Write((byte)ObjType.NullType);
            }
            else
            {
                Write((byte)ObjType.StringType);
                base.Write(str);
            }
        }

        /// <summary> Writes a byte array to the buffer.  Overrides the base implementation to
        /// send the length of the array which is needed when it is retrieved </summary>

        public override void Write(byte[] b)
        {
            if (b == null)
            {
                Write(-1);
            }
            else
            {
                int len = b.Length;
                Write(len);
                if (len > 0) base.Write(b);
            }
        }

        /// <summary> Writes a char array to the buffer.  Overrides the base implementation to
        /// sends the length of the array which is needed when it is read. </summary>

        public override void Write(char[] chars)
        {
            if (chars == null)
            {
                Write(-1);
            }
            else
            {
                int len = chars.Length;
                Write(len);
                if (len > 0) base.Write(chars);
            }
        }

        /// <summary> Writes a DateTime to the buffer. <summary>

        public void Write(DateTime dt) { Write(dt.Ticks); }

        /// <summary> Writes a generic ICollection (such as an IList<T>) to the buffer. </summary>

        public void Write<T>(ICollection<T> collection)
        {
            if (collection == null)
            {
                Write(-1);
            }
            else
            {
                Write(collection.Count);
                foreach (T item in collection) WriteObject(item);
            }
        }

        /// <summary> Writes a generic IDictionary to the buffer. </summary>

        public void Write<T, TU>(IDictionary<T, TU> dictionary)
        {
            if (dictionary == null)
            {
                Write(-1);
            }
            else
            {
                Write(dictionary.Count);
                foreach (KeyValuePair<T, TU> kvp in dictionary)
                {
                    WriteObject(kvp.Key);
                    WriteObject(kvp.Value);
                }
            }
        }

        public void Write(uint[] uInt32Array)
        {
            if (uInt32Array == null)
            {
                Write(-1);
            }
            else
            {
                base.Write(uInt32Array.Length);
                for (int i = 0; i < uInt32Array.Length; i++)
                    base.Write(uInt32Array[i]);
            }
        }

        public void Write(int[] int32Array)
        {
            if (int32Array == null)
            {
                Write(-1);
            }
            else
            {
                base.Write(int32Array.Length);
                for (int i = 0; i < int32Array.Length; i++)
                    base.Write(int32Array[i]);
            }
        }

        public void Write(double[] doubleArray)
        {
            if (doubleArray == null)
            {
                Write(-1);
            }
            else
            {
                base.Write(doubleArray.Length);
                for (int i = 0; i < doubleArray.Length; i++)
                    base.Write(doubleArray[i]);
            }
        }

        public void Write(Vector3F vector)
        {
            if (vector == null)
            {
                Write(-1);
            }
            else
            {
                Write(vector.ToArray());
            }
        }

        public void Write(Vector3D vector)
        {
            if (vector == null)
            {
                Write(-1);
            }
            else
            {
                Write(vector.ToArray());
            }
        }

        public void Write(Point3D point)
        {
            if (point == null)
            {
                Write(-1);
            }
            else
            {
                Write(point.Getxyz());
            }
        }

        /// <summary> Writes an arbitrary object to the buffer.  Useful where we have something of type "object"
        /// and don't know how to treat it.  This works out the best method to use to write to the buffer. </summary>
        public void WriteObject(object obj)
        {
            if (obj == null)
            {
                Write((byte)ObjType.NullType);
            }
            else
            {
                switch (obj.GetType().Name)
                {
                    case "Boolean":
                        Write((byte)ObjType.BoolType);
                        Write((bool)obj);
                        break;

                    case "Byte":
                        Write((byte)ObjType.ByteType);
                        Write((byte)obj);
                        break;

                    case "UInt16":
                        Write((byte)ObjType.Uint16Type);
                        Write((ushort)obj);
                        break;

                    case "UInt32":
                        Write((byte)ObjType.Uint32Type);
                        Write((uint)obj);
                        break;

                    case "UInt64":
                        Write((byte)ObjType.Uint64Type);
                        Write((ulong)obj);
                        break;

                    case "SByte":
                        Write((byte)ObjType.SbyteType);
                        Write((sbyte)obj);
                        break;

                    case "Int16":
                        Write((byte)ObjType.Int16Type);
                        Write((short)obj);
                        break;

                    case "Int32":
                        Write((byte)ObjType.Int32Type);
                        Write((int)obj);
                        break;

                    case "Int64":
                        Write((byte)ObjType.Int64Type);
                        Write((long)obj);
                        break;

                    case "Char":
                        Write((byte)ObjType.CharType);
                        base.Write((char)obj);
                        break;

                    case "String":
                        Write((byte)ObjType.StringType);
                        base.Write((string)obj);
                        break;

                    case "Single":
                        Write((byte)ObjType.SingleType);
                        Write((float)obj);
                        break;

                    case "Double":
                        Write((byte)ObjType.DoubleType);
                        Write((double)obj);
                        break;

                    case "Decimal":
                        Write((byte)ObjType.DecimalType);
                        Write((decimal)obj);
                        break;

                    case "DateTime":
                        Write((byte)ObjType.DateTimeType);
                        Write((DateTime)obj);
                        break;

                    case "Byte[]":
                        Write((byte)ObjType.ByteArrayType);
                        base.Write((byte[])obj);
                        break;

                    case "Char[]":
                        Write((byte)ObjType.CharArrayType);
                        base.Write((char[])obj);
                        break;

                    default:
                        Write((byte)ObjType.OtherType);
                        new BinaryFormatter().Serialize(BaseStream, obj);
                        break;
                } // switch
            } // if obj==null
        } // WriteObject

        /// <summary> Adds the SerializationWriter buffer to the SerializationInfo at the end of GetObjectData(). </summary>
        public void AddToInfo(SerializationInfo info)
        {
            byte[] b = ((MemoryStream)BaseStream).ToArray();
            info.AddValue("X", b, typeof(byte[]));
        }
    } // SerializationWriter

    /// <summary> SerializationReader.  Extends BinaryReader to add additional data types,
    /// handle null strings and simplify use with ISerializable. </summary>
    public class SerializationReader : BinaryReader
    {
        private SerializationReader(Stream s)
            : base(s)
        {
        }

        /// <summary> Static method to take a SerializationInfo object (an input to an ISerializable constructor)
        /// and produce a SerializationReader from which serialized objects can be read </summary>.
        public static SerializationReader GetReader(SerializationInfo info)
        {
            byte[] byteArray = (byte[])info.GetValue("X", typeof(byte[]));
            MemoryStream ms = new MemoryStream(byteArray);
            return new SerializationReader(ms);
        }

        /// <summary> Reads a string from the buffer.  Overrides the base implementation so it can cope with nulls. </summary>
        public override string ReadString()
        {
            ObjType t = (ObjType)ReadByte();
            if (t == ObjType.StringType) return base.ReadString();
            return null;
        }

        /// <summary> Reads a byte array from the buffer, handling nulls and the array length. </summary>
        public byte[] ReadByteArray()
        {
            int len = ReadInt32();
            if (len > 0) return ReadBytes(len);
            if (len < 0) return null;
            return new byte[0];
        }

        /// <summary> Reads a char array from the buffer, handling nulls and the array length. </summary>
        public char[] ReadCharArray()
        {
            int len = ReadInt32();
            if (len > 0) return ReadChars(len);
            if (len < 0) return null;
            return new char[0];
        }

        /// <summary> Reads a DateTime from the buffer. </summary>
        public DateTime ReadDateTime()
        {
            return new DateTime(ReadInt64());
        }

        /// <summary> Reads a generic list from the buffer. </summary>
        public IList<T> ReadList<T>()
        {
            int count = ReadInt32();
            if (count < 0) return null;
            IList<T> d = new List<T>();
            for (int i = 0; i < count; i++) 
                d.Add((T)ReadObject());
            return d;
        }

        /// <summary> Reads a generic Dictionary from the buffer. </summary>
        public IDictionary<T, TU> ReadDictionary<T, TU>()
        {
            int count = ReadInt32();
            if (count < 0) return null;
            IDictionary<T, TU> d = new Dictionary<T, TU>();
            for (int i = 0; i < count; i++) 
                d[(T)ReadObject()] = (TU)ReadObject();
            return d;
        }

        /// <summary>
        /// UInt32 Array
        /// </summary>
        /// <returns></returns>
        public uint[] ReadUInt32Array()
        {
            int len = ReadInt32();
            if (len < 0) return null;

            uint[] xyz = new uint[len];
            for (int i = 0; i < len; i++)
                xyz[i] = ReadUInt32();

            return xyz;
        }

        /// <summary>
        /// Int32 Array
        /// </summary>
        /// <returns></returns>
        public double[] ReadInt32Array()
        {
            int len = ReadInt32();
            if (len < 0) return null;

            double[] xyz = new double[len];
            for (int i = 0; i < len; i++)
                xyz[i] = ReadInt32();

            return xyz;
        }

        /// <summary>
        /// Float Array
        /// </summary>
        /// <returns></returns>
        public float[] ReadFloatArray()
        {
            int len = ReadInt32();
            if (len < 0) return null;

            float[] xyz = new float[len];
            for (int i = 0; i < len; i++)
                xyz[i] = ReadSingle();

            return xyz;
        }

        /// <summary>
        /// Double Array
        /// </summary>
        /// <returns></returns>
        public double[] ReadDoubleArray()
        {
            int len = ReadInt32();
            if (len < 0) return null;

            double[] xyz = new double[len];
            for (int i = 0; i < len; i++)
                xyz[i] = ReadDouble();

            return xyz;
        }

        /// <summary>
        /// Read Vector3f
        /// </summary>
        /// <returns></returns>
        public Vector3F ReadVector3F()
        {
            return new Vector3F(ReadFloatArray());
        }

        /// <summary>
        /// Read Vector3d
        /// </summary>
        /// <returns></returns>
        public Vector3D ReadVector3D()
        {
            return new Vector3D(ReadDoubleArray());
        }

        /// <summary>
        /// Read Point3d
        /// </summary>
        /// <returns></returns>
        public Point3D ReadPoint3D()
        {
            return new Point3D(ReadDoubleArray());
        }
        /// <summary> Reads an object which was added to the buffer by WriteObject. </summary>
        public object ReadObject()
        {
            ObjType t = (ObjType)ReadByte();
            switch (t)
            {
                case ObjType.BoolType:
                    return ReadBoolean();
                case ObjType.ByteType:
                    return ReadByte();
                case ObjType.Uint16Type:
                    return ReadUInt16();
                case ObjType.Uint32Type:
                    return ReadUInt32();
                case ObjType.Uint64Type:
                    return ReadUInt64();
                case ObjType.SbyteType:
                    return ReadSByte();
                case ObjType.Int16Type:
                    return ReadInt16();
                case ObjType.Int32Type:
                    return ReadInt32();
                case ObjType.Int64Type:
                    return ReadInt64();
                case ObjType.CharType:
                    return ReadChar();
                case ObjType.StringType:
                    return base.ReadString();
                case ObjType.SingleType:
                    return ReadSingle();
                case ObjType.DoubleType:
                    return ReadDouble();
                case ObjType.DecimalType:
                    return ReadDecimal();
                case ObjType.DateTimeType:
                    return ReadDateTime();
                case ObjType.ByteArrayType:
                    return ReadByteArray();
                case ObjType.CharArrayType:
                    return ReadCharArray();
                case ObjType.Vector3DType:
                    return ReadVector3D();
                case ObjType.Vector3FType:
                    return ReadVector3F();
                case ObjType.OtherType:
                    return new BinaryFormatter().Deserialize(BaseStream);
                default:
                    return null;
            }
        }
    } // SerializationReader

    #endregion
}
