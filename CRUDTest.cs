using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.WebTesting;

namespace azure_mobile_apps_node_perf
{

    public class CRUDTest : WebTest
    {

        public CRUDTest()
        {
            this.PreAuthenticate = true;
            this.Proxy = "default";
        }

        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            var id = Guid.NewGuid().ToString();
            var url = this.Context["URL"].ToString();

            yield return CreateRequest(url, "", "POST", 201, Body(id, "initial"));
            yield return CreateRequest(url, id, "PATCH", 200, Body(id, "updated"));
            yield return CreateRequest(url, id, "GET", 200);
            yield return CreateRequest(url, id, "DELETE", url.Contains("node") ? 200 : 204);
        }

        private WebTestRequest CreateRequest(string url, string id, string method, int expectedStatus, IHttpBody body = null)
        {
            var table = url.Contains("v1") ? "v1crud" : "crud";
            var request = new WebTestRequest(url + "tables/" + table + "/" + id)
            {
                Timeout = 5,
                Method = method,
                Encoding = Encoding.GetEncoding("utf-8"),
                ExpectedHttpStatusCode = expectedStatus,
                Body = body
            };
            request.Headers.Add("x-zumo-application", "EHRkCHqMqEhibaRxeSCyzXcmEUVThd11");
            return request;
        }

        private StringHttpBody Body(string id, string text)
        {
            return new StringHttpBody()
            {
                ContentType = "application/json",
                BodyString = "{\"id\":\"" + id + "\",\"text\":\"" + text + "\"}"
            };
        }
    }
}
