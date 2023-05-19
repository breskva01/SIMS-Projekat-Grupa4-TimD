using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Application.Util
{
    public class ArrayHandler
    {
        public static T[] AddObjectToArrayStart<T>(T[] array, T newObject)
        {
            T[] newArray = new T[array.Length + 1];
            newArray[0] = newObject;
            Array.Copy(array, 0, newArray, 1, array.Length);
            return newArray;
        }
        public static T[] AddObjectToArrayEnd<T>(T[] array, T newObject)
        {
            T[] newArray = new T[array.Length + 1];
            Array.Copy(array, newArray, array.Length);
            newArray[array.Length] = newObject;
            return newArray;
        }

        public static T[] RemoveFirstElement<T>(T[] array)
        {
            T[] newArray = new T[array.Length - 1];
            Array.Copy(array, 1, newArray, 0, newArray.Length);
            return newArray;
        }
    }
}
