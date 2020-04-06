using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantsGuideApplication.Controllers
{
    public class Calculation
    {
        Dictionary<string, string> galaticValue = new Dictionary<string, string>();
        Dictionary<string, double> objectValue = new Dictionary<string, double>();


        public string decodeMessages(string message)
        {
            try
            {
                if (message == "")
                    return ("");
                message= message.ToLower();
                if (message[message.Length - 1] == '?')
                {
                    message= message.Remove(message.Length - 1);
                }
                String[] messageWords = message.Split(' ');
                if (message.StartsWith("how much is"))
                {
                    String romanFromGalaticValue;
                    romanFromGalaticValue = GalaticToRoman(messageWords.Skip(3).ToArray());
                    string result = CalculateGalaticValue(romanFromGalaticValue);
                    if (result == "1")
                    {
                        var startingSentence = messageWords.Skip(3);
                        string sen = String.Join(" ", startingSentence);
                        return (sen + " is " + ConvertRomanToNumber(romanFromGalaticValue));
                    }
                    return (result);
                }

                else if (message.StartsWith("how many credits is"))
                {
                    var x = messageWords.Skip(4);
                    string romanFromGalaticValue = GalaticToRoman(x.Reverse().Skip(1).Reverse().ToArray());
                    string result = CalculateGalaticValue(romanFromGalaticValue);
                    if (result == "1")
                    {
                        double valueOfObject = objectValue[messageWords[messageWords.Length - 1]] * ConvertRomanToNumber(romanFromGalaticValue);
                        var startingSentence = messageWords.Skip(4);
                        string sen = String.Join(" ", startingSentence);
                        return (sen + " is " + valueOfObject + " Credits");
                    }
                    return (result);
                }
                
                else if (messageWords.Length == 3 && messageWords[1].Equals("is") && !message.Contains("credits"))
                {
                    if (IsRomanValid(messageWords[2]))
                    {
                        // int value = ConvertRomanToNumber(messageWords[2]);
                        galaticValue.Add(messageWords[0], messageWords[2]);
                        return ("");
                    }
                    else
                    {
                        return ("Not a valid Roman Value");
                    }
                }

                else if (message.Contains("is") && message.Contains("credits"))
                {
                    int objectValue = ValueOfObject(messageWords);
                    if (objectValue == 0)
                    {
                        return ("Not a proper statement");
                    }
                    else
                    {
                        return("");
                    }
                }

                return ("I have no idea what are you talking about!");

            }
            catch (Exception ex)
            {
                return (ex.Message);
            }

        }
        public String CalculateGalaticValue(String romanFromGalaticValue)
        {
            try
            {
                if (romanFromGalaticValue != null)
                {
                    if (IsRomanValid(romanFromGalaticValue))
                    {
                        return ("1");
                    }
                    else
                        return ("Not a Valid Roman sequence");
                }
                else
                {
                    return ("I have no idea what are you talking about!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public String GalaticToRoman(String[] message)
        {
            try
            {
                String romanTerm = "";
                int i;
                for (i = 0; i < message.Length; i++)
                {
                    if (galaticValue.ContainsKey(message[i]))
                    {
                        string galatic = galaticValue[message[i]];

                        romanTerm = romanTerm + galatic;

                    }                  
                    
                    else
                        break;
                }
                String x = romanTerm;
                if (i != message.Length)
                    return null;
                else
                    return x;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int ValueOfObject(string[] message)
        {
            try
            {
                String galaticTerm = "";
                int i;
                for (i = 0; i < message.Length; i++)
                {
                    if (galaticValue.ContainsKey(message[i]) ) { 
                        string galatic = galaticValue[message[i]];                   
                        galaticTerm = galaticTerm + galatic;
                    }
                    else
                        break;
                }
                if (IsRomanValid(galaticTerm))
                {
                    int credit = Int32.Parse(message[i + 2]);
                    objectValue.Add(message[i], credit / ConvertRomanToNumber(galaticTerm));
                    return (1);
                }
                else
                {
                    return (-1);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int DigitValue(char roman)
        {
            Dictionary<char, int> romanValue = new Dictionary<char, int>();
            romanValue.Add('i', 1);
            romanValue.Add('v', 5);
            romanValue.Add('x', 10);
            romanValue.Add('l', 50);
            romanValue.Add('c', 100);
            romanValue.Add('d', 500);
            romanValue.Add('m', 1000);

            return (romanValue[roman]);
        }

        public int ConvertRomanToNumber(string romanString)
        {

            if (romanString.Length == 1)
            {
                return DigitValue(romanString[0]);
            }

            int value = 0;
            for (int i = 0; i < romanString.Length; i++)
            {
                if (i + 1 < romanString.Length)
                {
                    if (DigitValue(romanString[i]) >= DigitValue(romanString[i + 1]))
                        value = value + DigitValue(romanString[i]);
                    else
                    {
                        value = value + (DigitValue(romanString[i + 1]) - DigitValue(romanString[i]));
                        i++;
                    }

                }
                else
                    value = value + DigitValue(romanString[i]);
            }
            return value;
        }

        public bool IsRomanValid(string romanString)
        {
            Dictionary<char, bool> romanValid = new Dictionary<char, bool>();
            romanValid.Add('i', true);
            romanValid.Add('v', false);
            romanValid.Add('x', true);
            romanValid.Add('l', false);
            romanValid.Add('c', true);
            romanValid.Add('d', false);
            romanValid.Add('m', true);

            char[] roman = { 'i', 'v', 'x', 'l', 'c', 'd', 'm' };
            //char previousChar=' ';
            int repeatingCharacter = 1;
            //int poss;
            if (romanString.Length == 0)
                return false;
            if (romanString.Length == 1)
            {
                if (roman.Contains(romanString[0]))
                    return true;
                else
                    return false;
            }

            else
            {
                for (int i = 1; i < romanString.Length; i++)
                {
                    if (romanString[i] == romanString[i - 1] && !romanValid[romanString[i]])
                        return false;
                    if (romanString[i] == romanString[i - 1] && ++repeatingCharacter == 4)
                        return false;
                    if (DigitValue(romanString[i - 1]) < DigitValue(romanString[i]))
                    {
                        if (!romanValid[romanString[i - 1]])
                            return false;
                        if (Array.IndexOf(roman, romanString[i]) - Array.IndexOf(roman, romanString[i - 1]) > 2)
                            return false;
                    }
                    if (roman.Contains(romanString[i]))
                    {
                        if (romanString[i] == romanString[i - 1])
                        {
                            repeatingCharacter++;
                        }
                        else
                            repeatingCharacter = 1;
                    }
                    else
                        return false;


                }
            }
            return true;
        }
    }
}