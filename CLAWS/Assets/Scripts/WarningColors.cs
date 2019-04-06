using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace suitInfo
{

    public class WarningColors : MonoBehaviour
    {
        public enum WarningLabel
        {
            Red, Green
        }

        public void startDisplay(GameObject obj)
        {
            SpriteRenderer m_SpriteRenderer;
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_SpriteRenderer.color = Color.green;
        }

        public void Display(GameObject obj, bool trigger)
        {
            SpriteRenderer m_SpriteRenderer;
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            // add code to display each bar
            if (trigger)
            {
                //display red
                m_SpriteRenderer.color = Color.red;
            }
            else
            {
                //Display green
                m_SpriteRenderer.color = Color.green;
            }
        }
    };

}
