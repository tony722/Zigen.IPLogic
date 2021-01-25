using AET.Zigen.IPLogic.HttpClient;
using Crestron.SimplSharp.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AET.Zigen.IPLogic.Tests {
  class TestHttpClient : IHttpClient {
    public static string RequestContents { get; set; }
    public static string ResponseContents { get; set; }
    public static string Url { get; set; }

    public void Post(string url, string contents) {
      Url = url;
      RequestContents = contents;
    }

    public void Post(string url, string contents, Action<string> response) {
      Post(url, contents);
      response(ResponseContents);
    }
  }
}
