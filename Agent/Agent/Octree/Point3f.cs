using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Tools.Vector;

namespace Tools.Point
{

    /// <summary>
    /// 3D float point
    /// </summary>
    [Serializable]
    public class Point3F :
        IEquatable<Point3F>,
        ISerializable
    {
        private float[] nxyz = new float[3];

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Point3F()
        {
        }

        /// <summary>
        /// Constructor - overload 1
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        public Point3F(float x, float y, float z)
        {
            nxyz[0] = x;
            nxyz[1] = y;
            nxyz[2] = z;
        }

        /// <summary>
        /// Constructor - overload 2
        /// </summary>
        /// <param name="XYZ">A float array for coordinates</param>
        public Point3F(float[] xyz)
        {
            nxyz = (float[])xyz.Clone();
        }

        /// <summary>
        /// Constructor - overload 3
        /// </summary>
        /// <param name="XYZ">A vector for coordinates</param>
        public Point3F(Vector3F vector)
        {
            nxyz = vector.ToArray();
        }

        #endregion

        #region ISerializable

        //Deserialization constructor
        public Point3F(SerializationInfo info, StreamingContext ctxt)
        {
            SerializationReader sr = SerializationReader.GetReader(info);
            nxyz = sr.ReadFloatArray();
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            SerializationWriter sw = SerializationWriter.GetWriter();
            sw.Write(nxyz);
            sw.AddToInfo(info);
        }

        #endregion

        #region Indexers
        /// <summary>
        /// Indexer
        /// </summary>
        /// <param name="XYZ">A float array for coordinates</param>
        public float this[byte i]
        {
            get
            {
                if (i < 3)
                    return nxyz[i];
                return float.NaN;
            }
            set
            {
                if (i < 3)
                    nxyz[i] = value;
            }

        }
        public float this[int i]
        {
            get
            {
                return this[(byte)i];
            }
            set
            {
                this[(byte)i] = value;
            }

        }
        public float this[uint i]
        {
            get
            {
                return this[(byte)i];
            }
            set
            {
                this[(byte)i] = value;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// get xyz coordinates
        /// </summary>
        public float[] Getxyz()
        {
            return nxyz;
        }
        /// <summary>
        /// get Max
        /// </summary>
        public float Max()
        {
            return Math.Max(nxyz[0], Math.Max(nxyz[1], nxyz[2]));
        }

        /// <summary>
        /// get Min
        /// </summary>
        public float Min()
        {
            return Math.Min(nxyz[0], Math.Min(nxyz[1], nxyz[2]));
        }
        

        /// <summary>
        /// Write coordinates
        /// </summary>
        /// <returns></returns>
        public string WriteCoordinate()
        {
            return new Vector3F(nxyz).ToString();
        }
        /// <summary>
        /// Write one coordinate
        /// </summary>
        /// <returns></returns>
        public string WriteCoordinate(byte index)
        {
            return this.nxyz[index].ToString();
        }
        /// <summary>
        /// Write one coordinate
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string WriteCoordinate(int index)
        {
            return WriteCoordinate((byte)index);
        }
        /// <summary>
        /// Write one coordinate
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string WriteCoordinate(uint index)
        {
            return WriteCoordinate((byte)index);
        }
        public bool AlmostEquals(Point3F p2, float error)
        {
            return Math.Abs(this.X - p2.X) <= error &&
                   Math.Abs(this.Y - p2.Y) <= error &&
                   Math.Abs(this.Z - p2.Z) <= error;
        }
        public bool Equals(Point3F p2)
        {
            return this.X == p2.X && this.Y == p2.Y && this.Z == p2.Z;
        }
        
        public static float Area2(Point3F p0, Point3F p1, Point3F p2)
        {
            return 0; // p0.x * (p1.y - p2.y) + p1.x * (p2.y - p0.y) + p2.x * (p0.y - p1.y);
        }
        #endregion

        #region Properties
        /// <summary>
        /// get/set x coordinate
        /// </summary>
        public float X
        {
            get { return nxyz[0]; }
            set { nxyz[0] = value; }
        }

        /// <summary>
        /// get/set y coordinate
        /// </summary>
        public float Y
        {
            get { return nxyz[1]; }
            set { nxyz[1] = value; }
        }

        /// <summary>
        /// get/set z coordinate
        /// </summary>
        public float Z
        {
            get { return nxyz[2]; }
            set { nxyz[2] = value; }
        }
        #endregion

        #region Overrides

        /// <summary>
        /// Overrides ToString to print a point
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.X + " " + this.Y + " " + this.Z;
        }

        #endregion


    }

}
