using AET.Zigen.IPLogic.CommandObjects;
using AET.Zigen.IPLogic.HttpClient;
using AET.Zigen.IPLogic.ResponseObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AET.Zigen.IPLogic {
  public static class CommsPoller {
    private static long lastEventId;
    private static readonly Dictionary<string, Action<string>> rxDelegates = new Dictionary<string, Action<string>>();
    private static List<string> addresses;

    static CommsPoller() {
      HttpClient = new CrestronHttpClient();
      Timer = new CrestronTimer();
      lastEventId = 0;
      Config.LoadComplete += () => {
        if (addresses == null || addresses.Count == 0) {
          StopPolling();
        } else {
          StartPolling();
        }
      };
    }

    internal static ITimer Timer { get; set; }
    public static void StartPolling() {
      Timer.TimerElapsed = PollAll;
      Timer.Start(Config.PollInterval);
    }

    public static void StopPolling() {
      Timer.Stop();
    }
    
    private static void Poll(GetCommsData request) {
      HttpClient.Post(APIClient.BuildFullUrl(request.Url), request.GetJson(), OnTransferResponse);
    }

    /// <summary> Registers a delegate to receive the rs232 data for a particular MAC Address </summary>
    public static void RegisterForAutomaticPolling(string macAddress, Action<string> commsRxDelegate) {
      if(rxDelegates.ContainsKey(macAddress)) {
        rxDelegates[macAddress] = commsRxDelegate;
      } else {
        rxDelegates.Add(macAddress, commsRxDelegate);
      }
      addresses = rxDelegates.Keys.ToList();
      if (lastEventId == 0) GetLastEventId();
    }

    internal static void GetLastEventId() {
      var request = new GetCommsData {
        FirstEventIdToRetrieve = 0,
        MacAddresses = addresses,
        MaxEventsToRetrieve = 1,
        Type = "rs232"
      };
      HttpClient.Post(APIClient.BuildFullUrl(request.Url), request.GetJson(), (response) => {
        var dataResponse = GetCommsDataResponse.Deserialize(response);
        if (dataResponse == null) return;
        var dataSet = dataResponse.Data;
        if (dataSet == null) return;
        var data = dataSet.LastOrDefault();
        if (data == null) return;
        lastEventId = data.EventId;
      });
    }

    internal static void OnTransferResponse(string response) {
      var dataResponse = GetCommsDataResponse.Deserialize(response);
      if (dataResponse == null) return;
      foreach (var data in dataResponse.Data) {
        Action<string> rx;
        if(rxDelegates.TryGetValue(data.Address, out rx)) {
          rx(data.Code);
        }
        lastEventId = data.EventId;
      }
    }

    internal static IHttpClient HttpClient { get; set; }
 
    public static void ManualPollMac(string address) {
      var request = new GetCommsData {
        FirstEventIdToRetrieve = lastEventId,
        MacAddresses = addresses,
        MaxEventsToRetrieve = 50,
        Type = "rs232"
      };
      Poll(request);
    }

    public static void PollAll() {
      if (addresses == null || addresses.Count == 0) return;
      var request = new GetCommsData {
        FirstEventIdToRetrieve = lastEventId,
        MacAddresses = addresses,
        MaxEventsToRetrieve = 50,
        Type = "rs232"
      };
      Poll(request);
    }
  }
}
