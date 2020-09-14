using UnityEngine;
using UnityEngine.UI;

namespace ShootAR.Menu
{
    public class MuteButton : MonoBehaviour
    {
        [SerializeField]
        private Material soundOffIcon;
        [SerializeField]
        private Material soundOnIcon;
        [SerializeField]
        private GameObject menu;
        [SerializeField]
        private GameObject outliner;
        private Image image;
        MeshRenderer m_Renderer;

        public void ToggleSound()
        {
            Configuration.SoundMuted = !Configuration.SoundMuted;

            if (Configuration.SoundMuted)
            {
                if (menu.activeSelf) m_Renderer.material = soundOffIcon;
                else image.material = soundOffIcon;
                AudioListener.volume = 0.0f;
            }
            else
            {
                if (menu.activeSelf) m_Renderer.material = soundOnIcon;
                else image.material = soundOnIcon;
                AudioListener.volume = 1.0f;
            }
        }

        void OnMouseOver()
        {
            outliner.SetActive(true);
        }

        void OnMouseExit()
        {
            outliner.SetActive(false);
        }

        void OnMouseDown()
        {
            ToggleSound();
        }

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(ToggleSound);
            if (menu.activeSelf)
            {
                m_Renderer = GetComponent<MeshRenderer>();
                m_Renderer.material = Configuration.SoundMuted ? soundOffIcon : soundOnIcon;
            }
            else
            {
                image = GetComponent<Image>();
                image.material = Configuration.SoundMuted ? soundOffIcon : soundOnIcon;
            }
        }
    }
}

