using FitnessTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace FitnessTracker.Models
{
    public class Jogging
    {
        private DateTime _dateTime;

        public DateTime DateTime
        {
            get { return _dateTime.Date; }
            set { _dateTime = value.Date; }
        }

        private TimeSpan _joggingTime;

        public TimeSpan JoggingTime
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
        }
        public List<Jogging> GetJogging()
        {
            return joggingList;
        }
    }
}
