using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game5
{
    internal class PlayerUpgrades
    {
        //Class properties
        private string[] upgradeLevels;
        private int hpBuff;
        private int atkBuff;
        private int spdBuff;

        public string[] UpgradeLevels
        { get { return upgradeLevels; } }
        public int HPBuff
        { get { return hpBuff; } }
        public int AtkBuff
        { get { return atkBuff; } }
        public int SpdBuff
        { get { return spdBuff; } }
        public void RetrieveUpgradeLevels()
        //This will retrieve upgrade levels from the playerupgrades.txt file.
        {
            string textFileData;
            //This will check if the file exist.
            if (File.Exists("playerupgrades.txt"))
            {
                using (StreamReader sr = new StreamReader("playerupgrades.txt"))
                {
                    textFileData = sr.ReadLine();
                }
            }
            //If the file doesn't exist. It will create a default text file with base values.
            else
            {
                textFileData = "0;0;0";
                using (StreamWriter sw = new StreamWriter("playerupgrades.txt"))
                {
                    sw.WriteLine(textFileData);
                }
            }
            //This will modify the class property 'upgradeLevels'.
            upgradeLevels = textFileData.Split(';');
        }
        public void RetrieveStatBuff()
        //This will set the values of hpBuff, atkBuff, and spdBuff based on the upgrade levels to be used in survival mode. 
        {
            RetrieveUpgradeLevels();
            hpBuff = int.Parse(upgradeLevels[0]) * 10;
            atkBuff = int.Parse(upgradeLevels[1]) * 1;
            spdBuff = int.Parse(upgradeLevels[2]) * 3;
        }
        public void UpdateUpgradeLevels(string[] newValues)
        //This will update the upgradeLevels array.
        {
            upgradeLevels = newValues;
            UpdateTextFile(upgradeLevels);
        }
        public void UpdateTextFile(string[] upgradeLevels)
        //This will overwrite the playerupgrades.txt file if any changes is made on the upgrade levels.
        {
            string tempString;
            tempString = string.Join(";", upgradeLevels);
            using (StreamWriter sw = new StreamWriter("playerupgrades.txt"))
            {
                sw.WriteLine(tempString);
            }
        }
    }
}
