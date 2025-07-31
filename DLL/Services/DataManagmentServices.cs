using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using DLL.Models;

namespace DLL.Services
{
    public class DataManagmentServices
    {
        static private string _dictionaryPath;

        static private List<Dictionary> FillDictionariesDLL()
        {
            List<Dictionary> dictionaries = new List<Dictionary>();

            dictionaries = new List<Dictionary>()
            {
                new Dictionary("Ukrainian-English"),
                new Dictionary("English-Ukrainian")
            };

            dictionaries[0].Words = new List<Word>();
            dictionaries[0].Words.Add(new Word() { WordValue = "Привiт" });
            dictionaries[0].Words.Add(new Word() { WordValue = "Папа" });
            dictionaries[0].Words.Add(new Word() { WordValue = "Друг" });
            dictionaries[0].Words.Add(new Word() { WordValue = "Вчитель" });
            dictionaries[0].Words.Add(new Word() { WordValue = "Зустрiч" });
            dictionaries[0].Words.Add(new Word() { WordValue = "Житло" });
            dictionaries[0].Words.Add(new Word() { WordValue = "Держава" });

            dictionaries[0].Words[0].Translations = new List<string>();
            dictionaries[0].Words[1].Translations = new List<string>();
            dictionaries[0].Words[2].Translations = new List<string>();
            dictionaries[0].Words[3].Translations = new List<string>();
            dictionaries[0].Words[4].Translations = new List<string>();
            dictionaries[0].Words[5].Translations = new List<string>();
            dictionaries[0].Words[6].Translations = new List<string>();

            dictionaries[0].Words[0].Translations.Add("Hi");
            dictionaries[0].Words[0].Translations.Add("Hey");
            dictionaries[0].Words[0].Translations.Add("Hello");
            dictionaries[0].Words[0].Translations.Add("Howdy");

            dictionaries[0].Words[1].Translations.Add("Bye");
            dictionaries[0].Words[1].Translations.Add("Good luck");

            dictionaries[0].Words[2].Translations.Add("Friend");
            dictionaries[0].Words[2].Translations.Add("Mate");

            dictionaries[0].Words[3].Translations.Add("Teacher");

            dictionaries[0].Words[4].Translations.Add("Meeting");
            dictionaries[0].Words[4].Translations.Add("Appointment");

            dictionaries[0].Words[5].Translations.Add("Apartment");
            dictionaries[0].Words[5].Translations.Add("House");
            dictionaries[0].Words[5].Translations.Add("Flat");

            dictionaries[0].Words[6].Translations.Add("State");
            dictionaries[0].Words[6].Translations.Add("Country");


            dictionaries[1].Words = new List<Word>();
            dictionaries[1].Words.Add(new Word() { WordValue = "Hi" });
            dictionaries[1].Words.Add(new Word() { WordValue = "Bye" });
            dictionaries[1].Words.Add(new Word() { WordValue = "Mate" });
            dictionaries[1].Words.Add(new Word() { WordValue = "Teacher" });
            dictionaries[1].Words.Add(new Word() { WordValue = "Meeting" });
            dictionaries[1].Words.Add(new Word() { WordValue = "Apartment" });
            dictionaries[1].Words.Add(new Word() { WordValue = "State" });

            dictionaries[1].Words[0].Translations = new List<string>();
            dictionaries[1].Words[1].Translations = new List<string>();
            dictionaries[1].Words[2].Translations = new List<string>();
            dictionaries[1].Words[3].Translations = new List<string>();
            dictionaries[1].Words[4].Translations = new List<string>();
            dictionaries[1].Words[5].Translations = new List<string>();
            dictionaries[1].Words[6].Translations = new List<string>();

            dictionaries[1].Words[0].Translations.Add("Привiт");
            dictionaries[1].Words[0].Translations.Add("Як ся маєш");
            dictionaries[1].Words[0].Translations.Add("Здоров");

            dictionaries[1].Words[1].Translations.Add("Папа");
            dictionaries[1].Words[1].Translations.Add("Бувай здоровий");

            dictionaries[1].Words[2].Translations.Add("Друг");
            dictionaries[1].Words[2].Translations.Add("Колєга");

            dictionaries[1].Words[3].Translations.Add("Вчитель");

            dictionaries[1].Words[4].Translations.Add("Зiбрання");
            dictionaries[1].Words[4].Translations.Add("Зустрiч");
            dictionaries[1].Words[4].Translations.Add("Сходини");

            dictionaries[1].Words[5].Translations.Add("Житло");
            dictionaries[1].Words[5].Translations.Add("Дім");
            dictionaries[1].Words[5].Translations.Add("Будинок");

            dictionaries[1].Words[6].Translations.Add("Держава");
            dictionaries[1].Words[6].Translations.Add("Країна");

            return dictionaries;
        }

        static public List<Dictionary> SetupDictionariesDLL()
        {
            _dictionaryPath = "../../../DLL/Backup";
            DirectoryInfo directory = new DirectoryInfo(_dictionaryPath);
            if (directory.Exists) //&& directory.GetFiles().Length != 0)
            {
                return DeserializeDictionariesDLL();
            }
            return FillDictionariesDLL();
        }

        static public void BackupTranslatorDLL(List<Dictionary> dictionaries)
        {
            DirectoryInfo directory = new DirectoryInfo(_dictionaryPath);
            if (!directory.Exists)
            {
                directory.Create();
            }
            SerializeDictionariesDLL(dictionaries);
        }

        static public void DeleteDictFile(Dictionary dictionary)
        {
            DirectoryInfo directory = new DirectoryInfo(_dictionaryPath);
            FileInfo file = directory.GetFiles().ToList<FileInfo>().Find(e => e.Name == dictionary.Name + ".tl");
            if (file != null)
            {
                File.Delete(file.FullName);
            }
        }

        static private List<Dictionary> DeserializeDictionariesDLL()
        {
            List<Dictionary> dictionaries = new List<Dictionary>();
            DirectoryInfo directoryInfo = new DirectoryInfo(_dictionaryPath);
            FileInfo[] files = directoryInfo.GetFiles();
            BinaryFormatter formatter = new BinaryFormatter();
            foreach (FileInfo file in files)
            {
                using (FileStream fileStream = new FileStream(_dictionaryPath + "/" + file.Name, FileMode.Open, FileAccess.Read))
                {
                    dictionaries.Add(formatter.Deserialize(fileStream) as Dictionary);
                }
            }
            return dictionaries;
        }

        static private void SerializeDictionariesDLL(List<Dictionary> dictionaries)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            for (int i = 0; i < dictionaries.Count; ++i)
            {
                using (FileStream fileStream = new FileStream(_dictionaryPath + "/" + dictionaries[i].Name + ".tl", FileMode.OpenOrCreate, FileAccess.Write))
                {
                    formatter.Serialize(fileStream, dictionaries[i]);
                }
            }
        }
    }
}