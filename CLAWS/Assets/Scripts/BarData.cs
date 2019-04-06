using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace suitInfo
{
    
    public class BarData : MonoBehaviour
    {
        public enum WarningLabel {
            Red, Green
        }

        double value;
        double start;
        float percentage;
        string title;
        WarningLabel color;
        int natavg_up = 100;
        int natavg_down = 30;
        GameObject titleObject;
        GameObject valueObject;

        public BarData() {
            value = 0.0; start = 0.0;
            title = "display data";
            percentage = (float)0.0;
        }
        public BarData(double val, double per, string titl)
        {
            start = val; value = val;
            percentage = (float)value / (float)start; // for water, express percentage as portion until ambient
            title = titl;
            Update_color(percentage);
        }
        public void change_val(double val)
        {
            value = val;
            percentage = (float)value / (float)start;
            if(title == "heartbpm")
            {
                if (val > natavg_up || val < natavg_down) color = WarningLabel.Red;
                else color = WarningLabel.Green;
            }
            else Update_color(percentage);
        }
        public void Update_color(float percent)
        {
            if (percent >= 30) color = WarningLabel.Green;
            //else if (percent < 50 && percent >= 30) color = WarningLabel.Yellow;
            else color = WarningLabel.Red;
        }
        public void startDisplay(GameObject obj)
        {
            
            //Debug.Log(value);
            obj.GetComponent<TextMesh>().text = value.ToString();
        }

        public void Display(GameObject obj) {
            // add code to display each bar

            obj.GetComponent<TextMesh>().text = value.ToString();

        }
        public void DisplayCircle(GameObject obj)
        {
            SpriteRenderer m_SpriteRenderer;
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            // add code to display each bar
            if (color == WarningLabel.Red)
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
        public void initialize(double val, string titl)
        {
            start = val; value = val;
            percentage = (float)value / (float)start;
            title = titl;
            Update_color(percentage);
        }
    };

}
