using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking; // web request

namespace suitInfo
{
    class SuitInformation : MonoBehaviour
    {
        const int NUM_DATA = 11;
        Dictionary<string, int> data = new Dictionary<string, int>(NUM_DATA);

        BarData oxygen = new BarData(0, 100, "oxygen");
        BarData battery_life = new BarData(0, 100, "battery life");
        BarData water = new BarData(0, 100, "water");
        //Simple simple_data;

        public void update_suitInformation()
        {
            // read in data using UnityWebRequest
            UnityWebRequest www = UnityWebRequest.Get("http://ip_of_the_server/api/telemetry/recent");
            UnityWebRequest warnings = UnityWebRequest.Get("warnings");
            //yeild return www.SendWebRequest();

            //if (www.isNetworkError || www.isHttpError)
            if(www.isHttpError) Debug.Log(www.error);
            else
            {
                // Show results as text
                //Debug.Log(www.downloadHandler.text);
                string json = www.downloadHandler.text;

                // Or retrieve results as binary data
                //byte[] results = www.downloadHandler.data;

                // retrieve as JSON file
                Movie m = JsonConvert.DeserializeObject<Movie>(json); // from JSON.Net

                // Bad Boys
                // store JSON information into dictionary
                data["p_sub"] = m.p_sub; // from JSON file
                data["t_sub"] = m.t_sub;
                data["v_fan"] = m.v_fan;
                data["t_eva"] = m.t_eva;
                data["p_o2"] = m.p_o2;
                data["rate_o2"] = m.rate_o2;
                data["cap_battery"] = m.cap_battery;
                data["p_h2o_g"] = m.p_h2o_g;
                data["p_h2o_l"] = m.p_h2o_l;
                data["p_sop"] = m.p_sop;
                data["rate_sop"] = m.rate_sop;

            }

            // update the display data
            oxygen.update_val(m.p_o2); // value is pressure in tank
            battery_life.update_val(m.cap_battery);
            water.update_val(m.p_h2o_g);

            // call functions to display data always 
            oxygen.Display(); // two more
            battery_life.Display();
            water.Display();
        }
    }
}
