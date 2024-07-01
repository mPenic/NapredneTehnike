using BankovniSustavApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

public class LogoviDataAccess
{
    private string _xmlFilePath;
    private string _jsonFilePath;

    public LogoviDataAccess(string xmlFilePath, string jsonFilePath)
    {
        _xmlFilePath = xmlFilePath;
        _jsonFilePath = jsonFilePath;
    }

    public void AddTransactionLog(Logovi log)
    {
        List<Logovi> data = ReadJsonData();
        data.Add(log);
        WriteJsonData(data);
    }

    public void AddAccountLog(Logovi log)
    {
        List<Logovi> data = ReadXmlData();
        data.Add(log);
        WriteXmlData(data);
    }

    private List<Logovi> ReadXmlData()
    {
        if (!File.Exists(_xmlFilePath))
        {
            return new List<Logovi>();
        }
        var serializer = new XmlSerializer(typeof(List<Logovi>));
        using (var reader = new FileStream(_xmlFilePath, FileMode.Open))
        {
            return (List<Logovi>)serializer.Deserialize(reader);
        }
    }

    private void WriteXmlData(List<Logovi> data)
    {
        var serializer = new XmlSerializer(typeof(List<Logovi>));
        using (var writer = new FileStream(_xmlFilePath, FileMode.Create))
        {
            serializer.Serialize(writer, data);
        }
    }

    private List<Logovi> ReadJsonData()
    {
        if (!File.Exists(_jsonFilePath))
        {
            return new List<Logovi>();
        }
        var jsonData = File.ReadAllText(_jsonFilePath);
        return JsonConvert.DeserializeObject<List<Logovi>>(jsonData);
    }

    private void WriteJsonData(List<Logovi> data)
    {
        var jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(_jsonFilePath, jsonData);
    }
}
