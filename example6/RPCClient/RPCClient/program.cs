﻿using System;
using RPCClient;

namespace RPCClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var RPCClient = new RPCClient();
            string n = "";
            var response = "";

            while (true)
            {
               
                Console.WriteLine("Enter the number of fib or 'end' to close the programm");
                n = Console.ReadLine();

                if (n == "end") break;

                response = RPCClient.Call(n);
                Console.WriteLine("[.] Got {0}", response);                
            }

            RPCClient.Close();
        }
    }
}