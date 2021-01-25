using Crestron.SimplSharp;

namespace AET.Zigen.IPLogic {
  interface ILogger {
    void Error(string message);
    void Error(string message, params object[] args);

    void Notice(string message);
    void Notice(string message, params object[] args);

    void PrintLine(string message);
    void PrintLine(string message, params object[] args);

    void Warn(string message);
    void Warn(string message, params object[] args);
  }
  class CrestronLogger : ILogger {
    public void Error(string message) {
      ErrorLog.Error(message);
    }

    public void Error(string message, params object[] args) {
      ErrorLog.Error(message, args);
    }

    public void Notice(string message) {
      ErrorLog.Notice(message);
    }

    public void Notice(string message, params object[] args) {
      ErrorLog.Notice(message, args);
    }

    public void PrintLine(string message) {
      CrestronConsole.PrintLine(message);
    }

    public void PrintLine(string message, params object[] args) {
      CrestronConsole.PrintLine(message, args);
    }

    public void Warn(string message) {
      ErrorLog.Warn(message);
    }
    public void Warn(string message, params object[] args) {
      ErrorLog.Warn(message, args);
    }
  }
}
