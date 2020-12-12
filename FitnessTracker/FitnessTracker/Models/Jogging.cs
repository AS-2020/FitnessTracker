using FitnessTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FitnessTracker.Models
{
    public class Jogging
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


        private string _joggingTime;

        public string JoggingTime
        {
            get { return _joggingTime; }
            set { _joggingTime = value; }
        }


    }
    class JoggingHandler
    { 
        private List<Jogging> joggingList;
        public JoggingHandler()
        {
            joggingList = new List<Jogging>();
        }

        private static JoggingHandler _instance;
        public static JoggingHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new JoggingHandler();
                }
                return _instance;
            }
        }

        public const string FILENAME = "FileJogging.txt";

        public static string localPath = System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + FILENAME);
        public async void Save()
        {
            await MainVm.CheckAndRequestStorageWritePermission();


            XmlSerializer ser = new XmlSerializer(joggingList.GetType());
            using (FileStream stream = File.Create(localPath))
            {
                ser.Serialize(stream, joggingList);
            }
        }
        public async void Load()
        {
            await MainVm.CheckAndRequestStorageReadPermission();

            try
            {
                if (File.Exists(localPath)) // File.Exists(FILENAME)
                {
                    XmlSerializer ser = new XmlSerializer(joggingList.GetType());
                    using (FileStream stream = File.Open(localPath, FileMode.Open))
                    {
                        joggingList = ser.Deserialize(stream) as List<Jogging>;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
                // Event auslösen ...
            }

        }
        public void AddJogging(Jogging jogging)
        {
            joggingList.Add(jogging);
            joggingList = joggingList.OrderBy(d => d.Date_asDate).ToList();
        }
        public List<Jogging> GetJogging()
        {
            return joggingList.OrderBy(d => d.Date_asDate).ToList();
        }

        public async void Delete()
        {
            await MainVm.CheckAndRequestStorageWritePermission();

            joggingList.Clear();

            XmlSerializer ser = new XmlSerializer(joggingList.GetType());
            using (FileStream stream = File.Create(localPath))
            {
                ser.Serialize(stream, joggingList);
            }
        }
    }
}
