using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DroneControl {
    public class DroneStatusConnection {
        private static volatile DroneStatusConnection _instance;
        private static object syncRoot = new Object();
        private int _port { get; set; }
        public static DroneStatusConnection Instance
        {
            get
            {
                if (_instance == null) {
                    lock (syncRoot) {
                        if (_instance == null)
                            _instance = new DroneStatusConnection();
                    }
                }
                return _instance;
            }
        }

        private DroneStatusConnection() {
            _port = 62553;
        }

        private int getQualityCheckID() {
            var request = (HttpWebRequest)WebRequest.Create("http://localhost:" + _port + "/api/QualityCheck/");

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return Convert.ToInt16(responseString);
        }

        public void UpdateStatus(string status) {
            var request = (HttpWebRequest)WebRequest.Create("http://localhost:" + _port + "/api/QualityCheck/" + getQualityCheckID());
            
            var data = Encoding.ASCII.GetBytes(status);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream()) {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
    }
}
