/*  SuitInformation.cs

    Contains all class definitions and functions related 
    to initializing and updating the suit/health information
    panel displayed on the lower right hand corner of the 
    Hololens display.

    POC: Emily Rassel
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking; // Required for web request

namespace suitInfo
{
    class SuitInformation : MonoBehaviour
    {
        /* --- Object Declarations --- */

        // Declare GameObjects for suit/health info 
        //   panel metrics
        public GameObject bpm;
        public GameObject oxy;
        public GameObject rate_oxy;
        public GameObject bat_life;
        public GameObject wat;
        public GameObject subp;
        public GameObject subt;
        public GameObject time_bat;
        public GameObject time_wat;
        public GameObject time_oxy;

        // Declare circle icons for indicator color
        public GameObject bpm_circle;
        public GameObject oxy_circle;
        public GameObject rate_oxy_circle;
        public GameObject bat_life_circle;
        public GameObject wat_circle;
        public GameObject subp_circle;
        public GameObject subt_circle;

        // Declare warning GameObjects
        public GameObject warningPicture;
        public GameObject warningsentence;

        // Declare BarData objects for each metric
        public BarData heartbpm;
        public BarData sub_press;
        public BarData sub_temp;
        public BarData oxygen;
        public BarData rate_oxygen;
        public BarData battery_life;
        public BarData water;

        // Declare SuitInfo object for telemetry data
        public SuitInfo telemetry_data;

        // Delcare WarningInfo object that 
        //   handles warning logic
        public WarningInfo warnings;

        // Declare TextMeshes for text component 
        //   of suit/health info GameObjects
        public TextMesh bpmText;
        public TextMesh oxyText;
        public TextMesh rate_oxyText;
        public TextMesh bat_lifeText;
        public TextMesh watText;
        public TextMesh subpText;
        public TextMesh subtText;
        public TextMesh time_batText;
        public TextMesh time_watText;
        public TextMesh time_oxyText;

        // Declare Text object for warning text
        public Text warningText;
        
        // Universal font size  
        public int TextFontSize = 15;

        // URLs used for telemetry reading
        public string suitURL = "https://suits-nasa-server.herokuapp.com/api/suit/recent";
        public string warningURL = "https://iss-program.herokuapp.com/api/suitswitch/recent";

        // Used for 2 minute telemetry time delay
        float delay;
        float nextTime = 0.0f;
        string json = "";


        // --- Class Definitions ---

        /*  --- SuitInfo ---
            The class definition of the SuitInfo object.
            This contains the string representations for the various metrics.
        */
        public class SuitInfo
        {
            // Declare strings for suit/health info
            public string create_date { get; set; }
            public string heart_bpm { get; set; }
            public string p_sub { get; set; }
            public string p_suit { get; set; }
            public string t_sub { get; set; }
            public string v_fan { get; set; }
            public string p_o2 { get; set; }
            public string rate_o2 { get; set; }
            public string cap_battery { get; set; }
            public string p_h2o_g { get; set; }
            public string p_h2o_l { get; set; }
            public string p_sop { get; set; }
            public string rate_sop { get; set; }
            public string t_battery { get; set; }
            public string t_oxygen { get; set; }
            public string t_water { get; set; }

            // Constructor
            //   Initialize everything to zero
            public SuitInfo()
            {
                this.create_date = "0";
                this.heart_bpm = "0";
                this.p_sub = "0";
                this.p_suit = "0";
                this.t_sub = "0";
                this.v_fan = "0";
                this.p_o2 = "0";
                this.rate_o2 = "0";
                this.cap_battery = "0";
                this.p_h2o_g = "0";
                this.p_h2o_l = "0";
                this.p_sop = "0";
                this.rate_sop = "0";
                this.t_battery = "0";
                this.t_oxygen = "0";
                this.t_water = "0";
            }
        }

        /*  --- WarningInfo ---
            The class definition of the WarningInfo object.
            This powers the logic for displaying the WarningImage.
        */
        public class WarningInfo
        {
            // Declare bools for warning triggers
            public string create_date { get; set; }
            public bool sop_on { get; set; }
            public bool sspe { get; set; }
            public bool fan_error { get; set; }
            public bool vent_error { get; set; }
            public bool vehicle_power { get; set; }
            public bool h2o_off { get; set; }
            public bool o2_off { get; set; }

            // Constructor
            //   Initialize all triggers to false
            public WarningInfo()
            {
                this.create_date = "0";
                this.sop_on = false;
                this.sspe = false;
                this.fan_error = false;
                this.vent_error = false;
                this.vehicle_power = false;
                this.h2o_off = false;
                this.o2_off = false;
            }
        }

        /*  --- BarData ---
            The class definition of the BarData object.
            Displayed within the Heath Information panel for 
            information readouts.
        */
        public class BarData : MonoBehaviour
        {
            // Contains the various indicator color 
            //   options for the circular icon
            public enum WarningLabel
            {
                // Red = bad
                // Green = good
                // Purple = stale info
                Red, Green, Purple
            }

            // Member variables
            public double value;
            double start;
            float percentage;
            string title;
            WarningLabel color;
            int natavg_up = 100;
            int natavg_down = 30;
            GameObject titleObject;
            GameObject valueObject;

            // Constructors
            public BarData()
            {
                value = 0.0; 
                start = 0.0;
                title = "display data";
                percentage = (float)0.0;
            }

            // Initializes/Resets existing BarData object using input params
            public void initialize(double value_in, string title_in)
            {
                value = value_in;
                start = value_in; 
                percentage = (float)value / (float)start;
                title = title_in;
                Update_color(percentage);
            }

            // Changes the value of the data and updates 
            //    percentage and indicator color
            public void change_val(double val)
            {
                value = val;
                percentage = (float)value / (float)start;
                if (title == "heartbpm")
                {
                    if (val > natavg_up || val < natavg_down) color = WarningLabel.Red;
                    else color = WarningLabel.Green;
                }
                else Update_color(percentage);
            }

            // Updates the indicator color based on the percentage
            public void Update_color(float percent)
            {
                if (percent >= 30) color = WarningLabel.Green;
                //else if (percent < 50 && percent >= 30) color = WarningLabel.Yellow;
                else color = WarningLabel.Red;
            }

            // Initializes the value to a text component on-screen
            public void startDisplay(GameObject obj)
            {
                obj.GetComponent<TextMesh>().text = value.ToString();
            }

            // 
            public void Display(TextMesh mesh)
            {
                mesh.text = value.ToString();
                mesh.fontSize = 25;
            }

            // Renders the circle with the proper indicator color
            //  If result = 0, stale data, so purple circle.
            public void DisplayCircle(GameObject obj, int result)
            {
                SpriteRenderer m_SpriteRenderer;
                m_SpriteRenderer = obj.GetComponent<SpriteRenderer>();
                // add code to display each bar

                if (result == 0) color = WarningLabel.Purple;

                if (title != "heartbpm")
                {
                    if (color == WarningLabel.Red)
                    {
                        //display red
                        m_SpriteRenderer.color = Color.red;
                    }
                    else if (color == WarningLabel.Green)
                    {
                        //Display green
                        m_SpriteRenderer.color = Color.green;
                    }
                    else
                    {
                        m_SpriteRenderer.color = new Color(143, 0, 254, 1);
                        // Alt: Color(161, 12, 232, 91);
                    }
                }
            }
        }

        /*  --- Start Function ---
            Initializes UI/UX and variable values
        */
        void Start()
        {
            // Suit and Warning Info
            telemetry_data = new SuitInfo();
            warnings = new WarningInfo();

            // Add TextMeshes to suit/health info sections
            bpmText = bpm.AddComponent<TextMesh>();
            oxyText = oxy.AddComponent<TextMesh>();
            bat_lifeText = bat_life.AddComponent<TextMesh>();
            watText = wat.AddComponent<TextMesh>();
            subpText = subp.AddComponent<TextMesh>();
            subtText = subt.AddComponent<TextMesh>();
            warningText = warningsentence.GetComponent<Text>();
            time_batText = time_bat.AddComponent<TextMesh>();
            time_watText = time_wat.AddComponent<TextMesh>();
            time_oxyText = time_oxy.AddComponent<TextMesh>();
            rate_oxyText = rate_oxy.AddComponent<TextMesh>();

            // --- Styling ---
            // Font size
            bpmText.fontSize = TextFontSize;
            oxyText.fontSize = TextFontSize;
            rate_oxyText.fontSize = TextFontSize;
            bat_lifeText.fontSize = TextFontSize;
            watText.fontSize = TextFontSize;
            subpText.fontSize = TextFontSize;
            subtText.fontSize = TextFontSize;
            time_batText.fontSize = TextFontSize;
            time_watText.fontSize = TextFontSize;
            time_oxyText.fontSize = TextFontSize;

            // Alignment
            bpmText.alignment = TextAlignment.Right;
            oxyText.alignment = TextAlignment.Right;
            rate_oxyText.alignment = TextAlignment.Right;
            bat_lifeText.alignment = TextAlignment.Right;
            watText.alignment = TextAlignment.Right;
            subpText.alignment = TextAlignment.Right;
            subtText.alignment = TextAlignment.Right;
            time_batText.alignment = TextAlignment.Right;
            time_watText.alignment = TextAlignment.Right;
            time_oxyText.alignment = TextAlignment.Right;

            // Anchor/Alignment
            bpmText.anchor = TextAnchor.MiddleRight;
            oxyText.anchor = TextAnchor.MiddleRight;
            rate_oxyText.anchor = TextAnchor.MiddleRight;
            bat_lifeText.anchor = TextAnchor.MiddleRight;
            watText.anchor = TextAnchor.MiddleRight;
            subpText.anchor = TextAnchor.MiddleRight;
            subtText.anchor = TextAnchor.MiddleRight;
            time_batText.anchor = TextAnchor.MiddleRight;
            time_watText.anchor = TextAnchor.MiddleRight;
            time_oxyText.anchor = TextAnchor.MiddleRight;

            // update the visual scale
            // transform.localScale = new Vector3(8, 3, 0);

            // Add the BarData component (which includes the circles)
            heartbpm = bpm.AddComponent<BarData>();
            oxygen = oxy.AddComponent<BarData>();
            battery_life = bat_life.AddComponent<BarData>();
            water = wat.AddComponent<BarData>();
            sub_press = subp.AddComponent<BarData>();
            sub_temp = subt.AddComponent<BarData>();
            rate_oxygen = rate_oxy.AddComponent<BarData>();

            // Initialize the health/suit info to reflect
            //   the data it holds
            heartbpm.initialize(0, "heartbpm");
            oxygen.initialize(0, "oxygen");
            rate_oxygen.initialize(0, "rate oxygen");
            battery_life.initialize(0, "battery life");
            water.initialize(0, "water");
            sub_press.initialize(0, "sub pressure");
            sub_temp.initialize(0, "sub temperature");
           
            // Set suit/health info text to hold initial data
            bpmText.text = telemetry_data.heart_bpm.ToString();
            oxyText.text = telemetry_data.p_o2.ToString();
            rate_oxyText.text = telemetry_data.rate_o2.ToString();
            bat_lifeText.text = telemetry_data.cap_battery.ToString();
            watText.text = telemetry_data.p_h2o_g.ToString();
            subpText.text = telemetry_data.p_sub.ToString();
            subtText.text = telemetry_data.t_sub.ToString();
            time_batText.text = telemetry_data.t_battery.ToString();
            time_watText.text = telemetry_data.t_water.ToString();
            time_oxyText.text = telemetry_data.t_oxygen.ToString();

            // Render the indicator color for the circle/icon
            heartbpm.DisplayCircle(bpm_circle, 1);
            oxygen.DisplayCircle(oxy_circle, 1);
            rate_oxygen.DisplayCircle(rate_oxy_circle, 1);
            battery_life.DisplayCircle(bat_life_circle, 1);
            water.DisplayCircle(wat_circle, 1);
            sub_press.DisplayCircle(subp_circle, 1);
            sub_temp.DisplayCircle(subt_circle, 1);
        }

        /* --- Update Functions --- */

        // General Unity Update Function
        // Updates based on time interval.
        void Update()
        {
            // Update telemetry data every two minutes 
            //   to better reflect resources during EVAs
            if (Time.time >= nextTime)
            {
                StartCoroutine(UpdateSuitInformation());
                delay = 120;
                nextTime += delay;
            }
        }

        // Updates the values/colors in the suit/health 
        //    info panel accordingly
        IEnumerator UpdateSuitInformation()
        {
            // Request telemetry data from server
            yield return StartCoroutine(GetText(suitURL));
            yield return StartCoroutine(GetText(warningURL));

            // Update the values of displayed data
            heartbpm.change_val(Convert.ToDouble(telemetry_data.heart_bpm));
            oxygen.change_val(Convert.ToDouble(telemetry_data.p_o2));
            rate_oxygen.change_val(Convert.ToDouble(telemetry_data.rate_o2));
            battery_life.change_val(Convert.ToDouble(telemetry_data.cap_battery));
            water.change_val(Convert.ToDouble(telemetry_data.p_h2o_g));
            sub_press.change_val(Convert.ToDouble(telemetry_data.p_sub));
            sub_temp.change_val(Convert.ToDouble(telemetry_data.t_sub));

            // Get Unity TextMeshes and update numeric values.
            bpmText = bpm.GetComponent<TextMesh>();
            oxyText = oxy.GetComponent<TextMesh>();
            rate_oxyText = rate_oxy.GetComponent<TextMesh>();
            bat_lifeText = bat_life.GetComponent<TextMesh>();
            watText = wat.GetComponent<TextMesh>();
            subpText = subp.GetComponent<TextMesh>();
            subtText = subt.GetComponent<TextMesh>();
            time_batText = time_bat.GetComponent<TextMesh>();
            time_watText = time_wat.GetComponent<TextMesh>();
            time_oxyText = time_oxy.GetComponent<TextMesh>();

            bpmText.text = heartbpm.value.ToString();
            oxyText.text = oxygen.value.ToString();
            rate_oxyText.text = rate_oxygen.value.ToString();
            bat_lifeText.text = battery_life.value.ToString();
            watText.text = water.value.ToString();
            subpText.text = sub_press.value.ToString();
            subtText.text = sub_temp.value.ToString();
            time_batText.text = telemetry_data.t_battery.ToString();
            time_watText.text = telemetry_data.t_water.ToString();
            time_oxyText.text = telemetry_data.t_oxygen.ToString();

            // Request for telemetry info
            UnityWebRequest www = UnityWebRequest.Get(suitURL);
            // Checks for request errors.
            if (www.responseCode == -1)
            {
                // Stale data - make circles purple
                heartbpm.DisplayCircle(bpm_circle, 0);
                oxygen.DisplayCircle(oxy_circle, 0);
                rate_oxygen.DisplayCircle(rate_oxy_circle, 0);
                battery_life.DisplayCircle(bat_life_circle, 0);
                water.DisplayCircle(wat_circle, 0);
                sub_press.DisplayCircle(subp_circle, 0);
                sub_temp.DisplayCircle(subt_circle, 0);
            }
            else
            {
                // Fresh data! - update circle colors
                heartbpm.DisplayCircle(bpm_circle, 1);
                oxygen.DisplayCircle(oxy_circle, 1);
                rate_oxygen.DisplayCircle(rate_oxy_circle, 1);
                battery_life.DisplayCircle(bat_life_circle, 1);
                water.DisplayCircle(wat_circle, 1);
                sub_press.DisplayCircle(subp_circle, 1);
                sub_temp.DisplayCircle(subt_circle, 1);
            }

            // Display image and warning text if 
            //   any warning switch is triggered
            if (warnings.sop_on)
            {
                warningPicture.SetActive(true);
                warningText.text = 
                    "Secondary Oxygen Pack is active";
            }
            else if (warnings.sspe)
            {
                warningPicture.SetActive(true);
                warningText.text = 
                    "Spacesuit pressure";
            }
            else if (warnings.fan_error)
            {
                warningPicture.SetActive(true);
                warningText.text = 
                    "Cooling fan of the spacesuit has a failure";
            }
            else if (warnings.vent_error)
            {
                warningPicture.SetActive(true);
                warningText.text = 
                    "No ventilation flow is detected";
            }
            else if (warnings.vehicle_power)
            {
                warningPicture.SetActive(true);
                warningText.text = 
                    "Spacesuit is receiving power through spacecraft";
            }
            else if (warnings.h2o_off)
            {
                warningPicture.SetActive(true);
                warningText.text = 
                    "H2O system is offline";
            }
            else if (warnings.o2_off)
            {
                warningPicture.SetActive(true);
                warningText.text = 
                    "O2 system is offline";
            }
            else
            {
                warningPicture.SetActive(false);
                warningText.text = "";
            }
        }
        
        /* --- Telemetry Functions --- */

        // Requests telemetry stream info and 
        //   updates SuitInfo and WarningInfo objects
        IEnumerator GetText(string url)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            // Checks for request errors
            if (www.responseCode == -1)
            {
                Debug.Log("Error:");
                Debug.Log(www.error);
            }
            else
            {
                // Save results as text
                // Debugging helpers:
                // Debug.Log("Downloaded:");
                // Debug.Log(www.downloadHandler.text);
                json = "";
                json = www.downloadHandler.text;
                Debug.Log(json);

                // Parse the JSON string and update
                //   SuitInfo and WarningInfo objects
                if (json != "")
                {
                    if (url == suitURL)
                    {
                        Update_Suit_Info(json);
                    }
                    else
                    {
                        Update_Warnings(json);
                    }
                }
            }
        }

        /*  --- JSON File Parsers ---
            Note: This is custom to the telemetry data format. 
            It must be updated if JSON were to change order
            
            ---- Current Format for Suit Information: ---- 
            [{"create_date":"2019-04-05T02:27:13.091Z","heart_bpm":"85",
            "p_sub":"7.95","t_sub":"6","v_fan":"39989","p_o2":"15",
            "rate_o2":"1.0","cap_battery":"30","p_h2o_g":"15",
            "p_h2o_l":"16","p_sop":"887","rate_sop":"0.9",
            "t_battery":"-5:-9:-28","t_oxygen":"8:35:9","t_water":"8:35:9"}]
        */
        void Update_Suit_Info(string json)
        {
            // Creates string array from input json string and removes symbols
            json = json.Trim(new Char[] { '[', ']', '}', '{' });
            json = json.Replace('"', ' ').Trim();
            string[] comps = json.Split(',');

            for (int i = 0; i < comps.Length; i++)
            {
                // Removes input titles and keeps numeric values
                comps[i] = comps[i].Substring(comps[i].IndexOf(':') + 1);
                comps[i] = comps[i].Replace('.', ',').Trim();
                comps[i] = comps[i].Trim(' ');
                Debug.Log(comps[i]);
            }

            // Updates values in SuitInfo object
            telemetry_data.create_date = comps[0];
            telemetry_data.heart_bpm = comps[1];
            telemetry_data.p_sub = comps[2];
            telemetry_data.p_suit = comps[3];
            telemetry_data.t_sub = comps[4];
            telemetry_data.v_fan = comps[5];
            telemetry_data.p_o2 = comps[6];
            telemetry_data.rate_o2 = comps[7];
            telemetry_data.cap_battery = comps[8];
            telemetry_data.p_h2o_g = comps[9];
            telemetry_data.p_h2o_l = comps[10];
            telemetry_data.p_sop = comps[11];
            telemetry_data.rate_sop = comps[12];
            telemetry_data.t_battery = comps[13];
            telemetry_data.t_oxygen = comps[14];
            telemetry_data.t_water = comps[15];
        }

        /*  ----Current Format for Warning Information: ----

            [{"create_date":"2019-04-05T02:52:31.307Z","sop_on":false,
            "sspe":false,"fan_error":false,"vent_error":false,
            "vehicle_power":false,"h2o_off":false,"o2_off":false}]
        */
        void Update_Warnings(string json)
        {
            // Creates string array from input json string and removes symbols
            json = json.Trim(new Char[] { '[', ']', '}', '{' });
            json = json.Replace('"', ' ').Trim();
            string[] comps = json.Split(',');

            for (int i = 0; i < comps.Length; i++)
            {
                // Removes input titles and keeps values
                comps[i] = comps[i].Substring(comps[i].IndexOf(':') + 1);
            }
            bool[] warn = new bool[comps.Length];

            // Converts from string to boolean
            for (int i = 1; i < comps.Length; i++)
            {
                //Debug.Log("Comps " + i + ": " + comps[i]);
                if (comps[i] == "true")
                {
                    warn[i] = true;
                }
                else warn[i] = false;
            }

            // Updates values in WarningInfo object
            warnings.sop_on = warn[1];
            warnings.sspe = warn[2];
            warnings.fan_error = warn[3];
            warnings.vent_error = warn[4];
            warnings.vehicle_power = warn[5];
            warnings.h2o_off = warn[6];
            warnings.o2_off = warn[7];
        }
    }
}