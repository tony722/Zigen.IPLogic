using System;

namespace AET.Zigen.IPLogic {
  public static class Utils {

    /// <summary> Takes a labelAndValue (e.g. "My Label: My Value") and returns value. If no colon, then returns entire value. </summary>
    /// <param name="labelAndValue"> A colon separated label:value string. </param>
    public static string ParseValue(string labelAndValue) {
      var values = labelAndValue.Split(':');
      if (values.Length > 1) return values[1].Trim();
      return labelAndValue;
    }

    public static bool Matches(this string value, string[] matchValues) {
      return Array.IndexOf(matchValues, value) >= 0;
    }
  }
}
