using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Models
{
    [Serializable]
    public class Dictionary
    {
        private string _name;
        private List<Word> _words;

        public string Name
        {
            get { return _name; }
            set
            {
                if (!String.IsNullOrEmpty(value) && !String.IsNullOrWhiteSpace(value))
                {
                    _name = value;
                }
            }
        }

        public List<Word> Words
        {
            get { return _words; }
            set 
            {
                if(value != null)
                {
                    _words = value;
                }
            }
        }

        public Dictionary(string name)
        {
            this._name = name;
        }

        public override string ToString()
        {
            string re = "";
            for (int i = 0; i < Words.Count; ++i)
            {
                re += (i + 1) + ") " + Words[i];
                if(i < Words.Count - 1)
                {
                    re += "\n";
                }
            }
            return re;
        }
    }
}