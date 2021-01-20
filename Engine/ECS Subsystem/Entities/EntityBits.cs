using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Engine.Entities
{
    public class EntityBits
    {
        public static readonly EntityBits Zero = new EntityBits();
        Vector<uint> myParallelType;

        public override string ToString()
        {
            return myParallelType.ToString();
        }

        public EntityBits()
        {
            myParallelType = new Vector<uint>();
        }
        public EntityBits(EntityBits myOther)
        {
            uint[] myInts = new uint[Vector<uint>.Count];
            myOther.myParallelType.CopyTo(myInts);
            myParallelType = new Vector<uint>(myInts);
        }

        public EntityBits(uint[] myInts)
        {
            myParallelType = new Vector<uint>(myInts);
        }

        public bool this[int Position]
        {
            get
            {
                int PositionBitToChange = Position % 32;
                int myIntToChange = (Position - PositionBitToChange) / 32;
                if (myIntToChange >= Vector<uint>.Count)
                {
                    throw new Exception("Invalid position requested in EntityBits Array!");
                }
                else
                {
                    uint myValue = myParallelType[myIntToChange];
                    myValue = myValue & ((uint)Math.Pow(2, PositionBitToChange));
                    return (myValue > 0);
                }
            }
            set
            {
                int PositionBitToChange = Position % 32;
                int myIntToChange = (Position - PositionBitToChange) / 32;
                if (myIntToChange >= Vector<uint>.Count)
                {
                    throw new Exception("Invalid position requested in EntityBits Array!");
                }
                else
                {
                    uint myValue = myParallelType[myIntToChange];
                    uint[] myInts = new uint[Vector<uint>.Count];
                    myParallelType.CopyTo(myInts, 0);
                    uint PositionValueCalculation = ((uint)Math.Pow(2, PositionBitToChange));                    
                    if (value)
                    {
                        if (PositionValueCalculation <= myValue) return; //It's true already.
                         myInts[myIntToChange] = myValue + PositionValueCalculation;
                    }
                    else
                    {
                        if (PositionValueCalculation > myValue) return; //It's false already.
                        myInts[myIntToChange] = myValue - ((uint)Math.Pow(2, PositionBitToChange));
                    }
                    myParallelType = new Vector<uint>(myInts);
                }
            }
        }





        public EntityBits Clone()
        {
            uint[] myInts = new uint[Vector<uint>.Count];
            myParallelType.CopyTo(myInts, 0);
            return new EntityBits(myInts);
        }
        /// <summary>
        /// Checks AND condition between two EntityBits and tests to see if the incoming Entity passes.
        /// </summary>
        /// <param name="AnotherEntity"></param>
        /// <returns></returns>
        public bool AndTest(EntityBits AnotherEntity)
        {
            var Return = myParallelType & AnotherEntity.myParallelType;
            return (Return == myParallelType);
        }
        /// <summary>
        /// Checks OR condition between two EntityBits and tests to see if the incoming Entity passes.
        /// </summary>
        /// <param name="AnotherEntity"></param>
        /// <returns></returns>
        public bool OrTest(EntityBits AnotherEntity)
        {
            var Return = myParallelType & AnotherEntity.myParallelType;
            return (Return != Vector<uint>.Zero);
        }
        /// <summary>
        /// Checks NOT condition between two EntityBits and tests to see if the incoming Entity passes. 
        /// {1,0,0} -> {0,1,1} | {#,#,#}
        /// </summary>
        /// <param name="AnotherEntity"></param>
        /// <returns>True if none of AnotherEntities bits overlaps the NOT operation</returns>
        public bool NotTest(EntityBits AnotherEntity)
        {
            var Return = (~myParallelType) & AnotherEntity.myParallelType;
            return (Return == Vector<uint>.Zero);
        }

        /// <summary>
        /// Permanently modifies
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static EntityBits operator &(EntityBits A, EntityBits B)
        {
            A.myParallelType = A.myParallelType & B.myParallelType;
            return A;
        }
        /// <summary>
        /// Permanently modifies
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static EntityBits operator |(EntityBits A, EntityBits B)
        {
            A.myParallelType = A.myParallelType | B.myParallelType;
            return A;
        }
        public static EntityBits operator ~(EntityBits A)
        {
            A.myParallelType = ~A.myParallelType;
            return A;
        }

        public static bool operator ==(EntityBits A, bool B)
        {
            if(B)
                return A.myParallelType != Vector<uint>.Zero;
            else
                return A.myParallelType == Vector<uint>.Zero;
        }

        public static bool operator !=(EntityBits A, bool B)
        {
            return (A == !B);
        }
        public static bool operator==(EntityBits A, EntityBits B)
        {
            if(B is null && !(A is null))
            {
                return false;
            }
            else if (A is null && !(B is null))
            {
                return false;

            }
            else if(B is null && A is null)
            {
                return true;
            }
            else
            {
                return A.myParallelType == B.myParallelType;
            }
        }

        public static bool operator !=(EntityBits A, EntityBits B)
        {
            if (B is null && !(A is null))
            {
                return true;
            }
            else if (A is null && !(B is null))
            {
                return true;

            }
            else if (B is null && A is null)
            {
                return false;
            }
            else
            {
                return A.myParallelType != B.myParallelType;
            }

        }

    }
}
