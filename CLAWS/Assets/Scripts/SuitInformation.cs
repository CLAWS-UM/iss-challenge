using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine.Networking; // web request

namespace suitInfo
{

    class SuitInformation : MonoBehaviour
    {
        // textt
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

        // circle icons which change color
        public GameObject bpm_circle;
        public GameObject oxy_circle;
        public GameObject rate_oxy_circle;
        public GameObject bat_life_circle;
        public GameObject wat_circle;
        public GameObject subp_circle;
        public GameObject subt_circle;

        public GameObject warningPicture;
        public GameObject warningsentence;

        // class objects
        public BarData heartbpm;
        public BarData sub_press;
        public BarData sub_temp;
        public BarData oxygen;
        public BarData rate_oxygen;
        public BarData battery_life;
        public BarData water;

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
        public Text warningText;

        public int TaskFontSize = 15;
        public string suitURL = "http://192.168.43.148:3000/api/suit/recent";
        public string warningURL = "http://192.168.43.148:3000/api/suitswitch/recent";

        // public GameObject textObject;

        public SuitInfo telemetry_data;
        public WarningInfo warnings;
               
        public class SuitInfo
        {
            public string create_date { get; set; }
            public string heart_bpm { get; set; }
            public string p_sub { get; set; }
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

            public SuitInfo()
            {
                this.create_date = "0";
                this.heart_bpm = "0";
                this.p_sub = "0";
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
        };

        //,"fan_error":false,"vent_error":false,"vehicle_power":false,"h2o_off":false,"o2_off":false}
        public class WarningInfo
        {
            public string create_date { get; set; }
            public bool sop_on { get; set; }
            public bool sspe { get; set; }
            public bool fan_error { get; set; }
            public bool vent_error { get; set; }
            public bool vehicle_power { get; set; }
            public bool h2o_off { get; set; }
            public bool o2_off { get; set; }

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
        };

        public class BarData : MonoBehaviour
        {
            public enum WarningLabel
            {
                Red, Green
            }

            public double value;
            double start;
            float percentage;
            string title;
            WarningLabel color;
            int natavg_up = 100;
            int natavg_down = 30;
            GameObject titleObject;
            GameObject valueObject;

            public BarData()
            {
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
                if (title == "heartbpm")
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

            public void Display(TextMesh mesh)
            {
                // add code to display each bar

                mesh.text = value.ToString();
                mesh.fontSize = 25;

            }
            public void DisplayCircle(GameObject obj)
            {
                SpriteRenderer m_SpriteRenderer;
                m_SpriteRenderer = obj.GetComponent<SpriteRenderer>();
                // add code to display each bar
                if (title != "heartbpm")
                {
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
            }
            public void initialize(double val, string titl)
            {
                start = val; value = val;
                percentage = (float)value / (float)start;
                title = titl;
                Update_color(percentage);
            }
        };


        void Start()
        {
            telemetry_data = new SuitInfo();
            warnings = new WarningInfo();


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

            bpmText.fontSize = TaskFontSize;
            oxyText.fontSize = TaskFontSize;
            rate_oxyText.fontSize = TaskFontSize;
            bat_lifeText.fontSize = TaskFontSize;
            watText.fontSize = TaskFontSize;
            subpText.fontSize = TaskFontSize;
            subtText.fontSize = TaskFontSize;
            time_batText.fontSize = TaskFontSize;
            time_watText.fontSize = TaskFontSize;
            time_oxyText.fontSize = TaskFontSize;

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
            //transform.localScale = new Vector3(8, 3, 0);

            // five circles of data
            heartbpm = bpm.AddComponent<BarData>();
            oxygen = oxy.AddComponent<BarData>(); //(0, 100, "oxygen");
            battery_life = bat_life.AddComponent<BarData>();
            water = wat.AddComponent<BarData>();//new BarData(0, 100, "water");
            sub_press = subp.AddComponent<BarData>();
            sub_temp = subt.AddComponent<BarData>();
            rate_oxygen = rate_oxy.AddComponent<BarData>();


            heartbpm.initialize(0, "heartbpm");
            oxygen.initialize(0, "oxygen");
            rate_oxygen.initialize(0, "rate oxygen");
            battery_life.initialize(0, "battery life");
            water.initialize(0, "water");
            sub_press.initialize(0, "sub pressure");
            sub_temp.initialize(0, "sub temperature");

            //Debug.Log(telemetry_data.p_o2);
           
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

            heartbpm.DisplayCircle(bpm_circle);
            oxygen.DisplayCircle(oxy_circle);
            rate_oxygen.DisplayCircle(rate_oxy_circle);
            battery_life.DisplayCircle(bat_life_circle);
            water.DisplayCircle(wat_circle);
            sub_press.DisplayCircle(subp_circle);
            sub_temp.DisplayCircle(subt_circle);
        }

        void Update()
        {

            //Vector3 pos = new Vector3(-(GetComponent<Renderer>().bounds.size.x / 2), (GetComponent<Renderer>().bounds.size.y / 2), 0);
            //transform.position = pos;

  
            StartCoroutine(GetText(suitURL));
            StartCoroutine(GetText(warningURL));

            // update the display data
            // textObject.GetComponent<TextMesh>().text = telemetry_data.p_o2.ToString();

            heartbpm.change_val(Convert.ToDouble(telemetry_data.heart_bpm));
            oxygen.change_val(Convert.ToDouble(telemetry_data.p_o2)); // value is pressure in tank
            rate_oxygen.change_val(Convert.ToDouble(telemetry_data.rate_o2));
            battery_life.change_val(Convert.ToDouble(telemetry_data.cap_battery));
            water.change_val(Convert.ToDouble(telemetry_data.p_h2o_g));
            sub_press.change_val(Convert.ToDouble(telemetry_data.p_sub));
            sub_temp.change_val(Convert.ToDouble(telemetry_data.t_sub));

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
            // call functions to display data always
            bpmText.text = heartbpm.value.ToString();
            oxyText.text = oxygen.value.ToString();
            rate_oxyText = rate_oxy.GetComponent<TextMesh>();
            bat_lifeText.text = battery_life.value.ToString();
            watText.text = water.value.ToString();
            subpText.text = sub_press.value.ToString();
            subtText.text = sub_temp.value.ToString();
            time_batText.text = telemetry_data.t_battery.ToString();
            time_watText.text = telemetry_data.t_water.ToString();
            time_oxyText.text = telemetry_data.t_oxygen.ToString();

            // update warning colors
            heartbpm.DisplayCircle(bpm_circle);
            oxygen.DisplayCircle(oxy_circle);
            rate_oxygen.DisplayCircle(rate_oxy_circle);
            battery_life.DisplayCircle(bat_life_circle);
            water.DisplayCircle(wat_circle);
            sub_press.DisplayCircle(subp_circle);
            sub_temp.DisplayCircle(subt_circle);

            if (warnings.sop_on)
            {
                warningPicture.SetActive(true);
                warningText.text = "Secondary Oxygen Pack is active";
            } else if(warnings.sspe)
            {
                warningPicture.SetActive(true);
                warningText.text = "Spacesuit pressure";
            }
            else if (warnings.fan_error)
            {
                warningPicture.SetActive(true);
                warningText.text = "Cooling fan of the spacesuit has a failure";
            }
            else if (warnings.vent_error)
            {
                warningPicture.SetActive(true);
                warningText.text = "No ventilation flow is detected";
            }
            else if (warnings.vehicle_power)
            {
                warningPicture.SetActive(true);
                warningText.text = "Spacesuit is receiving power through spacecraft";
            }
            else if (warnings.h2o_off)
            {
                warningPicture.SetActive(true);
                warningText.text = "H2O system is offline";
            }
            else if (warnings.o2_off)
            {
                warningPicture.SetActive(true);
                warningText.text = "O2 system is offline";
            }
            else {
                warningPicture.SetActive(false);
                warningText.text = "";
            }

        }
        
        
        // telemetry
        IEnumerator GetText(string url)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("Error:");
                Debug.Log(www.error);
            }
            else
            {
                // Show results as text
                Debug.Log("Downloaded:");
                Debug.Log(www.downloadHandler.text);

                string json = "";
                json = www.downloadHandler.text;
                Debug.Log(json);
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
        //[{"create_date":"2019-04-05T02:27:13.091Z","heart_bpm":"85","p_sub":"7.95","t_sub":"6","v_fan":"39989","p_o2":"15","rate_o2":"1.0","cap_battery":"30","p_h2o_g":"15","p_h2o_l":"16","p_sop":"887","rate_sop":"0.9","t_battery":"-5:-9:-28","t_oxygen":"8:35:9","t_water":"8:35:9"}]
        void Update_Suit_Info(string json)
        {
            json = json.Trim(new Char[] { '[', ']', '}', '{' });
            json = json.Replace('"', ' ').Trim();
            string[] comps = json.Split(',');
            for (int i = 0; i < comps.Length; i++)
            {
                comps[i] = comps[i].Substring(comps[i].IndexOf(':') + 1);
                comps[i] = comps[i].Replace('.', ',').Trim();
                comps[i] = comps[i].Trim(' ');
                Debug.Log(comps[i]);
            }
            telemetry_data.create_date = comps[0];
            telemetry_data.heart_bpm = comps[1];
            telemetry_data.p_sub = comps[2];
            telemetry_data.t_sub = comps[3];
            telemetry_data.v_fan = comps[4];
            telemetry_data.p_o2 = comps[5];
            telemetry_data.rate_o2 = comps[6];
            telemetry_data.cap_battery = comps[7];
            telemetry_data.p_h2o_g = comps[8];
            telemetry_data.p_h2o_l = comps[9];
            telemetry_data.p_sop = comps[10];
            telemetry_data.rate_sop = comps[11];
            telemetry_data.t_battery = comps[12];
            telemetry_data.t_oxygen = comps[13];
            telemetry_data.t_water = comps[14];

        }
        //[{"create_date":"2019-04-05T02:52:31.307Z","sop_on":false,"sspe":false,"fan_error":false,"vent_error":false,"vehicle_power":false,"h2o_off":false,"o2_off":false}]
        void Update_Warnings(string json)
        {
            json = json.Trim(new Char[] { '[', ']', '}', '{' });
            json = json.Replace('"', ' ').Trim();
            string[] comps = json.Split(',');
            for (int i = 0; i < comps.Length; i++)
            {
                comps[i] = comps[i].Substring(comps[i].IndexOf(':') + 1);
            }
            //Debug.Log(comps);
            bool[] warn = new bool[comps.Length];

            for (int i = 1; i < comps.Length; i++)
            {
                Debug.Log("Comps " + i + ": " + comps[i]);
                if (comps[i] == "true")
                {
                    warn[i] = true;
                }
                else warn[i] = false;
            }
            //warnings.create_date = comps[0];
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