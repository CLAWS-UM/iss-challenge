using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace suitInfo
{
    interface DisplayData
    {
        void Display();
    }
    public class BarData : DisplayData
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
            percentage = (float)value / (float)start;
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

        public void DisplayData.Display()
        {
            // add code to display each bar

        }
    }
    public class Simple
    {
        const int NUM_DATA = 11;
        readonly string[] units;

        public Simple(){
            units = { "mL","seconds",...};
        }
        public void change_unit(int index, string unit)
        {
            units[index] = unit;
        }

        // this function gets called when
        public void Display(Dictionary<string, int> data)
        {
            // add code to display simple text in Unity
                // takes in dictionary and prints each value on screen

        }
    }
}
