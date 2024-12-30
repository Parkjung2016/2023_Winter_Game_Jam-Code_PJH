using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Parkjung2016.Library
{
    public static class StringLibrary
    {
        private static Dictionary<string, StringBuilder> stringBuilderBuffer = new();

        /// <summary>
        /// 문자열 이어붙이기
        /// </summary>
        /// <param name="str">순서대로 문자열을 이어붙임</param>
        /// <returns></returns>
        public static string AppendString(string[] str)
        {
            StringBuilder sb = GetStringBuildr(str[0]);

            for (int i = 1; i < str.Length; i++)
            {
                sb.Append(str[i]);
            }

            return sb.ToString();
        }
        /// <summary>
        /// 지정된 인덱스에 문자열 삽입
        /// </summary>
        /// <param name="str1">문지열</param>
        /// <param name="str2">이어붙일 문자열</param>
        /// <param name="idx">인덱스 </param>
        /// <returns></returns>
        public static string InsertString(string str1,string str2,byte idx)
        {
            StringBuilder sb = GetStringBuildr(str1);

            sb.Insert(idx, str2);

            return sb.ToString();
        }
        /// <summary>
        /// 지정된 인덱스 위치에서 지정된 길이만큼 문자열 제거
        /// </summary>
        /// <param name="str">문지열</param>
        /// <param name="idx">인덱스 </param>
        /// <param name="length">길이 </param>
        /// <returns></returns>
        public static string InsertString(string str,byte idx,byte length)
        {
            StringBuilder sb = GetStringBuildr(str);

            sb.Remove(idx, length);

            return sb.ToString();
        }
        /// <summary>
        /// 특정 문자열을 지정된 문자열로 변경
        /// </summary>
        /// <param name="str">문지열</param>
        /// <param name="str2">바꿀 문자열</param>
        /// <returns></returns>
        public static string InsertString(string str,string str2)
        {
            StringBuilder sb = GetStringBuildr(str);

            sb.Replace(str, str2);

            return sb.ToString();
        }
        private static StringBuilder GetStringBuildr(string str)
        {
            StringBuilder sb = null;
            if (!stringBuilderBuffer.TryGetValue(str, out sb))
                sb = new StringBuilder(str);

            return sb;
        }
    }
}