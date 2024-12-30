using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Parkjung2016.Library
{
    public class MathLibrary
    {
        /// <summary>
        /// 전체값에서 일부값을 구하는 함수
        /// </summary>
        /// <param name="totalValue">전체값</param>
        /// <param name="someValue">일부값</param>
        /// <returns></returns>
        public static float GetPercentageOfSomeValuesInTheTotalValue(float totalValue, float someValue)
        {
            return someValue / totalValue * 100;
        }

        /// <summary>
        /// 전체값의 몇 퍼센트는 얼마인지 구하는 함수
        /// </summary>
        /// <param name="totalValue">전체값</param>
        /// <param name="percent">몇 퍼센트</param>
        /// <returns></returns>
        public static float GetPercentageOfTheTotalValue(float totalValue, float percent)
        {
            return totalValue * percent / 100;
        }

        /// <summary>
        /// 숫자를 몇 퍼센트 증가시키는 함수
        /// </summary>
        /// <param name="value">숫자</param>
        /// <param name="percent">몇 퍼센트</param>
        /// <returns></returns>
        public static float GetIncreaseTheNumberByAFewPercent(float value, float percent)
        {
            return value * (1 + percent * 100);
        }
        /// <summary>
        /// 벡터 투영(경사로)
        /// </summary>
        /// <param name="direction">방향</param>
        /// <param name="normal">법선 벡터</param>
        /// <returns></returns>
        public static  Vector3 AdjustDirectionToSlope(Vector3 direction,Vector3 normal)
        {
            return Vector3.ProjectOnPlane(direction, normal).normalized;
        }

        /// <summary>
        /// 숫자를 몇 퍼센트 감소시키는 함수
        /// </summary>
        /// <param name="value">숫자</param>
        /// <param name="percent">몇 퍼센트</param>
        /// <returns></returns>
        public static float GetDecreaseTheNumberByAFewPercent(float value, float percent)
        {
            
            return value * (1 - percent * 100);
        }
        /// <summary>
        /// 타겟 트랜스폼에서 가까운 위치를 구하는 함수
        /// </summary>
        /// <param name="t">트랜스폼 타겟</param>
        /// <param name="x">현재 위치</param>
        /// <returns></returns>
        public static Vector3 GetClosetPoint(Transform t, Vector3 x)
        {
            var position = t.position;
            Vector3 p = position;
            Vector3 q = position + t.right * 5;
            float k = Vector3.Dot((x - p), (q - p)) / Vector3.Dot((q - p), (q - p));
            Vector3 closet = p + k * (q - p);

            return closet;
        }
        /// <summary>
        /// Vector one,two 사이의 거리를 구하는 함수
        /// </summary>
        /// <param name="one">첫번째 위치/param>
        /// <param name="two">두번째 위치</param>
        /// <returns></returns>
        public static float GetDistance(Vector3 one, Vector3 two)
        {
            return Vector3.Distance(one,two);
        }
    
    }
}