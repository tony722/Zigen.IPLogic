namespace AET.Zigen.IPLogic {
  public class LoadCompleteRunner {
    public delegate void LoadCompleteDelegate();
    public LoadCompleteRunner() {
      Config.LoadComplete += () => LoadComplete();
    }

    public LoadCompleteDelegate LoadComplete { get; set; }
  }
}
