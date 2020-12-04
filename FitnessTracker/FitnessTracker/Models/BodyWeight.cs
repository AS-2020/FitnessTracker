using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Xamarin.Essentials;

namespace FitnessTracker.Models
{
    public class BodyWeight
    {
        public DateTime DateTime { get; set; }
        public decimal Weight { get; set; }
        public decimal BodyFat { get; set; }
    }
    class BodyWeightHandler
    {
        private List<BodyWeight> bodyWeightList;
        private BodyWeightHandler()
        {
            bodyWeightList = new List<BodyWeight>();
        }
        private static BodyWeightHandler _instance;
        public static BodyWeightHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BodyWeightHandler();
                }
                return _instance;
            }
        }
        public const string FILENAME = "FileBodyWeight.txt";

        public static string localPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/FitnessTracker/FileBodyWeight.txt";
        public void AddBodyWeight(BodyWeight bodyWeight)
        {
            bodyWeightList.Add(bodyWeight);
        }

        public IEnumerable<BodyWeight> GetBodyWeight()
        {
            return bodyWeightList;
        }

        public void Save()
        {
            if (!File.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            XmlSerializer ser = new XmlSerializer(bodyWeightList.GetType());
            using (FileStream stream = File.Create(localPath))
            {
                ser.Serialize(stream, bodyWeightList);
            }
        }
        public void Load()
        {
            try
            {
                if (File.Exists(localPath)) // File.Exists(FILENAME)
                {
                    XmlSerializer ser = new XmlSerializer(bodyWeightList.GetType());
                    using (FileStream stream = File.Open(localPath, FileMode.Open))
                    {
                        bodyWeightList = ser.Deserialize(stream) as List<BodyWeight>;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
                // Event auslösen ...
            }

        }

    }
}
