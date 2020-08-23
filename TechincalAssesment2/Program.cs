using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechincalAssesment2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Declare varaible
            Dictionary<int, int> InputDic = new Dictionary<int, int>();
            int number = 0;
            int total = 0;
            int finaltotal = 0;
            string message = "";
            string finalmessage = "";
            string plus = "";
            int lastnum = 0;

            Console.WriteLine("Please insert 9 numbers:");
            //loop the number of time for user to insert the input
            for (int i = 1; i<=9; i++) 
            {
                string input = Console.ReadLine();
                int result;
                //validation to check valid input
                if(input != null && int.TryParse(input, out result))
                {
                    number = Convert.ToInt32(input);
                    InputDic.Add(i, number);
                }
                else
                {
                    Console.WriteLine("Please insert a valid  number:");
                    i -= 1;
                }
            }


            //loop through each item in Dictonary
            foreach (KeyValuePair<int,int> item in InputDic)
            {
                //to check the mod is 0, means by looping the position of even in the list.
                if(item.Key % 2 == 0)
                {
                    message = lastnum + "+" + item.Value;
                    finalmessage += plus + message;
                    total = item.Value + lastnum;
                    finaltotal += total;
                    plus = "+";
                    Console.WriteLine(finalmessage + "=" + finaltotal + "\n");
                }
                lastnum = item.Value;
            }
            Console.ReadLine();
        }
    }
}
