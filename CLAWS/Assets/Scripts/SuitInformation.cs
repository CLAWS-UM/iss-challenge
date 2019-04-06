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
        public GameObject bat_life;
        public GameObject wat;
        public GameObject subp;
        public GameObject subt;
        // circle icons which change color
        public GameObject bpm_circle;
        public GameObject oxy_circle;
        public GameObject bat_life_circle;
        public GameObject wat_circle;
        public GameObject subp_circle;
        public GameObject subt_circle;
        // class objects
        BarData heartbpm;
        BarData sub_press;
        BarData sub_temp;
        BarData oxygen;
        BarData battery_life;
        BarData water;

        public GameObject textObject;

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


        private void Start()
        {

            // update the visual scale
            transform.localScale = new Vector3(8, 3, 0);

            // five circles of data
            heartbpm = bpm.AddComponent<BarData>();
            oxygen = oxy.AddComponent<BarData>(); //(0, 100, "oxygen");
            battery_life = bat_life.AddComponent<BarData>();
            water = wat.AddComponent<BarData>();//new BarData(0, 100, "water");
            sub_press = subp.AddComponent<BarData>();
            sub_temp = subt.AddComponent<BarData>();
            /*heartbpm_circle = bpm_circle.AddComponent<WarningColors>();
            oxygen_circle = oxy_circle.AddComponent<WarningColors>(); //(0, 100, "oxygen");
            battery_life_circle = bat_life_circle.AddComponent<WarningColors>();
            water_circle = wat_circle.AddComponent<WarningColors>();//new BarData(0, 100, "water");
            sub_press_circle = subp_circle.AddComponent<WarningColors>();
            sub_temp_circle = subt_circle.AddComponent<WarningColors>(); */

            heartbpm.initialize(0, "heartbpm");
            oxygen.initialize(0, "oxygen");
            battery_life.initialize(0, "battery life");
            water.initialize(0, "water");
            sub_press.initialize(0, "sub pressure");
            sub_temp.initialize(0, "sub temperature");

            heartbpm.startDisplay(bpm);
            oxygen.startDisplay(oxy);
            battery_life.startDisplay(bat_life);
            water.startDisplay(wat);
            sub_press.startDisplay(subp);
            sub_temp.startDisplay(subt);

            heartbpm.DisplayCircle(bpm_circle);
            oxygen.DisplayCircle(oxy_circle);
            battery_life.DisplayCircle(bat_life_circle);
            water.DisplayCircle(wat_circle);
            sub_press.DisplayCircle(subp_circle);
            sub_temp.DisplayCircle(subt_circle);

            //Debug.Log(telemetry_data.p_o2);
            bpm.GetComponent<TextMesh>().text = telemetry_data.heart_bpm.ToString();
            oxy.GetComponent<TextMesh>().text = telemetry_data.p_o2.ToString();
            bat_life.GetComponent<TextMesh>().text = telemetry_data.cap_battery.ToString();
            wat.GetComponent<TextMesh>().text = telemetry_data.p_h2o_g.ToString();
            subp.GetComponent<TextMesh>().text = telemetry_data.cap_battery.ToString();
            wat.GetComponent<TextMesh>().text = telemetry_data.p_h2o_g.ToString();
        }

        void Update()
        {

            //Vector3 pos = new Vector3(-(GetComponent<Renderer>().bounds.size.x / 2), (GetComponent<Renderer>().bounds.size.y / 2), 0);
            //transform.position = pos;

            update_suitInformation();
        }

        public void update_suitInformation()
        {
            string suitURL = "http://localhost:3000/api/suit/recent";
            string warningURL = "http://localhost:3000/api/suitswitch/recent";
            StartCoroutine(GetText(suitURL));
            StartCoroutine(GetText(warningURL));

            // update the display data
            textObject.GetComponent<TextMesh>().text = telemetry_data.p_o2.ToString();

            oxygen.change_val(Convert.ToDouble(telemetry_data.p_o2)); // value is pressure in tank
            battery_life.change_val(Convert.ToDouble(telemetry_data.cap_battery));
            water.change_val(Convert.ToDouble(telemetry_data.p_h2o_g));
            sub_press.change_val(Convert.ToDouble(telemetry_data.p_sub));
            sub_temp.change_val(Convert.ToDouble(telemetry_data.t_sub));

            // call functions to display data always 
            oxygen.Display(oxy);
            battery_life.Display(bat_life);
            water.Display(wat);
            sub_press.Display(subp);
            sub_temp.Display(subt);

            // update warning colors
            heartbpm.DisplayCircle(bpm_circle);
            oxygen.DisplayCircle(oxy_circle);
            battery_life.DisplayCircle(bat_life_circle);
            water.DisplayCircle(wat_circle);
            sub_press.DisplayCircle(subp_circle);
            sub_temp.DisplayCircle(subt_circle);
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

                string json = www.downloadHandler.text;
                Debug.Log(json);
                if (url == "http://localhost:3000/api/suit/recent")
                {
                    Update_Suit_Info(json);
                }
                else
                {
                    Update_Warnings(json);
                }
            }
        }
        //[{"create_date":"2019-04-05T02:27:13.091Z","heart_bpm":"85","p_sub":"7.95","t_sub":"6","v_fan":"39989","p_o2":"15","rate_o2":"1.0","cap_battery":"30","p_h2o_g":"15","p_h2o_l":"16","p_sop":"887","rate_sop":"0.9","t_battery":"-5:-9:-28","t_oxygen":"8:35:9","t_water":"8:35:9"}]
        void Update_Suit_Info(string json)
        {
            json = json.Trim(new Char[] { '[', ']' });
            json = json.Replace('"', ' ').Trim();
            string[] comps = json.Split(',');
            for (int i = 0; i < comps.Length; i++)
            {
                comps[i] = comps[i].Substring(comps[i].IndexOf(':') + 1);
            }
            Debug.Log(comps);
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
            json = json.Trim(new Char[] { '[', ']' });
            json = json.Replace('"', ' ').Trim();
            string[] comps = json.Split(',');
            for (int i = 0; i < comps.Length; i++)
            {
                comps[i] = comps[i].Substring(comps[i].IndexOf(':') + 1);
            }
            Debug.Log(comps);
            bool[] warn = new bool[comps.Length];

            for (int i = 0; i < comps.Length; i++)
            {
                if (comps[i] == "true")
                {
                    warn[i] = true;
                }
                else warn[i] = false;
            }
            warnings.create_date = comps[0];
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