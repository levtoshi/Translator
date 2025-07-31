using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DLL.Models;
using DLL.Services;

namespace BLL.Services
{
    public class BusinessLogicServices
    {
        static public bool AddDictionaryBLL(List<Dictionary> dictionaries, string name)
        {
            Dictionary dictionary = dictionaries.Find(e => e.Name == name);
            if(dictionary == null)
            {
                dictionaries.Add(new Dictionary(name));
                dictionaries[dictionaries.Count - 1].Words = new List<Word>();
                return true;
            }
            return false;
        }

        static public bool DeleteDictionaryBLL(List<Dictionary> dictionaries, string name)
        {
            Dictionary testDict = dictionaries.Find(e => e.Name == name);
            if (testDict != null)
            {
                dictionaries.Remove(testDict);
                DataManagmentServices.DeleteDictFile(testDict);
                return true;
            }
            return false;
        }

        static public bool AddWordBLL(Dictionary dictionary, string word, string translations)
        {
            Word wordObj = dictionary.Words.Find(e => e.WordValue == word);
            if(wordObj == null)
            {
                dictionary.Words.Add(new Word() { WordValue = word, Translations = translations.Split(' ').ToList<string>() });
                return true;
            }
            return false;
        }

        static public int ChangeWordBLL(Dictionary dictionary, string oldWord, string newWord)
        {
            Word testWord = dictionary.Words.Find(e => e.WordValue == oldWord);
            Word dublicateWord = dictionary.Words.Find(e => e.WordValue == newWord);
            if (dublicateWord != null)
            {
                return 3;
            }
            else if (testWord != null)
            {
                testWord.WordValue = newWord;
                return 1;
            }
            return 2;
        }

        static public int ChangeTranslationBLL(Dictionary dictionary, string word, string oldTranslation, string newTranslation)
        {
            Word testWord = dictionary.Words.Find(e => e.WordValue == word);
            if (testWord != null)
            {
                string testStr = testWord.Translations.Find(e => e == oldTranslation);
                string dublicateStr = testWord.Translations.Find(e => e == newTranslation);
                if (dublicateStr != null)
                {
                    return 4; // Dublicate
                }
                if (testStr != null)
                {
                    int index = testWord.Translations.IndexOf(testStr);
                    testWord.Translations[index] = newTranslation;
                    return 1; // OK
                }
                return 3; // Translation not found
            }
            return 2; // Word not found
        }

        static public int AddWordTranslationBLL(Dictionary dictionary, string word, string translation)
        {
            Word testWord = dictionary.Words.Find(e => e.WordValue == word);
            if (testWord != null)
            {
                string dublicateStr = testWord.Translations.Find(e => e == translation);
                if(dublicateStr != null)
                {
                    return 3;
                }
                testWord.Translations.Add(translation);
                return 1;
            }
            return 2;
        }

        static public bool DeleteWordBLL(Dictionary dictionary, string word)
        {
            Word testWord = dictionary.Words.Find(e => e.WordValue == word);
            if(testWord != null)
            {
                dictionary.Words.Remove(testWord);
                return true;
            }
            return false;
        }

        static public int DeleteTranslationBLL(Dictionary dictionary, string word, string translation)
        {
            Word testWord = dictionary.Words.Find(e => e.WordValue == word);
            if (testWord != null)
            {
                if (testWord.Translations.Count > 1)
                {
                    string testStr = testWord.Translations.Find(e => e == translation);
                    if (testStr != null)
                    {
                        testWord.Translations.Remove(testStr);
                        return 1; // OK
                    }
                    return 3; // Translation not found
                }
                return 4; // The least translation
            }
            return 2; // Word not found
        }

        static public Word FindWordBLL(Dictionary dictionary, string word)
        {
            return dictionary.Words.Find(e => e.WordValue == word);
        }
    }
}