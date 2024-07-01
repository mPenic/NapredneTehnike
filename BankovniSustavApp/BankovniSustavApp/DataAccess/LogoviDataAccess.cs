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

    public List<Logovi> ReadXmlData()
    {
        if (!File.Exists(_xmlFilePath))
        {
            return new List<Logovi>();
        }

        using (var stream = new FileStream(_xmlFilePath, FileMode.Open))
        {
            var serializer = new XmlSerializer(typeof(List<Logovi>));
            return (List<Logovi>)serializer.Deserialize(stream);
        }
    }

    public void WriteXmlData(List<Logovi> data)
    {
        using (var stream = new FileStream(_xmlFilePath, FileMode.Create))
        {
            var serializer = new XmlSerializer(typeof(List<Logovi>));
            serializer.Serialize(stream, data);
        }
    }

    public List<Logovi> ReadJsonData()
    {
        if (!File.Exists(_jsonFilePath))
        {
            return new List<Logovi>();
        }

        var jsonData = File.ReadAllText(_jsonFilePath);
        return JsonConvert.DeserializeObject<List<Logovi>>(jsonData);
    }

    public void WriteJsonData(List<Logovi> data)
    {
        var jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(_jsonFilePath, jsonData);
    }

    public void AddLog(Logovi log, string format = "xml")
    {
        List<Logovi> data = format.ToLower() == "json" ? ReadJsonData() : ReadXmlData();
        data.Add(log);

        if (format.ToLower() == "json")
        {
            WriteJsonData(data);
        }
        else
        {
            WriteXmlData(data);
        }
    }
}
