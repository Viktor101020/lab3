using System.Diagnostics;
using System.Globalization;

namespace TempProject;

internal class Program
{
    public static DateTime GetDateTime(string line)
    {
        int index = line.IndexOf(' ', line.IndexOf(' ') + 1);
        string datetimeStr = line.Substring(0, index);

        return DateTime.Parse(datetimeStr);
    }

    public static string GetLevel(string line)
    {
        int index1 = line.IndexOf('[') + 1;
        int index2 = line.IndexOf(']') - 1;
        string getLevelStr = line.Substring(index1, index2-index1+1);

        return getLevelStr;
    }

    public static string GetCategory(string line)
    {
        int index1 = line.IndexOf('[', line.IndexOf('[') + 1);
        int index2 = line.IndexOf(']', line.IndexOf(']') + 1);
        string getCategorylStr = line.Substring(index1 + 1,index2-index1 - 1);

        return getCategorylStr;
    }

    public static string GetText(string line)
    {
        int index = line.IndexOf(' ', line.IndexOf(']', line.IndexOf(']') + 1) + 1);
        string getText = line.Substring(index+1);
        return getText;
    }

    class Ivent
    {
        public string ivdate= "Undefined";
        public string ivname= "Undefined";
        public string ivloot= "Undefined";
        public string ivviner= "Undefined";
        public int ivvpoint;
        public string ivloot2= "Undefined";
        public string ivvin2 = "Undefined";
        public int ivwarning=0;
        public int iverror=0;
    }


    public static void Main()
    {
        string baffer;
        string[] lines = File.ReadAllLines(@"event_server.log");
        Ivent seriv = new Ivent();
        bool F=false;
        int baf;
        foreach (string line in lines) {
            if (GetText(line).Contains("Событие началось:"))
            {
                F= true;
                DateTime dateTime = GetDateTime(line);
                seriv.ivdate = dateTime.ToString("dd.MM.yyyy");
                seriv.ivname = line.Substring(line.IndexOf(":",line.IndexOf('['))+2);
                
            }
            
            else if (GetText(line) == $" Событие \"{seriv.ivname}\" закрыто")
            {
                F = false;
                Console.WriteLine("F = 0");
            }
            else if (GetLevel(line)== "Warning" && F)
            {
                seriv.ivwarning++;
                
            }
            else if (GetLevel(line) == "Error" && F)
            {
                seriv.iverror++;
                
            }

            else if (GetCategory(line) == "Loot" & GetText(line).Contains("получили ивентовый предмет:") & F)
            {
                baffer = GetText(line);
                seriv.ivloot = baffer.Substring(baffer.IndexOf(':')+2);
            }
            else if (GetCategory(line) == "Reward" & GetText(line).Contains("объявлены победителями события") & F)
            {
                baffer = GetText(line);
                seriv.ivviner = baffer.Substring(0, baffer.IndexOf(' ', baffer.IndexOf(' ') + 1));
            }
            else if (GetCategory(line) == "Reward" & GetText(line).Contains("утешительную награду:") & F)
            {
                baffer = GetText(line);
                baf = baffer.IndexOf(' ', baffer.IndexOf(' ', baffer.IndexOf(':')+2)+1);
                seriv.ivloot2 = baffer.Substring(baffer.IndexOf(':')+2 ,baf  - baffer.IndexOf(':')-2);
                seriv.ivvin2= baffer.Substring(0, baffer.IndexOf(' ', baffer.IndexOf(' ')+ 1));
            }
            else if (GetCategory(line) == "Reward" & GetText(line).Contains("получили") & GetText(line).Contains("очков события") & F)
            {
                baffer = GetText(line);
                baf = baffer.IndexOf("получили") + 9;
                baffer = baffer.Substring(baf, baffer.IndexOf("очков события") - baf - 1);
                if (GetText(line).Contains($"получили {baffer} очков события"))
                {
                    seriv.ivvpoint = int.Parse(baffer);
                }
            }

        }
        Console.WriteLine($"Итоги события: {seriv.ivname}\n\n");
        Console.WriteLine($"Дата: {seriv.ivdate}");
        Console.WriteLine($"Победитель: {seriv.ivviner}");
        Console.WriteLine($"Очки победителя: {seriv.ivvpoint}");
        Console.WriteLine($"Ивентовый предмет: {seriv.ivloot}");
        Console.WriteLine($"Утешительная награда клана \"{seriv.ivvin2}\":{seriv.ivloot2}");
        Console.WriteLine($"Предупреждений во время события: {seriv.ivwarning}");
        Console.WriteLine($"Ошибок во время события: {seriv.iverror}");
               
        


       
    }
}
