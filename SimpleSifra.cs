using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace To_Do_list
{
    public class SimpleSifra
    {
        static List<string> Convertor(string a)
        {
            List<string> b = new List<string>();

            string[] test = Regex.Split(a, string.Empty);

            foreach (string s in test)
            {

                if (s != "")
                {
                    b.Add(s);
                }
            }
            return b;
        }
        public string Cipher(string text)
        {
            List<string> outputList = new List<string>();
            List<string> abeceda = new List<string>();
            List<string> sifra = new List<string>();
            List<string> veta = new List<string>();

            string finall = "";
            string key = @" !""#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyzěščřžýáíéúů{|}~";

            abeceda = Convertor(key);
            sifra = Enumerable.Reverse(abeceda).ToList();

            while (true)
            {
                veta = Convertor(text);
                for (int i = 0; i < veta.Count; i++)
                {
                    if (abeceda.Contains(veta[i]))
                    {
                        int indx = abeceda.IndexOf(veta[i]);
                        outputList.Add(sifra[indx]);
                    }
                }

                for (int i = 0; i < outputList.Count; i++)
                {
                    finall += outputList[i];
                }
                outputList.Clear();

                return finall;

            }
        }
    }
}
