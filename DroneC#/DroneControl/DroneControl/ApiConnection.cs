using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

/**
 * @author: Gerhard Kroes
 * */
namespace DroneControl {
    /// <summary>
    /// Connection class with the api
    /// </summary>
    public class ApiConnection {
        private static volatile ApiConnection _instance;
        private static object syncRoot = new Object();
        private int _port { get; set; }
        public static ApiConnection Instance
        {
            get
            {
                if (_instance == null) {
                    lock (syncRoot) {
                        if (_instance == null)
                            _instance = new ApiConnection();
                    }
                }
                return _instance;
            }
        }

        private ApiConnection() {
            _port = 62553;
        }

        /// <summary>
        /// Get the lates qualitycheck id
        /// </summary>
        /// <returns>ID</returns>
        private int getQualityCheckID() {
            var request = (HttpWebRequest)WebRequest.Create("http://localhost:" + _port + "/api/QualityCheck/GetQualityCheckID");

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            int id = int.Parse(responseString.Substring(1, responseString.Length - 2));
            return id;
        }

        /// <summary>
        /// Sends a status update to the active qualitycheck in the api
        /// </summary>
        /// <param name="status"></param>
        public void UpdateQualityCheck(string status) {
            var request = (HttpWebRequest)WebRequest.Create("http://localhost:" + _port + "/api/QualityCheck/PutQualityCheck/" + getQualityCheckID());

            var postData = "id=" + getQualityCheckID();
            postData += "&Status=" + status;
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "PUT";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream()) {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

        /// <summary>
        /// Sends a image path to a qualitycheck in the api
        /// </summary>
        /// <param name="id"></param>
        /// <param name="imagePath"></param>
        public void UpdateQualityCheckImagePath(int id, string imagePath) {
            var request = (HttpWebRequest)WebRequest.Create("http://localhost:" + _port + "/api/QualityCheck/PutQualityCheck/" + id);

            var postData = "id=" + id;
            postData += "&PictureFolderUrl=" + imagePath;
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "PUT";
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
