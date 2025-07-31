using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Models
{
    [Serializable]
    public class Word
    {
        private string _word;
        private List<string> _tranlations;

        public string WordValue
        {
            get { return _word; }
            set
            {
                if(!String.IsNullOrEmpty(value) && !String.IsNullOrWhiteSpace(value))
                {
                    _word = value;
                }
            }
        }

        public List<string> Translations
        {
            get { return _tranlations; }
            set
            {
                if(value != null)
                {
                    _tranlations = value;
                }
            }
        }

        public override string ToString()
        {
            string translations = "";
            for (int i = 0; i < Translations.Count; ++i)
            {
                translations += Translations[i];
                if(i < Translations.Count - 1)
                {
                    translations += ", ";
                }
            }
            return $"{WordValue}: {translations}";
        }
    }
}