using System;

namespace AET.Zigen.IPLogic.HttpClient {
  interface IHttpClient {
    void Post(string url, string contents);

    void Post(string url, string contents, Action<string> response);
  }
}
