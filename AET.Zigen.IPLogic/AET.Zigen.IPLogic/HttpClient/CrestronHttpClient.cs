using Crestron.SimplSharp.Net.Http;
using System;

namespace AET.Zigen.IPLogic.HttpClient {
  class CrestronHttpClient : IHttpClient {

    public void Post(string url, string contents) {
      Post(url, contents, (responseString) => { }); //CrestronConsole.PrintLine("IPLogic Post Results: {0}", responseString));
    }

    public void Post(string url, string contents, Action<string> responseCallback) {
      //CrestronConsole.PrintLine("IPLogic HttpRequest: {0} | {1}", url, contents);
      var request = BuildRequest(url, contents);
      var httpClient = new Crestron.SimplSharp.Net.Http.HttpClient();  //Using(){ } doesn't work. It disposes of the client before it sends the request!
      try {        
        var error = httpClient.DispatchAsync(request, 
          (response, err) => OnTransferResponse(response, err, responseCallback)   //HttpClientResponseCallback Delegate as a lambda to pickup Action<string> responseCallback
        );
        if (error != Crestron.SimplSharp.Net.Http.HttpClient.DISPATCHASYNC_ERROR.PENDING) {
          Config.Logger.Error(error.ToString());
        }
      } catch (Exception ex) {
        Config.Logger.Error("IPLogic API Error: {0}", ex.Message);
      }
    }

    private void OnTransferResponse(HttpClientResponse response, HTTP_CALLBACK_ERROR err, Action<string> responseCallback) {
        if (err != HTTP_CALLBACK_ERROR.COMPLETED || response == null) {
          Config.Logger.Warn("IPLogic Post: No reply or error.");
          return;
        }        
        responseCallback(response.ContentString);
    }

    private HttpClientRequest BuildRequest(string url, string contents) {
      return new HttpClientRequest {
        RequestType = RequestType.Post,
        ContentString = contents,
        Url = { Url = url },
        KeepAlive = false,
        Header = { ContentType = "application/json" }
      };
    }
  }
}
