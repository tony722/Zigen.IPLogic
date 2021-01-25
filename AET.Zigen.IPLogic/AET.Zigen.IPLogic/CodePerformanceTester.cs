using Crestron.SimplSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp.CrestronData;

namespace AET.Zigen.IPLogic {
  public class CodePerformanceTester {    
    public CodePerformanceTester() {
      /*
      using (var con = new SQLiteConnection("Data Source =:memory:")) {
        con.Open();
        using (var cmd = new SQLiteCommand(con)) {
          cmd.CommandText = "DROP TABLE IF EXISTS devices";
          cmd.ExecuteNonQuery();

          cmd.CommandText = @"CREATE TABLE devices(name STRING, deviceType STRING, index INT, macAddress STRING";
          cmd.ExecuteNonQuery();

          cmd.CommandText = @"CREATE INDEX devicesName ON devices(name)";
          cmd.ExecuteNonQuery();

          cmd.CommandText = @"CREATE INDEX devicesIndex ON devices(index)";
          cmd.ExecuteNonQuery();

          cmd.CommandText = @"CREATE INDEX devicesDeviceType ON devices(deviceType)";
          cmd.ExecuteNonQuery();
        }
      }
      */
    }

    public void LookupsTest() {
      DictLookups();  //18ms, 290ms, 423ms
      DictLookups2(); //34ms, 908ms, 877ms
      DictLookups3(); //19ms, 159ms, 501ms
      DataTableLookup(); //151ms, 14327ms, 12823ms
      ListLookups(); //12ms, 1350ms, 73665ms
    }

    private void ListLookups() {
      var watch = new Stopwatch();
      watch.Start();
      var devices = new List<Device>();
      for (var i = 0; i < 200; i++) {
        devices.Add(new Device {
          Name = "Device: " + i,
          Index = i,
          MacAddress = "ABACADAEAF" + i
        });
      }
      watch.Stop();
      ErrorLog.Notice("200 List records created in {0} ms.", watch.ElapsedMilliseconds);
      watch.Reset();
      watch.Start();
      for (var i = 0; i < 10000; i++) {
        var Index = devices.FindIndex(d => d.Index == i % 300);
      }
      watch.Stop();
      ErrorLog.Notice("10,000 FindIndex Index = {0} ms.", watch.ElapsedMilliseconds);

      watch.Reset();
      watch.Start();
      for (var i = 0; i < 10000; i++) {
        var Index = devices.FindIndex(d => d.Name == "Device: " + i);
      }
      watch.Stop();
      ErrorLog.Notice("10,000 FindIndex Name = {0} ms.", watch.ElapsedMilliseconds);
    }

    private void DictLookups() {
      var watch = new Stopwatch();
      watch.Start();
      var byName = new Dictionary<string, Device>();
      var byIndex = new Dictionary<int, Device>();

      for (var i = 0; i < 200; i++) {
        var device = new Device {
          Name = "Device: " + i,
          Index = i,
          MacAddress = "ABACADAEAF" + i
        };
        byName.Add(device.Name, device);
        byIndex.Add(device.Index, device);
      }
      watch.Stop();
      ErrorLog.Notice("200 Dictionary x 2 records created in {0} ms.", watch.ElapsedMilliseconds);

      watch.Reset();
      watch.Start();
      for (var i = 0; i < 10000; i++) {
        Device device;
        var found = byIndex.TryGetValue(i % 300, out device);
      }
      watch.Stop();
      ErrorLog.Notice("10,000 DictLookup Index = {0} ms.", watch.ElapsedMilliseconds);

      watch.Reset();
      watch.Start();
      for (var i = 0; i < 10000; i++) {
        Device device;
        var found = byName.TryGetValue("Device: " + i, out device);
      }
      watch.Stop();
      ErrorLog.Notice("10,000 DictLookup Name = {0} ms.", watch.ElapsedMilliseconds);
    }


    private void DictLookups2() {
      var watch = new Stopwatch();
      watch.Start();
      var byName = new Dictionary<DeviceNameKey, Device>();
      var byIndex = new Dictionary<DeviceIndexKey, Device>();

      for (var i = 0; i < 200; i++) {
        var device = new Device {
          Name = "Device: " + i,
          Index = i,
          MacAddress = "ABACADAEAF" + i
        };
        byName.Add(new DeviceNameKey { Name = device.Name, DeviceType = "receiver" }, device);
        byIndex.Add(new DeviceIndexKey { Index = device.Index, DeviceType = "receiver" }, device);
      }
      watch.Stop();
      ErrorLog.Notice("200 Dict2 x 2 records created in {0} ms.", watch.ElapsedMilliseconds);

      watch.Reset();
      watch.Start();
      for (var i = 0; i < 10000; i++) {
        Device device;
        if(byIndex.TryGetValue(new DeviceIndexKey { Index = i % 300, DeviceType = "receiver" }, out device)) { var macAddress = device.MacAddress; };
      }
      watch.Stop();
      ErrorLog.Notice("10,000 Dict2Lookup Index = {0} ms.", watch.ElapsedMilliseconds);

      watch.Reset();
      watch.Start();
      for (var i = 0; i < 10000; i++) {
        Device device;
        if(byName.TryGetValue(new DeviceNameKey { Name = "Device: " + i, DeviceType = "receiver" }, out device)) { var macAddress = device.MacAddress; };
      }
      watch.Stop();
      ErrorLog.Notice("10,000 Dict2Lookup Name = {0} ms.", watch.ElapsedMilliseconds);
    }

    private void DictLookups3() {
      var watch = new Stopwatch();
      watch.Start();
      var byName = new Dictionary<string, Dictionary<string, Device>>();
      var byIndex = new Dictionary<string, Dictionary<int, Device>>();
      byName.Add("receiver", new Dictionary<string, Device>());
      byIndex.Add("receiver", new Dictionary<int, Device>());
      for (var i = 0; i < 200; i++) {
        var device = new Device {
          Name = "Device: " + i,
          Index = i,
          MacAddress = "ABACADAEAF" + i
        };
        byName["receiver"].Add(device.Name, device);
        byIndex["receiver"].Add(device.Index, device);
      }
      watch.Stop();
      ErrorLog.Notice("200 Dict3 x 2 records created in {0} ms.", watch.ElapsedMilliseconds);

      watch.Reset();
      watch.Start();
      for (var i = 0; i < 10000; i++) {
        Device device;
        if(byIndex["receiver"].TryGetValue(i % 300, out device)) { var macAddress = device.MacAddress; };
      }
      watch.Stop();
      ErrorLog.Notice("10,000 Dict3Lookup Index = {0} ms.", watch.ElapsedMilliseconds);

      watch.Reset();
      watch.Start();
      for (var i = 0; i < 10000; i++) {
        Device device;
        if(byName["receiver"].TryGetValue("Device: " + i, out device)) { var macAddress = device.MacAddress; }
      }
      watch.Stop();
      ErrorLog.Notice("10,000 Dict3Lookup Name = {0} ms.", watch.ElapsedMilliseconds);
    }


    /* Crestron SQLite does not support in memory databases 
    public void SQLiteLookup() {
      var watch = new Stopwatch();
      watch.Start();
      using (var con = new SQLiteConnection("Data Source =:memory:")) {
        con.Open();
        using (var cmd = new SQLiteCommand(con)) {
          for (var i = 0; i < 200; i++) {
            cmd.CommandText = "INSERT INTO devices(deviceType, name, index, macAddress) VALUES (@deviceType, @name, @index, @macAddress";
            cmd.Parameters.AddWithValue("@deviceType", "receiver");
            cmd.Parameters.AddWithValue("@name", "Device: " + i);
            cmd.Parameters.AddWithValue("@index", i);
            cmd.Parameters.AddWithValue("@macAddress", "ABACADAEAF" + i);
            cmd.ExecuteNonQuery();
          }
        }
      }
      watch.Stop();
      ErrorLog.Notice("200 SQLite records created in {0} ms.", watch.ElapsedMilliseconds);

      watch.Reset();
      watch.Start();
      for (var i = 0; i < 10000; i++) {
        using (var con = new SQLiteConnection("Data Source =:memory:")) {
          con.Open();
          using (var cmd = new SQLiteCommand(con)) {
            cmd.CommandText = "SELECT * FROM devices WHERE deviceType = 'receiver' AND index = @index";
            cmd.Parameters.AddWithValue("@index", i % 300);
            using (var reader = cmd.ExecuteReader()) {
              if (reader.HasRows) {
                var macAddress = (string)reader["macAddress"];
              }
            }           
          }
        }
      }
      watch.Stop();
      ErrorLog.Notice("10,000 Sqlite Index = {0} ms.", watch.ElapsedMilliseconds);

      watch.Reset();
      watch.Start();
      for (var i = 0; i < 10000; i++) {
        using (var con = new SQLiteConnection("Data Source =:memory:")) {
          con.Open();
          using (var cmd = new SQLiteCommand(con)) {
            cmd.CommandText = "SELECT * FROM devices WHERE deviceType = 'receiver' AND name = @name";
            cmd.Parameters.AddWithValue("@name", "Device: " + i);
            using (var reader = cmd.ExecuteReader()) {
              if (reader.HasRows) {
                var macAddress = (string)reader["macAddress"];
              }
            }
          }
        }
      }
      watch.Stop();
      ErrorLog.Notice("10,000 Sqlite Name = {0} ms.", watch.ElapsedMilliseconds);
    }
    */

    public void DataTableLookup() {
      var watch = new Stopwatch();
      watch.Start();
      var devices = new DataTable("devices");
      devices.Columns.Add("Name", typeof(string));
      devices.Columns.Add("Index", typeof(int));
      devices.Columns.Add("DeviceType", typeof(string));
      devices.Columns.Add("MacAddress", typeof(string));
      devices.PrimaryKey = new DataColumn[] { devices.Columns["Name"], devices.Columns["Index"], devices.Columns["DeviceType"] };
      
      for (var i = 0; i < 200; i++) {
        devices.Rows.Add("Device: " + i, i, "receiver", "ABACADAEAF" + i);
      }
      watch.Stop();
      ErrorLog.Notice("200 List records created in {0} ms.", watch.ElapsedMilliseconds);
      watch.Reset();

      watch.Start();
      for (var i = 0; i < 10000; i++) {
        var result = devices.Select(string.Format("DeviceType = 'receiver' AND Index = {0}", i % 300));
        if (result.Length > 0) { var macAddress = result[0]["MacAddress"]; }
      }
      watch.Stop();
      ErrorLog.Notice("10,000 DataTable Index = {0} ms.", watch.ElapsedMilliseconds);

      watch.Reset();
      watch.Start();
      for (var i = 0; i < 10000; i++) {
        var result = devices.Select(string.Format("DeviceType = 'receiver' AND Name = '{0}'", "Device: " + i));
        if (result.Length > 0) { var macAddress = result[0]["MacAddress"]; }
      }
      watch.Stop();
      ErrorLog.Notice("10,000 DataTable Name = {0} ms.", watch.ElapsedMilliseconds);
    }
  }

  

  


  internal struct DeviceIndexKey {
    public int Index { get; set; }
    public string DeviceType { get; set; }
  }
  internal struct DeviceNameKey {
    public string Name { get; set; }
    public string DeviceType { get; set; }
  }
}
