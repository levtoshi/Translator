using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DLL.Services;
using DLL.Models;
using BLL.Services;

namespace Translator.Services
{
    public class TranslatorUIServices
    {
        static private List<Dictionary> Dictionaries;
        static public void MenuUI()
        {
            Dictionaries = DataManagmentServices.SetupDictionariesDLL();

            int command;
            string temp = "";
            int dictionary = -1;
            bool IsInMenu = true;
            bool ClearConsole = true;

            while (temp != "q")
            {
                if (ClearConsole)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    if (IsInMenu)
                    {
                        Console.WriteLine("-----------------TRANSLATOR MENU-----------------\n" +
                        "If you want to exit program and save all changes type 'q'\n" +
                        "Type 'clear' to clear the console\n" +
                        "Enter number of command:\n" +
                        "1 - Add new dictionary\n" +
                        "2 - Delete dictionary\n" +
                        "3 - Show all dictionaries names\n" +
                        "4 - Switch to dictionary\n");
                    }
                    else
                    {
                        Console.WriteLine($"-----------------DICTIONARY #{dictionary + 1}-----------------\n" +
                        "If you want to go back to menu type 'q'\n" +
                        "Type 'clear' to clear the console\n" +
                        "Enter number of command:\n" +
                        "1 - Add new word with its translation\n" +
                        "2 - Change word or its translation\n" +
                        "3 - Delete word or its translation\n" +
                        "4 - Find word translation\n" +
                        "5 - Show all words with their translations\n");
                    }
                    ClearConsole = false;
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Terminal = ");
                temp = Console.ReadLine();
                if(temp == "clear")
                {
                    ClearConsole = true;
                }
                else if (temp != "q")
                {
                    try
                    {
                        command = Convert.ToInt32(temp);
                        Console.WriteLine();
                        if (IsInMenu)
                        {
                            switch (command)
                            {
                                case 1: AddDictionaryUI(Dictionaries); break;
                                case 2: DeleteDictionaryUI(Dictionaries); break;
                                case 3: ShowAllDictionariesUI(); break;
                                case 4:
                                    dictionary = SwitchToDictionaryUI();
                                    if(dictionary != -1)
                                    {
                                        ClearConsole = true;
                                        IsInMenu = false;
                                    }
                                    break;
                                default:
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Invalid input!");
                                    break;
                            }
                        }
                        else
                        {
                            switch (command)
                            {
                                case 1: AddWordUI(Dictionaries[dictionary]); break;
                                case 2: ChangeWordUI(Dictionaries[dictionary]); break;
                                case 3: DeleteWordUI(Dictionaries[dictionary]); break;
                                case 4: FindWordUI(Dictionaries[dictionary]); break;
                                case 5:
                                    if(Dictionaries[dictionary].Words.Count > 0)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine(Dictionaries[dictionary]);
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("There are no words in the dictionary!");
                                    }
                                    break;
                                default:
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Invalid input!");
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else if (!IsInMenu)
                {
                    temp = "c";
                    ClearConsole = true;
                    IsInMenu = true;
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Goodbye!");
            Console.WriteLine();
            Console.ResetColor();
            DataManagmentServices.BackupTranslatorDLL(Dictionaries);
        }

        static private int SwitchToDictionaryUI()
        {
            ShowAllDictionariesUI();
            int mode = -1;
            if (Dictionaries.Count > 0)
            {
                Console.WriteLine();
                while(mode > Dictionaries.Count || mode < 1)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    try
                    {
                        Console.Write("Enter number of dictionary = ");
                        mode = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                return (mode - 1);
            }
            return -1;
        }

        static private void ShowAllDictionariesUI()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (Dictionaries.Count > 0)
            {
                for (int i = 0; i < Dictionaries.Count; ++i)
                {
                    Console.WriteLine($"{i + 1}) {Dictionaries[i].Name}");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There are no dictionaries!");
            }
        }

        static private void AddDictionaryUI(List<Dictionary> dictionaries)
        {
            string name = "";
            while(String.IsNullOrEmpty(name))
            {
                Console.Write("Enter new dictionary name = ");
                name = Console.ReadLine();
            }
            if (BusinessLogicServices.AddDictionaryBLL(dictionaries, name))
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Dictionary successfully added!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Dictionary already exists!");
            }
        }

        static private void DeleteDictionaryUI(List<Dictionary> dictionaries)
        {
            if (dictionaries.Count > 0)
            {
                string name = "";
                while (String.IsNullOrEmpty(name))
                {
                    Console.Write("Enter dictionary name for deletion = ");
                    name = Console.ReadLine();
                }
                if (BusinessLogicServices.DeleteDictionaryBLL(dictionaries, name))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Dictionary successfully deleted!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Dictionary can't be found!");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There are no dictionaries!");
            }
        }

        static private void AddWordUI(Dictionary dictionary)
        {
            string word = "", translation = "";
            while (String.IsNullOrEmpty(word))
            {
                Console.Write("Enter new word = ");
                word = Console.ReadLine();
            }
            while (String.IsNullOrEmpty(translation))
            {
                Console.Write("Enter new word's translation (it can be some words with Space between them) = ");
                translation = Console.ReadLine();
            }
            if (BusinessLogicServices.AddWordBLL(dictionary, word, translation))
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Word successfully added!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Word already exists!");
            }
        }

        static private void ChangeWordUI(Dictionary dictionary)
        {
            if (dictionary.Words.Count > 0)
            {
                int mode;
                string modeStr = "";
                string translationOld = "";
                string translationNew = "";
                string word = "";
                string newWord = "";
                bool correct = false;
                while (!correct)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    try
                    {
                        while (String.IsNullOrEmpty(word))
                        {
                            Console.Write("Enter word = ");
                            word = Console.ReadLine();
                        }
                        while (String.IsNullOrEmpty(modeStr))
                        {
                            Console.Write("Enter what you would like to change (1 - word, 2 - specific translation of the word, 3 - add translation) = ");
                            modeStr = Console.ReadLine();
                        }
                        mode = Convert.ToInt32(modeStr);
                        switch (mode)
                        {
                            case 1:
                                while (String.IsNullOrEmpty(newWord))
                                {
                                    Console.Write("Enter word's new value = ");
                                    newWord = Console.ReadLine();
                                }
                                switch(BusinessLogicServices.ChangeWordBLL(dictionary, word, newWord))
                                {
                                    case 1:
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine("Word successfully changed!");
                                        break;
                                    case 2:
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Word can't be found!");
                                        break;
                                    case 3:
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Word already exists!");
                                        break;
                                }
                                break;
                            case 2:
                                while (String.IsNullOrEmpty(translationOld))
                                {
                                    Console.Write("Enter translation = ");
                                    translationOld = Console.ReadLine();
                                }
                                while (String.IsNullOrEmpty(translationNew))
                                {
                                    Console.Write("Enter translation's new value = ");
                                    translationNew = Console.ReadLine();
                                }
                                switch (BusinessLogicServices.ChangeTranslationBLL(dictionary, word, translationOld, translationNew))
                                {
                                    case 1:
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine("Translation changed!");
                                        break;
                                    case 2:
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Word can't be found!");
                                        break;
                                    case 3:
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Translation not found!");
                                        break;
                                    case 4:
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Translation already exists!");
                                        break;
                                }
                                break;
                            case 3:
                                while (String.IsNullOrEmpty(translationNew))
                                {
                                    Console.Write("Enter word's new translation = ");
                                    translationNew = Console.ReadLine();
                                }
                                switch (BusinessLogicServices.AddWordTranslationBLL(dictionary, word, translationNew))
                                {
                                    case 1:
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine("Translation successfully added!");
                                        break;
                                    case 2:
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Word can't be found!");
                                        break;
                                    case 3:
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Translation already exists!");
                                        break;
                                }
                                break;
                            default:
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid input!");
                                break;
                        }
                        correct = true;
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Error: {ex.Message}");
                        Console.WriteLine();
                        modeStr = "";
                        word = "";
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There are no words in the dictionary!");
            }
        }

        static private void DeleteWordUI(Dictionary dictionary)
        {
            if (dictionary.Words.Count > 0)
            {
                int mode;
                string tempStr = "";
                string word = "";
                bool correct = false;
                while(!correct)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    try
                    {
                        while (String.IsNullOrEmpty(word))
                        {
                            Console.Write("Enter word = ");
                            word = Console.ReadLine();
                        }
                        while (String.IsNullOrEmpty(tempStr))
                        {
                            Console.Write("Enter what you would like to delete (1 - word with translation, 2 - specific translation of the word) = ");
                            tempStr = Console.ReadLine();
                        }
                        mode = Convert.ToInt32(tempStr);
                        tempStr = "";
                        switch (mode)
                        {
                            case 1:
                                if (BusinessLogicServices.DeleteWordBLL(dictionary, word))
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine("Word successfully deleted!");
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Word can't be found!");
                                }
                                break;
                            case 2:
                                while (String.IsNullOrEmpty(tempStr))
                                {
                                    Console.Write("Enter translation = ");
                                    tempStr = Console.ReadLine();
                                }
                                switch (BusinessLogicServices.DeleteTranslationBLL(dictionary, word, tempStr))
                                {
                                    case 1:
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine("Translation deleted!");
                                        break;
                                    case 2:
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Word can't be found!");
                                        break;
                                    case 3:
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Translation not found!");
                                        break;
                                    case 4:
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("You can't delete the least translation of the word!");
                                        break;
                                }
                                break;
                            default:
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid input!");
                                break;
                        }
                        correct = true;
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Error: {ex.Message}");
                        Console.WriteLine();
                        tempStr = "";
                        word = "";
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There are no words in the dictionary!");
            }
        }

        static private void FindWordUI(Dictionary dictionary)
        {
            if (dictionary.Words.Count > 0)
            {
                string word = "";
                while (String.IsNullOrEmpty(word))
                {
                    Console.Write("Enter word to find its tranlation = ");
                    word = Console.ReadLine();
                }
                Word wordObj = BusinessLogicServices.FindWordBLL(dictionary, word);
                if (wordObj != null)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Word found:\n{wordObj}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Word can't be found!");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There are no words in the dictionary!");
            }
        }
    }
}