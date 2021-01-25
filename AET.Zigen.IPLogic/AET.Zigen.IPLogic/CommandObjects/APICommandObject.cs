using Newtonsoft.Json;

namespace AET.Zigen.IPLogic.CommandObjects {

  [JsonObject(MemberSerialization.OptIn)]
  public abstract class APICommandObject {
    private readonly string url;

    protected APICommandObject(string url) {
      this.url = url;
    }

    public string Url { get { return url; } }

    public virtual void Execute() {
      if(RequiredFieldsAreValid()) APIClient.ExecuteRequest(this);      
    }

    public virtual string GetJson() {
      var json = JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings {
        NullValueHandling = NullValueHandling.Ignore
      });
      return json;
    }

    protected abstract bool RequiredFieldsAreValid();

    protected bool FalseWithErrorMessage(string message) {
      Config.Logger.Error(message);
      return false;
    }

    protected bool FalseWithErrorMessage(string message, params object[] args) {
      Config.Logger.Error(message, args);
      return false;
    }
  }
}
