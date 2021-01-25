using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AET.Zigen.IPLogic.Tests {
  class TestLogger : ILogger {
    public static string ErrorMessage { get; set; }
    public static string NoticeMessage { get; set; }
    public static string PrintMessage { get; set; }
    public static string WarningMessage { get; set; }
    
    private void ClearMessages() {
      ErrorMessage = null;
      NoticeMessage = null;
      PrintMessage = null;
      WarningMessage = null;
    }

    public void Error(string message) {
      ClearMessages();
      ErrorMessage = message;
    }

    public void Error(string message, params object[] args) {
      ClearMessages();
      ErrorMessage = string.Format(message, args);
    }

    public void Notice(string message) {
      ClearMessages();
      NoticeMessage = message;
    }

    public void Notice(string message, params object[] args) {
      ClearMessages();
      NoticeMessage = string.Format(message, args);
    }

    public void PrintLine(string message) {
      ClearMessages();
      PrintMessage = message;
    }

    public void PrintLine(string message, params object[] args) {
      ClearMessages();
      PrintMessage = string.Format(message, args);
    }

    public void Warn(string message) {
      ClearMessages();
      WarningMessage = message;
    }

    public void Warn(string message, params object[] args) {
      ClearMessages();
      WarningMessage = string.Format(message, args); 
    }
  }
}
