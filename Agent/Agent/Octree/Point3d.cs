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
    /// 3D double point
    /// </summary>
    [Serializable]
    public class Point3D : 
        IEquatable<Point3D>, 
        ISerializable
    {
        private double[] nxyz = new double[3];

        public static readonly Point3D NullPoint = new Point3D(
                                                                double.NegativeInfinity,
                                                                double.NegativeInfinity,
                                                                double.NegativeInfinity);

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Point3D()
        {
        }

        /// <summary>
        /// Constructor - overload 1
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        public Point3D(double x, double y, double z)
        {
            nxyz[0] = x;
            nxyz[1] = y;
            nxyz[2] = z;
        }

        /// <summary>
        /// Constructor - overload 2
        /// </summary>
        /// <param name="XYZ">A double array for coordinates</param>
        public Point3D(double[] xyz)
        {
            nxyz = (double[])xyz.Clone();
        }

        /// <summary>
        /// Constructor - overload 3
        /// </summary>
        /// <param name="XYZ">A vector for coordinates</param>
        public Point3D(Vector3D vector)
        {
            nxyz = vector.ToArray();
        }

        #endregion

        #region ISerializable

        //Deserialization constructor
        public Point3D(SerializationInfo info, StreamingContext ctxt)
        {
            SerializationReader sr = SerializationReader.GetReader(info);
            nxyz = sr.ReadDoubleArray();
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
        /// <param name="XYZ">A double array for coordinates</param>
        public double this[byte i]
        {
            get
            {
                if (i < 3)
                    return nxyz[i];
                return Double.NaN;
            }
            set
            {
                if (i < 3)
                    nxyz[i] = value;
            }

        }
        public double this[int i]
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
        public double this[uint i]
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
        public double[] Getxyz()
        {
            return nxyz;
        }
        /// <summary>
        /// get Max
        /// </summary>
        public double Max()
        {
            return Math.Max(nxyz[0], Math.Max(nxyz[1], nxyz[2]));
        }

        /// <summary>
        /// get Min
        /// </summary>
        public double Min()
        {
            return Math.Min(nxyz[0], Math.Min(nxyz[1], nxyz[2]));
        }
        

        /// <summary>
        /// Write coordinates
        /// </summary>
        /// <returns></returns>
        public string WriteCoordinate()
        {
            return new Vector3D(nxyz).ToString();
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
        public bool AlmostEquals(Point3D p2, double error)
        {
            return Math.Abs(this.X - p2.X) <= error &&
                   Math.Abs(this.Y - p2.Y) <= error &&
                   Math.Abs(this.Z - p2.Z) <= error;
        }
        public bool Equals(Point3D p2)
        {
            return this.X == p2.X && this.Y == p2.Y && this.Z == p2.Z;
        }
        #endregion

        #region Properties
        /// <summary>
        /// get/set x coordinate
        /// </summary>
        public double X
        {
            get { return nxyz[0]; }
            set { nxyz[0] = value; }
        }

        /// <summary>
        /// get/set y coordinate
        /// </summary>
        public double Y
        {
            get { return nxyz[1]; }
            set { nxyz[1] = value; }
        }

        /// <summary>
        /// get/set z coordinate
        /// </summary>
        public double Z
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
