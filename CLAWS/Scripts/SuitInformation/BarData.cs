using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace suitInfo
{
    /*interface DisplayData
    {
        void Display();
    } */
    public class BarData : MonoBehaviour
    {
        public enum Color {
            Red, Yellow, Green
        }

        int value;
        int start;
        float percentage;
        Color color;
        string title;

        public BarData() { }
        public BarData(int val, int per, string titl)
        {
            start = val; value = val;
            percentage = (float)value / (float)start; // for water, express percentage as portion until ambient
            title = titl;
            Update_color(percentage);
        }
        public void change_val(int val)
        {
            value = val;
            percentage = (float)value / (float)start;
            Update_color(percentage);
        }
        public void Update_color(float percent)
        {
            if (percent >= 50) color = Color.Green;
            else if (percent < 50 && percent >= 30) color = Color.Yellow;
            else color = Color.Red;
        }

        public void Display() {
            // add code to display each bar

        }
    };
    public class Simple
    {
        const int NUM_DATA = 11;
        readonly System.String[] units;
        System.String[] names;

        public Simple() {
            units = { "psia","degrees F", "RPM", "hr:min:sec", "psia", "psi/min", "amp-hr", "psia", "psia", "psia", "psi/min"};
            names = { "SUB PRESSURE", "SUB TEMPERATURE", "FAN TACHOMETER", "EXTRAVEHICULAR ACTIVITY TIME", "OXYGEN PRESSURE", "OXYGEN RATE", "BATTERY CAPACITY", "H2O GAS PRESSURE", ...
                "H2O LIQUID PRESSURE", "SOP PRESSURE", "SOP RATE"};
        }
        public void change_unit(int index, string unit)
        {
            units[index] = unit;
        }

        // this function gets called when
        public void Display(Dictionary<string, int> data)
        {


        }
    };

}
