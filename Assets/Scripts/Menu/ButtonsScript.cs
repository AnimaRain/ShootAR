using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace ShootAR.Menu
{
    public class ButtonsScript : MonoBehaviour
    {
        /// <summary>
        /// the number labelled on the button
        /// </summary>
        private int numberOnLabel;

        [SerializeField] private RoundSelectMenu roundMenu;
        [SerializeField] private MenuManager menuManager;
        private Color startcolor;
        private bool isInside = false;
        Material m_Material;

        void Start()
        {
            m_Material = GetComponent<Renderer>().material;
            startcolor = m_Material.color;
            if (menuManager == null && (GetComponent<MenuManager>() != null))
            {
                menuManager = GetComponent<MenuManager>();
            }
        }

        void OnMouseEnter()

        {
                m_Material.color = Color.red;
                isInside = true;
        }

        void OnMouseExit()
        {
                isInside = false;
                m_Material.color = startcolor;
        }

        void OnMouseDown()
        {
            if (isInside)
            {
                if (this.gameObject.name == "StartButton")
                {
                    SceneManager.LoadScene(1);
                }
                else if (this.gameObject.name == "RoundSelectButton")
                {
                    menuManager.ToRoundSelect();
                }
                else if (this.gameObject.name == "BackButton")
                {
                    menuManager.ToMainMenu();
                }
                else
                {
                    /* When a button on roundselect is pressed, add the number
			 * on its label to the level index. */
                    string label = GetComponentInChildren<TextMesh>().text.ToString();
                    if (int.TryParse(label, out numberOnLabel))
                    {
                        roundMenu.RoundToPlay += numberOnLabel;                        
                    }
                    else
                        Debug.LogError($"{label} is not an acceptable number. Try labeling" +
                            " the button with an integer and without using spaces, letters " +
                            "and other symbols. ('+', '-', '.' and 'e' are allowed)");
                }
            }
        }

    }
}
