using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game5
{
    internal class Currency
    {
        //Class Properties
        private int currencyAmount;

        public int CurrencyAmount
        {
            get { return currencyAmount; }
        }
        public void GetCurrency()
        //This method will read the currency.txt to retrieve the value of currencyAmount.
        {
            string textFileData;
            //This will check if the file exist.
            if (File.Exists("currency.txt"))
            {
                using (StreamReader sr = new StreamReader("currency.txt"))
                {
                    textFileData = sr.ReadLine();
                }
            }
            //If the file doesn't exist. It will create a default text file with 0 value.
            else
            {
                textFileData = "0";
                using (StreamWriter sw = new StreamWriter("currency.txt"))
                {
                    sw.WriteLine(textFileData);
                }
            }
            //This will modify the class property 'currencyAmount'.
            currencyAmount = int.Parse(textFileData);
        }
        public void EarnCurrency(int score)
        //This method will be used to increase the value of currencyAmount based on the score from Survival Mode.
        {
            currencyAmount += (score / 1000);
            UpdateTextFile(currencyAmount);
        }
        public void SpendCurrency(int cost)
        //This method will be used to modify the currencyAmount whenever something is upgraded in the Armory.
        {
            currencyAmount -= cost;
            UpdateTextFile(currencyAmount);
        }
        public void UpdateTextFile(int currencyAmount)
        //This will overwrite the currency.txt file if the value of currencyAmount is changed.
        {
            using (StreamWriter sw = new StreamWriter("currency.txt"))
            {
                sw.WriteLine(currencyAmount.ToString());
            }
        }
    }
}
