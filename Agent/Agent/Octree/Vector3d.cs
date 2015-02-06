using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Tools.Point;

namespace Tools.Vector
{

    ///<summary>
    ///3D Vector (double)
    ///</summary>
    [Serializable]
    public class Vector3D : 
        IComparable<Vector3D>, 
        ICloneable, 
        ISerializable
    {
        #region Fileds

        protected double[] nXyz = new double[3];

        #endregion Fileds

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Vector3D()
        {
            nXyz[0] = 0.0;
            nXyz[1] = 0.0;
            nXyz[2] = 0.0;
        }

        /// <summary>
        /// Constructor - overload 1
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        public Vector3D(double x, double y, double z)
        {
            nXyz[0] = x;
            nXyz[1] = y;
            nXyz[2] = z;
        }

        public Vector3D(Point3D point)
        {
            nXyz[0] = point.X;
            nXyz[1] = point.Y;
            nXyz[2] = point.Z;
        }

        /// <summary>
        /// Constructor - overload 2
        /// </summary>
        /// <param name="XYZ">A double array for coordinates</param>
        public Vector3D(double[] xyz)
        {
            nXyz = (double[])xyz.Clone();
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
                    return nXyz[i];
                return Double.NaN;
            }
            set 
            {
                if (i < 3)
                    nXyz[i] = value;
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

        #region ISerializable

        //Deserialization constructor
        public Vector3D(SerializationInfo info, StreamingContext ctxt)
        {
            SerializationReader sr = SerializationReader.GetReader(info);
            nXyz = sr.ReadDoubleArray();
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            SerializationWriter sw = SerializationWriter.GetWriter();
            sw.Write(nXyz);
            sw.AddToInfo(info);
        }

        #endregion

        #region IDisposable

        //// Pointer to an external unmanaged resource.
        //// Other managed resource this class uses.
        //private Component component = new Component();
        //// Track whether Dispose has been called.
        //private bool disposed = false;
        //private IntPtr handle;

        //// Implement IDisposable.
        //// Do not make this method virtual.
        //// A derived class should not be able to override this method.
        //public void Dispose()
        //{
        //    Dispose(true);
        //    // This object will be cleaned up by the Dispose method.
        //    // Therefore, you should call GC.SupressFinalize to
        //    // take this object off the finalization queue 
        //    // and prevent finalization code for this object
        //    // from executing a second time.
        //    GC.SuppressFinalize(this);
        //}

        //// Dispose(bool disposing) executes in two distinct scenarios.
        //// If disposing equals true, the method has been called directly
        //// or indirectly by a user's code. Managed and unmanaged resources
        //// can be disposed.
        //// If disposing equals false, the method has been called by the 
        //// runtime from inside the finalizer and you should not reference 
        //// other objects. Only unmanaged resources can be disposed.
        //private void Dispose(bool disposing)
        //{
        //    // Check to see if Dispose has already been called.
        //    if (!disposed)
        //    {
        //        // If disposing equals true, dispose all managed 
        //        // and unmanaged resources.
        //        if (disposing)
        //        {
        //            // Dispose managed resources.
        //            component.Dispose();
        //        }

        //        // Call the appropriate methods to clean up 
        //        // unmanaged resources here.
        //        // If disposing is false, 
        //        // only the following code is executed.
        //        CloseHandle(handle);
        //        handle = IntPtr.Zero;
        //    }
        //    disposed = true;
        //}

        //// Use interop to call the method necessary  
        //// to clean up the unmanaged resource.
        //[DllImport("Kernel32")]
        //private static extern Boolean CloseHandle(IntPtr handle);

        //// Use C# destructor syntax for finalization code.
        //// This destructor will run only if the Dispose method 
        //// does not get called.
        //// It gives your base class the opportunity to finalize.
        //// Do not provide destructors in types derived from this class.
        //~Vector3d()
        //{
        //    // Do not re-create Dispose clean-up code here.
        //    // Calling Dispose(false) is optimal in terms of
        //    // readability and maintainability.
        //    Dispose(false);
        //}

        #endregion

        #region ICloneable
        /// <summary>
        /// deep copy of a Vector3d
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new Vector3D((double[])nXyz.Clone());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Cross product of two vectors.
        /// </summary>
        /// <param name="v">Vector3d</param>
        /// <returns>a Vector3d representing the cross product of the current vector and vector v</returns>
        public Vector3D CrossProduct(Vector3D v)
        {
            return new Vector3D(
                (this.Y * v.Z) - (this.Z * v.Y),
                (this.Z * v.X) - (this.X * v.Z),
                (this.X * v.Y) - (this.Y * v.X));
        }

        /// <summary>
        /// dot product of two vectors.
        /// </summary>
        /// <param name="v">Vector3d</param>
        /// <returns>a float representing the dot product of the current vector and vector v</returns>
        public double DotProduct(Vector3D v)
        {
            return this.X * v.X + this.Y * v.Y + this.Z * v.Z;
        }

        public Vector3F ToVector3F()
        {
            return new Vector3F(nXyz);
        }

        /// <summary>
        /// returns the sorted xyz coordinates
        /// </summary>
        public double[] SortedList()
        {
            double[] xyzS = (double[])nXyz.Clone();
            Array.Sort(xyzS);
            return xyzS;
        }

        /// <summary>
        /// check if the vector is NaN or Infinity
        /// </summary>
        /// <returns></returns>
        private bool IsNan()
        {
            for (int i = 0; i < 3; i++)
                if (Double.IsNaN(nXyz[i]) || Double.IsInfinity(nXyz[i]))
                    return true;

            return false;
        }

        /// <summary>
        /// get Max
        /// </summary>
        public double Max()
        {
            return Math.Max(nXyz[0], Math.Max(nXyz[1], nXyz[2]));
        }

        /// <summary>
        /// get Min
        /// </summary>
        public double Min()
        {
            return Math.Min(nXyz[0], Math.Min(nXyz[1], nXyz[2]));
        }

        /// <summary>
        /// Vector length
        /// </summary>
        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        /// <summary>
        /// get xyz coordinates
        /// </summary>
        public double[] ToArray()
        {
            return nXyz;
        }

        /// <summary>
        /// Converts coordinates to a Point3d
        /// </summary>
        public Point3D ToPoint3D()
        {
            return new Point3D(this.X, this.Y, this.Z);
        }
       
        #endregion

        #region IComparable

        /// <summary>
        /// 0: Vectors are identical.
        /// 1: All vectors components are greater. 
        /// -1: All vectors components are smaller. 
        /// -10: otherwise.
        /// </summary>
        /// <param name="other">Vector3d</param>
        /// <returns></returns>
        public int CompareTo(Vector3D other)
        {
            int result = 0;
            for (int i = 0; i < 3; i++)
                result += nXyz[i].CompareTo(other.nXyz[i]);

            switch (result)
            {
                case 0:
                    return 0;
                case 3:
                    return 1;
                case -3:
                    return -1;
                default:
                    return -10;
            }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Overrides ToString to print a vector
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.X + " " + this.Y + " " + this.Z;
        }

        #endregion

        #region Operator Overloads

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector3D operator +(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector3D operator +(Vector3D v1, Point3D v2)
        {
            return new Vector3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v2"></param>
        /// <param name="v1"></param>
        /// <returns></returns>
        public static Vector3D operator -(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector3D operator -(Vector3D v1, Point3D v2)
        {
            return new Vector3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }
        /// <summary>
        /// Scalar Product
        /// </summary>
        /// <param name="v"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Vector3D operator *(Vector3D v, double s)
        {
            return new Vector3D(s * v.X, s * v.Y, s * v.Z);
        }
        public static Vector3D operator *(double s, Vector3D v)
        {
            return v * s;
        }
        public static Vector3D operator /(Vector3D v, double s)
        {
            return v * (1 / s);
        }

        /// <summary>
        /// Dot Product
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double operator *(Vector3D v1, Vector3D v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        //public static bool operator ==(Vector3d v1, Vector3d v2)
        //{

        //    double[] v1Sorted = v1.SortedList();
        //    double[] v2Sorted = v2.SortedList();

        //    for (int i = 0; i < 3; i++)
        //    {
        //        if (Math.Abs(v1Sorted[i] - v2Sorted[i]) > Epsilon)
        //            return false;
        //    }

        //    return true;
        //}

        //public static bool operator !=(Vector3d v1, Vector3d v2)
        //{
        //    return !(v1 == v2);
        //}
        #region IEquatable
        //public override bool Equals(object v)
        //{
        //    return (this.CompareTo((Vector3d)v) == 0 ? true : false);
        //}
        public bool Equals(Vector3D v)
        {
            return (this.CompareTo(v) == 0 ? true : false);
        }
        #endregion
        //public override int GetHashCode()
        //{
        //    return 0;
        //}

        #endregion Operator Overloads

        #region Properties

        /// <summary>
        /// get/set x coordinate
        /// </summary>
        public double X
        {
            get { return nXyz[0]; }
            set { nXyz[0] = value; }
        }

        /// <summary>
        /// get/set y coordinate
        /// </summary>
        public double Y
        {
            get { return nXyz[1]; }
            set { nXyz[1] = value; }
        }

        /// <summary>
        /// get/set z coordinate
        /// </summary>
        public double Z
        {
            get { return nXyz[2]; }
            set { nXyz[2] = value; }
        }

        #endregion

        #region IEnumerator

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public IEnumerator GetEnumerator()
        //{
        //    return new VectorEnumerator(this);
        //}

        /// <summary>
        /// this enable a vector to be looped in a foreach
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        //public double this[uint i]
        //{
        //    get
        //    {
        //        switch (i)
        //        {
        //            case 0:
        //                return x;
        //            case 1:
        //                return y;
        //            case 2:
        //                return z;
        //            default:
        //                throw new IndexOutOfRangeException(
        //                   "Attempt to retrieve Vector element" + i);
        //        }
        //    }
        //    set
        //    {
        //        switch (i)
        //        {
        //            case 0:
        //                x = value;
        //                break;
        //            case 1:
        //                y = value;
        //                break;
        //            case 2:
        //                z = value;
        //                break;
        //            default:
        //                throw new IndexOutOfRangeException(
        //                   "Attempt to set Vector element" + i);
        //        }
        //    }
        //}
        //private class VectorEnumerator : IEnumerator
        //{
        //    Vector3d theVector;      // Vector object that this enumerato refers to 
        //    int location;   // which element of theVector the enumerator is currently referring to 

        //    public VectorEnumerator(Vector3d theVector)
        //    {
        //        this.theVector = theVector;
        //        location = -1;
        //    }

        //    public bool MoveNext()
        //    {
        //        ++location;
        //        return (location > 2) ? false : true;
        //    }

        //    public object Current
        //    {
        //        get
        //        {
        //            if (location < 0 || location > 2)
        //                throw new InvalidOperationException(
        //                   "The enumerator is either before the first element or " +
        //                   "after the last element of the Vector");
        //            return theVector[(uint)location];
        //        }
        //    }

        //    public void Reset()
        //    {
        //        location = -1;
        //    }
        //}

        #endregion
    }


}
