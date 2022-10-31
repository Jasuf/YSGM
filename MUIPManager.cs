﻿using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace YSGM
{
    public class MUIPManager
    {
        public static MUIPManager Instance = new();

        private MUIPManager() { }

        public string GM(string uid, string cmd)
        {
            return GET(uid, 1116, cmd);
        }

        public string FetchPlayerBin(string uid)
        {
            return GET(uid, 1004, "");
        }

        public string GET(string uid, int cmd, string? msg) // These both are numbers, but string for convenience
        {
#if DEBUG
            var builder = new UriBuilder("http://hk4e-storage.mihoyo.com:14311/api");
#else
            var builder = new UriBuilder(ConfigurationManager.AppSettings.Get("MUIP_HOST")!);
#endif
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["uid"] = uid;
            query["cmd"] = cmd.ToString();
            if (msg != null) query["msg"] = msg;
            query["region"] = ConfigurationManager.AppSettings.Get("MUIP_TARGET_REGION");
            query["sign"] = SHA($"{query.ToString()}1d8z98SAKF98bdf878skswa8kdjfy1m9dses");
            builder.Query = query.ToString();

            var client = new HttpClient();

            var webRequest = new HttpRequestMessage(HttpMethod.Get, builder.ToString());

            var response = client.Send(webRequest);

            using var reader = new StreamReader(response.Content.ReadAsStream());

            return reader.ReadToEnd();
        }

        private string SHA(string str)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(str));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}
