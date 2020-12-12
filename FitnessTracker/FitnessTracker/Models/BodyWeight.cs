using FitnessTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Xamarin.Essentials;

namespace FitnessTracker.Models
{
    public class BodyWeight
    {
        private DateTime _date_asDate;

        public DateTime Date_asDate
        {
            get { return _date_asDate; }
            set { _date_asDate = value; }
        }

        private string _showDate;
        public string ShowDate
        {
            get { return Date_asDate.ToShortDateString(); }
        }

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

        //var documentsPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public static string localPath = System.IO.Path.Combine (System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + FILENAME);
        public void AddBodyWeight(BodyWeight bodyWeight)
        {
            bodyWeightList.Add(bodyWeight);
            bodyWeightList = bodyWeightList.OrderBy(d => d.Date_asDate).ToList();
        }

        public List<BodyWeight> GetBodyWeight()
        {
            return bodyWeightList.OrderBy(d => d.Date_asDate).ToList();
        }

        public async void Save()
        {
            await MainVm.CheckAndRequestStorageWritePermission();

            
            XmlSerializer ser = new XmlSerializer(bodyWeightList.GetType());
            using (FileStream stream = File.Create(localPath))
            {
                ser.Serialize(stream, bodyWeightList);
            }
        }
        public async void Load()
        {
            await MainVm.CheckAndRequestStorageReadPermission();

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

        public async void Delete()
        {
            await MainVm.CheckAndRequestStorageWritePermission();

            bodyWeightList.Clear();

            XmlSerializer ser = new XmlSerializer(bodyWeightList.GetType());
            using (FileStream stream = File.Create(localPath))
            {
                ser.Serialize(stream, bodyWeightList);
            }
        }

    }
}
