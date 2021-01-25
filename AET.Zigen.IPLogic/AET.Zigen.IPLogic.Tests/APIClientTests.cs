using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AET.Zigen.IPLogic.Tests {
  [TestClass]
  public class APIClientTests {
    [TestMethod]
    public void HostNameSet_ValueEndsWithSlashes_SlashesRemoved() {
      APIClient.HostName = "http://test//";
      APIClient.HostName.Should().Be("http://test");
    }

    [TestMethod]
    public void HostNameSet_ValueDoesNotEndWithSlashes_ValueUnchanged() {
      APIClient.HostName = "http://test";
      APIClient.HostName.Should().Be("http://test");
    }
  }
}
