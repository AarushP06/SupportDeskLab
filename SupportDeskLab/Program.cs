using System;
using System.Collections.Generic;
using static SupportDeskLab.Utility;


namespace SupportDeskLab
{
   
     
    class Program
    {
        static int NextTicketId = 1;

        //Create Customer Dictionary
        //create Ticket Queue
        //Create UndoEvent stack

        static void Main()
        {
            initCustomer();

            while (true)
            {
                Console.WriteLine("\n=== Support Desk ===");
                Console.WriteLine("[1] Add customer");
                Console.WriteLine("[2] Find customer");
                Console.WriteLine("[3] Create ticket");
                Console.WriteLine("[4] Serve next ticket");
                Console.WriteLine("[5] List customers");
                Console.WriteLine("[6] List tickets");
                Console.WriteLine("[7] Undo last action");
                Console.WriteLine("[0] Exit");
                Console.Write("Choose: ");
                string choice = Console.ReadLine();

                //create switch cases and then call a reletive method 
                //for example for case 1 you need to have a method named addCustomer(); or case 2 add a method name findCustomer

                switch (choice)
                {
                    case "1": AddCustomer(); break;
                    case "2": FindCustomer(); break;
                    case "3": CreateTicket(); break;
                    case "4": ServeNext(); break;
                    case "5": ListCustomers(); break;
                    case "6": ListTickets(); break;
                    case "7": Undo(); break;
                    case "0": return;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
        }
        /*
         * Do not touch initCustomer method. this is like a seed to have default customers.
         */
        static void initCustomer()
        {
            //uncomments these 3 lines after you create the Customer Dictionary
            //Customers["C001"] = new Customer("C001", "Ava Martin", "ava@example.com");
            //Customers["C002"] = new Customer("C002", "Ben Parker", "ben@example.com");
            //Customers["C003"] = new Customer("C003", "Chloe Diaz", "chloe@example.com");
        }

        static void AddCustomer()
        {
            //look at the Demo captuerd image and add your code here
           
        }

        static void FindCustomer()
        {
            //look at the Demo captuerd image and add your code here

        }

        static void CreateTicket()
        {
            //look at the Demo captuerd image and add your code here
         
        }

        static void ServeNext()
        {
            //look at the Demo captuerd image and add your code here
           
        }

        static void ListCustomers()
        {
            Console.WriteLine("-- Customers --");
            //look at the Demo captuerd image and add your code here

        }

        static void ListTickets()
        {
           
            Console.WriteLine("-- Tickets (front to back) --");
            //look at the Demo captuerd image and add your code here
            if (Tickets.Count == 0)
            {
                Console.WriteLine("(none)");
                return;
            }

        }

        static void Undo()
        {
            //look at the Demo captuerd image and add your code here
           
        }
    }
}

