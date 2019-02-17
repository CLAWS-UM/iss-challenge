using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace suitInfo
{
    class SuitInformation
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
            //yeild return www.SendWebRequest();

            //if (www.isNetworkError || www.isHttpError)
            if(www.isHttpError) Debug.Log(www.error);
            else
            {
                // Show results as text
                Debug.Log(www.downloadHandler.text);

                // Or retrieve results as binary data
                byte[] results = www.downloadHandler.data;

                // retrieve as JSON file

                // store JSON information into dictionary
                data["p_sub"] = 3; // from JSON file

            
            }

            // call functions to display data
            oxygen.Display(); // two more
            battery_life.Display();
            water.Display();
        }
    }
}
