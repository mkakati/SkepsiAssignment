using CleanArchitecture.Application.Account.Models;
using CleanArchitecture.Common.ApiResponse;
using MediatR;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static CleanArchitecture.Common.Enums.ResponseEnums;

namespace CleanArchitecture.Application.Account.Queries.SunriseSunset{
    public class SunriseSunsetQueryHandler : IRequestHandler<SunriseSunsetQuery, object>
    {
        public async Task<object> Handle(SunriseSunsetQuery model, CancellationToken cancellationToken)
        {
            ApiResponse res = new ApiResponse();
            try
            {
                string baseURL = "https://api.sunrise-sunset.org/json?";

                float lat = model.Latitude;
                float lng = model.Longitude;

                string URL = string.Concat(baseURL, "lat=", lat, "&lng=", lng);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = URL.Length;
                using (Stream webStream = request.GetRequestStream())
                using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
                {
                    requestWriter.Write(URL);
                }

                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                using (StreamReader responseReader = new StreamReader(webStream))
                {
                    string r = responseReader.ReadToEnd();
                    
                    var details = JObject.Parse(r);
                   
                    string a = (string)details["results"]["sunrise"];
                    string b = (string)details["results"]["sunset"];

                    //DateTime sunr = Convert.ToDateTime(a).ToLocalTime();
                    //DateTime suns = Convert.ToDateTime(b).ToLocalTime();
                    //string sunr1 = sunr.ToString().Split(" ")[1];
                    //string suns1 = suns.ToString().Split(" ")[1];
                    
                    ResponseSunriseSunset newSunriseSunset = new ResponseSunriseSunset();
                    {
                        newSunriseSunset.Sunrise = a;
                        newSunriseSunset.Sunset = b;

                    }

                    res.Status = (int)Number.One;
                    res.StatusCode = ResponseCode.Ok;
                    res.Message = "Success";
                    res.ResponseData = newSunriseSunset;
                    res.Total = (int)Number.One;
                   

                }


            }
            catch (Exception ex)
            {

                res.Message = ex.Message;
            }
            return res;

        }
    }
}
