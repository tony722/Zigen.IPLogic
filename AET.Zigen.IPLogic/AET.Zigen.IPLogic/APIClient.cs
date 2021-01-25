using AET.Zigen.IPLogic.CommandObjects;
using AET.Zigen.IPLogic.HttpClient;

namespace AET.Zigen.IPLogic {
  public static class APIClient {
    static APIClient() {
      HttpClient = new CrestronHttpClient();
    }

    internal static IHttpClient HttpClient { get; set; }

    private static string hostName;
    public static string HostName {
      get { return hostName; }
      set { hostName = value.TrimEnd('/'); }
    }

    internal static void ExecuteRequest(APICommandObject command) {
      HttpClient.Post(BuildFullUrl(command.Url), command.GetJson());
    }

    internal static string BuildFullUrl(string url) {
      return string.Format("{0}{1}", HostName, url);
    }
  }
}