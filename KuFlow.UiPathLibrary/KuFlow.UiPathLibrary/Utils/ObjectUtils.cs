using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuFlow.UiPathLibrary.Utils
{
    public static class ObjectUtils
    {
        public static bool IsNumber(object value)
        {
            return value is int || value is float || value is double || value is decimal;
        }

        public static bool IsListOrArray(object obj)
        {
            return obj is IList || obj is Array;
        }
    }
}
