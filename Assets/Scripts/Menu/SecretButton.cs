using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootAR.Menu
{
    public class SecretButton : MonoBehaviour
    {

        [SerializeField]
        private MenuManager menuManager;

        void OnMouseDown()
        {
            menuManager.ActivateClouds();
        }
    }
}
