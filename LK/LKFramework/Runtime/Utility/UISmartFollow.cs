using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LucienKlein
{

    //让UI元素跟随目标.可以设置偏移,并且当超出屏幕时自动反转
    public class UISmartFollow : MonoBehaviour
    {
    public Transform UpdateTarget;

    public Vector2 offset;
    //自动在目标右方
    public bool IsAutoOffsetX;
    //自动在目标下方
    public bool IsAutoOffsetY;

    //是否反转的信息,外部可以知道信息
    public bool IsFlipX=false;
    public bool IsFlipY=false;

    Vector3 tempv=Vector3.zero;

    private void FixedUpdate()
    {
        if (UpdateTarget==null)
        {
            return;
        }
        SetTarget(UpdateTarget);
    }


    /// <summary>
    /// 暂时目标
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(Transform target)
    {
        Vector3 tempOffset = offset;
        IsFlipX = false;
        IsFlipY = false;

        //判定超出屏幕则反向
        Vector3[] worldCorners = new Vector3[4];
        //在边界时检测的超出边界的距离.每个点判定取最大那个
        float xMaxMove = 0;
        float yMaxMove = 0;

        if (target is RectTransform trt )
        {
            RectTransform rect = transform as RectTransform;

            if (trt.GetComponentInParent<Canvas>()==null)
            {
                return;
            }

            if (trt.GetComponentInParent<Canvas>().renderMode==RenderMode.ScreenSpaceOverlay)
            {
            
                if (IsAutoOffsetY)
                {
                    tempOffset.x=0;
                    tempOffset.y=-(trt.rect.height * trt.lossyScale.y*.5f+rect.rect.height*rect.lossyScale.y*.5f+1);
                }
                if (IsAutoOffsetX)
                {
                    tempOffset.x=trt.rect.width * trt.lossyScale.x*.5f+rect.rect.width*rect.lossyScale.x*.5f+1;
                    tempOffset.y=trt.rect.height*rect.lossyScale.y*0.5f-rect.rect.height*rect.lossyScale.y*0.5f;
                }
                transform.position=target.position;
                transform.position+=tempOffset;

                //判定超出屏幕则反向
                worldCorners = new Vector3[4];
                rect.GetWorldCorners(worldCorners);
                //在边界时检测的超出边界的距离.每个点判定取最大那个
                xMaxMove = 0;
                yMaxMove = 0;
                // 遍历四个角
                foreach (Vector3 corner in worldCorners)
                {
                    //// 转换为屏幕坐标
                    Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(null, corner);

                    if (tempOffset.x==0)
                    {
                        if (screenPoint.x<0)
                        {
                            xMaxMove= Mathf.Max(xMaxMove, Mathf.Abs(screenPoint.x-0));
                        }
                        else if (screenPoint.x>Screen.width)
                        {
                            xMaxMove=Mathf.Max(xMaxMove, Mathf.Abs(Screen.width-screenPoint.x));
                        }
                    }

                    if (screenPoint.x > Screen.width)
                    {
                        IsFlipX=true;
                    }

                    if (screenPoint.y < 0)
                    {
                        //Debug.Log(screenPoint.y+"----screenPoint.y");

                        IsFlipY=true;
                    }

                }
                if (tempOffset.x==0)
                {
                    //Debug.Log(xMaxMove+"----xMaxMove");

                    tempOffset.x+=xMaxMove;
                }
                if (tempOffset.y==0)
                {
                    tempOffset.y+=yMaxMove;
                }

                if (IsFlipX)
                {
                    tempOffset.x *=-1;
                }
                if (IsFlipY)
                {
                    tempOffset.y *=-1;
                }

                transform.position=target.position;
                transform.position+=tempOffset;
            }
            else
            {
                //这里还需要再乘以lossyScale
                if (IsAutoOffsetY)
                {
                    tempOffset.x=0;
                    tempOffset.y=-(trt.rect.height *rect.lossyScale.y*.5f+rect.rect.height*rect.lossyScale.y*.5f+1);
                }
                if (IsAutoOffsetX)
                {
                    tempOffset.x=trt.rect.width * rect.lossyScale.x*.5f+rect.rect.width*rect.lossyScale.x*.5f+1;
                    //tempOffset.y=trt.rect.height*rect.lossyScale.y*0.5f-rect.rect.height*rect.lossyScale.y*0.5f;
                }
                Vector3 worldPos = target.position;

                Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

                RectTransformUtility.ScreenPointToWorldPointInRectangle(
                    transform.parent as RectTransform,
                    screenPos,
                    null, // 如果 Canvas 是 Overlay 模式，传 null
                    out worldPos
                );
                transform.position=worldPos;
                transform.position+=tempOffset;


                //判定超出屏幕则反向
                worldCorners = new Vector3[4];
                rect.GetWorldCorners(worldCorners);
                //在边界时检测的超出边界的距离.每个点判定取最大那个
                xMaxMove = 0;
                yMaxMove = 0;
                // 遍历四个角
                foreach (Vector3 corner in worldCorners)
                {
                    //// 转换为屏幕坐标
                    Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(null, corner);

                    if (tempOffset.x==0)
                    {
                        if (screenPoint.x<0)
                        {
                            xMaxMove= Mathf.Max(xMaxMove, Mathf.Abs(screenPoint.x-0));
                        }
                        else if (screenPoint.x>Screen.width)
                        {
                            xMaxMove=Mathf.Max(xMaxMove, Mathf.Abs(Screen.width-screenPoint.x));
                        }
                    }

                    if (screenPoint.x > Screen.width)
                    {
                        IsFlipX=true;
                    }

                    if (screenPoint.y < 0)
                    {
                        //Debug.Log(screenPoint.y+"----screenPoint.y");

                        IsFlipY=true;
                    }

                }
                if (tempOffset.x==0)
                {
                    //Debug.Log(xMaxMove+"----xMaxMove");

                    tempOffset.x+=xMaxMove;
                }
                if (tempOffset.y==0)
                {
                    tempOffset.y+=yMaxMove;
                }

                if (IsFlipX)
                {
                    tempOffset.x *=-1;
                }
                if (IsFlipY)
                {
                    tempOffset.y *=-1;
                }
                transform.position=worldPos;
                transform.position+=tempOffset;
            }

        

        }
        else
        {

            Vector3 targetPos ;

            const int pixelperunit = 16;

            Canvas transformCanvas= transform.GetComponentInParent<Canvas>();

            if (transformCanvas!=null&& transformCanvas.renderMode==RenderMode.ScreenSpaceOverlay)
            {
                targetPos = Camera.main.WorldToScreenPoint(target.position);

                tempv.x=targetPos.x+offset.x*pixelperunit;
                tempv.y=targetPos.y+offset.y*pixelperunit;
                tempv.z=transform.position.z;
            }
            else
            {
                targetPos = target.position;

                tempv.x=targetPos.x+offset.x;
                tempv.y=targetPos.y+offset.y;
                tempv.z=transform.position.z;

            }
            transform.position=tempv;
        }


    }

    public void Clear()
    {
        UpdateTarget=null;
    }

    }

}
