using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace LucienKlein
{
    //using Lean.Transition;

    public static class UnityExtensionMethod
    {
        #region Vector2

        public static Vector2 GetNewVector2WithX(this Vector2 oldPos, float x)
        {
            Vector3 newV3 = new Vector2(x, oldPos.y);
            return newV3;
        }
        public static Vector2 GetNewVector2WithY(this Vector2 oldPos, float y)
        {
            Vector3 newV3 = new Vector2(oldPos.x, y);
            return newV3;
        }
        #endregion
        #region Vector3
        public static Vector3 Multiply(this Vector3 target, Vector3 tempV3)
        {
            target.x*= tempV3.x;
            target.y*= tempV3.y;
            target.z*= tempV3.z;
            return target;
        }
        public static Vector3 Multiply(this Vector3 target, float f)
        {
            target.x*= f;
            target.y*= f;
            target.z*= f;
            return target;
        }
        public static Vector3 GetPositiveNegativeValue(this Vector3 target)
        {
            Vector3 pn = new Vector3(GetValue(target.x), GetValue(target.y), GetValue(target.z));


            return pn;

            float GetValue(float f)
            {
                return f>=0 ? 1 : -1;

            }
        }


        public static Vector3 GetNewVector3WithX(this Vector3 oldPos, float x)
        {
            Vector3 newV3 = new Vector3(x, oldPos.y, oldPos.z);
            return newV3;
        }
        public static Vector3 GetNewVector3WithY(this Vector3 oldPos, float y)
        {
            Vector3 newV3 = new Vector3(oldPos.x, y, oldPos.z);
            return newV3;
        }
        public static Vector3 GetNewVector3WithZ(this Vector3 oldPos, float z)
        {
            Vector3 newV3 = new Vector3(oldPos.x, oldPos.y, z);
            return newV3;
        }
        //public static void SetX(this Vector3 oldPos, float x)
        //{
        //    oldPos.x = x;
        //}
        //public static void SetY(this Vector3 oldPos, float y)
        //{
        //    oldPos.y = y;
        //}
        //public static void SetZ(this Vector3 oldPos, float z)
        //{
        //    Vector3 newV3 = new Vector3(oldPos.x, oldPos.y, z);
        //    oldPos = newV3;
        //}

        #endregion


        #region Transform

        // 放最上面
        public static void ShowInTop(this Transform transform)
        {
            transform.SetAsLastSibling();
        }

        public static Vector3 LocalToWorld(this Transform t, Vector3 v)
        {
            return t.TransformPoint(v);
        }
        public static Vector3 WorldToLocal(this Transform t, Vector3 v)
        {
            return t.InverseTransformPoint(v);
        }

        public static void FlipX(this Transform tra)
        {
            Vector3 tempV = tra.localScale;
            tempV.x*=-1;
            tra.localScale = tempV;
        }
        public static void ClearChildGameObject(this Transform transform)
        {
            if (transform==null)
            {
                return;
            }
            if (transform.childCount==0)
            {
                return;
            }
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }
        }
        public static void LocalMove(this Transform transform, float x, float y = 0, float z = 0)
        {
            Vector3 currentPos = transform.position;
            currentPos.x+= x;
            currentPos.y+= y;
            currentPos.z+= z;
            transform.position=currentPos;
            return;
        }
        public static IEnumerator MoveTo2D(this Transform transform, Vector3 targetPosition, float distance = 0, float moveTime = .2f, int smoothness = 20)
        {

            WaitForSeconds waitTime = new WaitForSeconds(moveTime/smoothness);
            Vector3 startPosition = transform.position;
            targetPosition.z=startPosition.z;

            if (distance!=0)
            {
                float totalDistance = Vector3.Distance(startPosition, targetPosition);
                float percent = Mathf.Max((totalDistance- distance)/totalDistance, 0f);
                targetPosition=Vector3.Lerp(startPosition, targetPosition, percent);
            }
            for (int i = 1; i <= smoothness; i++)
            {
                transform.position=Vector3.Lerp(startPosition, targetPosition, 1f/smoothness*i);
                yield return waitTime;
            }
        }
        public static IEnumerator MoveTo2DSameY(this Transform transform, Vector3 targetPosition, float distance = 0, float moveTime = .2f, int smoothness = 20)
        {

            WaitForSeconds waitTime = new WaitForSeconds(moveTime/smoothness);
            Vector3 startPosition = transform.position;
            targetPosition.z=startPosition.z;
            if (distance!=0)
            {
                targetPosition.x=targetPosition.x>startPosition.x ? targetPosition.x-distance : targetPosition.x+distance;
            }
            for (int i = 1; i <= smoothness; i++)
            {
                transform.position=Vector3.Lerp(startPosition, targetPosition, 1f/smoothness*i);
                yield return waitTime;
            }
        }
        public static void SetPosX(this Transform transform, float x)
        {
            Vector3 newV3 = new Vector3(x, transform.position.y, transform.position.z);
            transform.position = newV3;
        }
        public static void SetPosY(this Transform transform, float y)
        {
            Vector3 newV3 = new Vector3(transform.position.x, y, transform.position.z);
            transform.position = newV3;
        }
        public static void SetPosZ(this Transform transform, float z)
        {
            Vector3 newV3 = new Vector3(transform.position.x, transform.position.y, z);
            transform.position = newV3;
        }
        public static void SetPosNoZ(this Transform transform, Vector3 pos)
        {
            Vector3 newV3 = new Vector3(pos.x, pos.y, transform.position.z);
            transform.position = newV3;
        }

        public static void SmartSetParent(this Transform transform, Transform Parent)
        {
            if (transform is RectTransform)
            {
                transform.SetParent(Parent, false);
                transform.SetAsLastSibling();
            }
            else
            {
                transform.SetParent(Parent);
            }
        }

        public static void RightFaceDirection(this Transform transform, Vector3 direction)
        {
            if (direction == Vector3.zero)
                return;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        public static void RightFaceTo(this Transform transform, Vector3 target)
        {
            transform.RightFaceDirection(target-transform.position);
        }


        public static RectTransform GetRectTransform(this Transform tra)
        {
            return tra as RectTransform;
        }

        #endregion


        #region RectTransform


        public static Canvas GetCanvas(this RectTransform rectTransform)
        {
            Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
            return canvas;


        }

        #endregion


        #region MonoBehaviour
        public static void Invoke(this UnityEngine.MonoBehaviour mono, float second, UnityAction action)
        {
            mono.StartCoroutine(DelayToInvoke(second, action));

            IEnumerator DelayToInvoke(float second, UnityAction action)
            {
                if (second>0)
                {
                    yield return new WaitForSeconds(second);
                }
                else if (second<0)
                {
                    int frameCount = (int)Mathf.Ceil(-1*second);
                    Debug.Log("信息"+Time.deltaTime*frameCount);


                    yield return new WaitForSeconds(Time.deltaTime*frameCount);
                }
                else
                {
                    yield return null;
                }
                action?.Invoke();

            }
        }
        #endregion


        #region SpriteRenderer
        public static void TrySetSprite(this SpriteRenderer sr, string path)
        {
            Sprite sprite = ResMgr.Instance.Load<Sprite>(path);
            if (sprite!=null)
            {
                sr.sprite = sprite;
            }
        }

        #endregion


        #region GameObject
        public static void DestroyMe(this GameObject obj)
        {
            GameObject.Destroy(obj);
        }
        public static GameObject InstantiateMe(this GameObject obj)
        {
            return GameObject.Instantiate(obj);
        }
        public static T InstantiateMeReturnComponent<T>(this GameObject obj) where T : Component
        {

            return GameObject.Instantiate(obj).GetComponent<T>();
        }


        #endregion

        #region Color

        public static string ColorToHex(this Color color, bool includeAlpha = false)
        {
            Color32 c = color; // 自动转换为 0-255 范围
            if (includeAlpha)
                return $"#{c.r:X2}{c.g:X2}{c.b:X2}{c.a:X2}";
            else
                return $"#{c.r:X2}{c.g:X2}{c.b:X2}";
        }

        #endregion
    }

}
