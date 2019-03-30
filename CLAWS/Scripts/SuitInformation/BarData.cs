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
            Red, Yellow, Green
        }

        double value;
        double start;
        float percentage;
        WarningLabel color;
        string title;
        GameObject titleObject;
        GameObject valueObject;

        public BarData() {
            value = 0.0; start = 0.0;
            title = "display data";
            color = WarningLabel.Green;
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
            Update_color(percentage);
        }
        public void Update_color(float percent)
        {
            if (percent >= 50) color = WarningLabel.Green;
            else if (percent < 50 && percent >= 30) color = WarningLabel.Yellow;
            else color = WarningLabel.Red;
        }
        public void startDisplay(GameObject obj)
        {

            Debug.Log(value);
            obj.GetComponent<TextMesh>().text = value.ToString();
            //Debug.Log(title);
            //titleObject.GetComponent<TextMesh>().text = title.ToString();
        }

        public void Display(GameObject obj) {
            // add code to display each bar
            if (color == WarningLabel.Red){
                title = "oh shit";
                value = 0;
            }

            Debug.Log(value);
            obj.GetComponent<TextMesh>().text = value.ToString();

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