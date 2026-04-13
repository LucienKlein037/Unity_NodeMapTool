using System;
using UnityEngine;
using UnityEngine.Events;

namespace LucienKlein
{

    public class MonoMgrHelper : UnityEngine.MonoBehaviour
    {
        public event UnityAction updateAction = null;
        //***********************************************************************************************************************************************/
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (updateAction != null)
            {
                updateAction();
            }
        }
        //***********************************************************************************************************************************************/
        public void AddUpdateListener(UnityAction action)
        {
            updateAction += action;
        }
        public void RemoveUpdateListener(UnityAction action)
        {
            if (updateAction==null)
            {
                return;
            }
            Delegate[] invocationList = updateAction.GetInvocationList();
            foreach (Delegate del in invocationList)
            {
                if (del.Equals(action))
                {
                    updateAction -= (UnityAction)del;
                    break;
                }
            }

        }
        public void ClearUpdateListener()
        {
            updateAction = null;
        }
        //***********************************************************************************************************************************************/














    }

}
