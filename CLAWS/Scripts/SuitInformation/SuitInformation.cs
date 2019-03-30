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

        public GameObject oxy;
        public GameObject bat_life;
        public GameObject wat;
        BarData oxygen;// = oxy.AddComponent <BarData> (); //(0, 100, "oxygen");
        BarData battery_life;// = bat_life.AddComponent<BarData>();
        BarData water;// = wat.AddComponent<BarData>();//new BarData(0, 100, "water");

        public GameObject textObject;
        SuitInfo telemetry_data = new SuitInfo();


        public class SuitInfo
        {
            public string _id { get; set; }
            public double p_sub { get; set; }
            public double t_sub { get; set; }
            public double v_fan { get; set; }
            public double t_eva { get; set; }
            public double p_o2 { get; set; }
            public double rate_o2 { get; set; }
            public double cap_battery { get; set; }
            public double p_h2o_g { get; set; }
            public double p_h2o_l { get; set; }
            public double p_sop { get; set; }
            public double rate_sop { get; set; } //{ get; set; }

            public SuitInfo(){
                this._id = "0";
                this.p_sub = 0;
                this.t_sub = 0;
                this.v_fan = 0;
                this.t_eva = 0;
                this.p_o2 = 0;
                this.rate_o2 = 0;
                this.cap_battery = 0;
                this.p_h2o_g = 0;
                this.p_h2o_l = 0;
                this.p_sop = 0;
                this.rate_sop = 0;
            }
        };
        //public TextAsset ta;

        private void Start(){

            // update the visual scale
            transform.localScale = new Vector3(8,3,0);

            oxygen = oxy.AddComponent<BarData>(); //(0, 100, "oxygen");
            battery_life = bat_life.AddComponent<BarData>();
            water = wat.AddComponent<BarData>();//new BarData(0, 100, "water");

            oxygen.initialize(0, "oxygen");
            battery_life.initialize(0, "battery life");
            water.initialize(0, "water");

            oxygen.startDisplay(oxy);
            battery_life.startDisplay(bat_life);
            water.startDisplay(wat);
            Debug.Log(telemetry_data.p_o2);
            textObject.GetComponent<TextMesh>().text = telemetry_data.p_o2.ToString();
            oxy.GetComponent<TextMesh>().text = telemetry_data.p_o2.ToString();
            bat_life.GetComponent<TextMesh>().text = telemetry_data.cap_battery.ToString();
            wat.GetComponent<TextMesh>().text = telemetry_data.p_h2o_g.ToString();
        }

        void Update(){
            
            //Vector3 pos = new Vector3(-(GetComponent<Renderer>().bounds.size.x / 2), (GetComponent<Renderer>().bounds.size.y / 2), 0);
            //transform.position = pos;

            update_suitInformation();
        }

        public void update_suitInformation()
        {
            //SuitInfo telemetry_data = new SuitInfo();
            // read in data using UnityWebRequest
            UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/api/telemetry/recent");
            //UnityWebRequest warnings = UnityWebRequest.Get("http://localhost:3000/api/suitswitch/recent");
            //yeild return www.SendWebRequest();

            //if (www.isNetworkError || www.isHttpError)
            if (www.isHttpError) Debug.Log(www.error);
            else
            {
                // Show results as text
                Debug.Log(www.downloadHandler.text);
                string json = www.downloadHandler.text;

                // retrieve as JSON file
                //Movie m = JsonConvert.DeserializeObject<Movie>(json); // from JSON.Net
                //telemetry_data = JsonConvert.DeserializeObject<SuitInfo>(json);
                //Debug.Log(json);
                //JsonConvert.PopulateObject(json, telemetry_data);
                
                telemetry_data.p_o2 = 9.0;
                Debug.Log(telemetry_data.p_o2);
                textObject.GetComponent<TextMesh>().text = telemetry_data.p_o2.ToString();
                

            }

            // update the display data
            oxygen.change_val(telemetry_data.p_o2); // value is pressure in tank
            battery_life.change_val(telemetry_data.cap_battery);
            water.change_val(telemetry_data.p_h2o_g);

            // call functions to display data always 
            oxygen.Display(oxy);
            battery_life.Display(bat_life);
            water.Display(wat);
        }
    }

}