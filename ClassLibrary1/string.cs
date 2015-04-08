using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{
    public class str_ftns
    {
        //public static string CapitalizeWords(string value)
        //{
        //    if (value == null)
        //        throw new ArgumentNullException("value");
        //    if (value.Length == 0)
        //        return value;

        //    StringBuilder result = new StringBuilder(value);
        //    result[0] = char.ToUpper(result[0]);
        //    for (int i = 1; i < result.Length; ++i)
        //    {
        //        if (char.IsWhiteSpace(result[i - 1]))
        //            result[i] = char.ToUpper(result[i]);
        //        else
        //            result[i] = char.ToLower(result[i]);
        //    }
        //    return result.ToString();
        //}


        /// <summary>
        /// convert lowercase strings with underscores (as used in sql databases) to capitalized strings used as vb variables
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string lower_underscore_to_capitalize(string value)
        {
            string res = "";
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == '_')
                {
                    array[i] = char.ToUpper(array[i]);
                }
                else 
                {
                    array[i] = char.ToLower(array[i]);
                }
            }

            res = new string(array);
            //res = array.ToString();//.Replace( .Replace("_", "");
            res = res.Replace("_", "");
            return (res);
        }


        /// <summary>
        /// convert lowercase strings with underscores (as used in sql databases) to display names (intended to be used as labels on a form)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string lower_underscore_to_display(string value)
        {
            string res = "";
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == '_')
                {
                    array[i] = char.ToUpper(array[i]);
                }
                else
                {
                    array[i] = char.ToLower(array[i]);
                }
            }

            res = new string(array);
            //res = array.ToString();//.Replace( .Replace("_", "");
            res = res.Replace("_", " ");
            return (res);
        }

    }
}
