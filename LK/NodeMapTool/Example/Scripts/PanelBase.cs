
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LucienKlein
{


    public abstract class PanelBase : MonoBehaviour
    {
        protected CanvasGroup _canvasGroup;
        //[MMReadOnly]
        public string ID => GetID();

        private void Awake()
        {
            _canvasGroup=GetComponent<CanvasGroup>();
        }
        public virtual void Show()
        {
            if (_canvasGroup==null)
                _canvasGroup=GetComponent<CanvasGroup>();
            _canvasGroup.alpha=1;
            gameObject.SetActive(true);
        }
        //不要随便用 要用Mgr的HidePanel才能记录
        public virtual void Hide()
        {
            if (_canvasGroup==null)
                _canvasGroup=GetComponent<CanvasGroup>();
            _canvasGroup.alpha=0;
            gameObject.SetActive(false);
        }
        public virtual void Delete()
        {
            gameObject.DestroyMe();
        }

        public virtual void Close()
        {
            gameObject.DestroyMe();
            //UIMgr.Instance.DeletePanel(this);
        }


        string _id = string.Empty;
        string GetID()
        {
            if (_id==string.Empty)
            {
                _id=GetType().Name;
            }
            return _id;
        }

        ///生成时立即执行的方法.
        public virtual void Init()
        {

        }
    }

}
