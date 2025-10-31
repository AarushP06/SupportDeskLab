using System;
using System.Collections.Generic;
using static SupportDeskLab.Utility;

namespace SupportDeskLab
{
    class Program
    {
        static int NextTicketId = 1;

        //Create Customer Dictionary
        static Dictionary<string, Customer> Customers = new Dictionary<string, Customer>(StringComparer.OrdinalIgnoreCase);

        //create Ticket Queue
        static Queue<Ticket> Tickets = new Queue<Ticket>();

        //Create UndoEvent stack
        static Stack<UndoEvent> UndoStack = new Stack<UndoEvent>();

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
            Customers["C001"] = new Customer("C001", "Ava Martin", "ava@example.com");
            Customers["C002"] = new Customer("C002", "Ben Parker", "ben@example.com");
            Customers["C003"] = new Customer("C003", "Chloe Diaz", "chloe@example.com");
        }

        static void AddCustomer()
        {
            //look at the Demo captuerd image and add your code here
            Console.Write("New CustomerId (e.g., C001, C011): ");
            var id = (Console.ReadLine() ?? "").Trim();
            Console.Write("Name: ");
            var name = (Console.ReadLine() ?? "").Trim();
            Console.Write("Email: ");
            var email = (Console.ReadLine() ?? "").Trim();

            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            if (Customers.ContainsKey(id))
            {
                Console.WriteLine("CustomerId already exists.");
                return;
            }

            var c = new Customer(id, name, email);
            Customers[id] = c;

            
            UndoStack.Push(new UndoAddCustomer(c));

            Console.WriteLine("Added: " + c);
        }

        static void FindCustomer()
        {
            //look at the Demo captuerd image and add your code here
            Console.Write("Enter CustomerId: ");
            var id = (Console.ReadLine() ?? "").Trim();

            
            Customer match = null;
            foreach (var kv in Customers)
            {
                if (string.Equals(kv.Key, id, StringComparison.Ordinal))
                {
                    match = kv.Value;
                    break;
                }
            }

            if (match == null)
            {
                Console.WriteLine("Invalid option.");
                return;
            }

            Console.WriteLine("Found: " + match);
        }

        static void CreateTicket()
        {
            //look at the Demo captuerd image and add your code here
            Console.Write("CustomerId: ");
            var id = (Console.ReadLine() ?? "").Trim();
            if (!Customers.ContainsKey(id))
            {
                Console.WriteLine("Not found.");
                return;
            }

            Console.Write("Subject: ");
            var subject = (Console.ReadLine() ?? "").Trim();
            if (string.IsNullOrWhiteSpace(subject))
            {
                Console.WriteLine("Invalid subject.");
                return;
            }

            var t = new Ticket(NextTicketId++, id, subject);
            Tickets.Enqueue(t);

          
            UndoStack.Push(new UndoCreateTicket(t));

            Console.WriteLine("Created ticket: " + t);
        }

        static void ServeNext()
        {
            //look at the Demo captuerd image and add your code here
            if (Tickets.Count == 0)
            {
                Console.WriteLine("No tickets.");
                return;
            }

            var t = Tickets.Dequeue();

           
            UndoStack.Push(new UndoServeTicket(t));

            Console.WriteLine("Served ticket: " + t);
        }

        static void ListCustomers()
        {
            Console.WriteLine("-- Customers --");
            //look at the Demo captuerd image and add your code here
            if (Customers.Count == 0)
            {
                Console.WriteLine("(none)");
                return;
            }

            foreach (var kvp in Customers)
            {
                Console.WriteLine(kvp.Value);
            }
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

            foreach (var t in Tickets)
            {
                Console.WriteLine(t);
            }
        }

        static void Undo()
        {
            //look at the Demo captuerd image and add your code here
            if (UndoStack.Count == 0)
            {
                Console.WriteLine("Nothing to undo.");
                return;
            }

            var last = UndoStack.Pop();

            if (last is UndoAddCustomer uAdd && uAdd.Customer != null)
            {
                if (Customers.Remove(uAdd.Customer.CustomerId))
                {
                    Console.WriteLine("Undo: Deleted customer " + uAdd.Customer.CustomerId);
                }
                else
                {
                    Console.WriteLine("Undo: Customer not found.");
                }
            }
            else if (last is UndoCreateTicket uCreate && uCreate.Ticket != null)
            {
                // remove that ticket from the queue
                var newQ = new Queue<Ticket>();
                bool removed = false;
                while (Tickets.Count > 0)
                {
                    var t = Tickets.Dequeue();
                    if (!removed && t.TicketId == uCreate.Ticket.TicketId)
                    {
                        removed = true; 
                        continue;
                    }
                    newQ.Enqueue(t);
                }
                Tickets = newQ;
                Console.WriteLine("Undo: Deleted ticket #" + uCreate.Ticket.TicketId);
            }
            else if (last is UndoServeTicket uServe && uServe.Ticket != null)
            {
                
                var newQ = new Queue<Ticket>();
                newQ.Enqueue(uServe.Ticket);
                while (Tickets.Count > 0) newQ.Enqueue(Tickets.Dequeue());
                Tickets = newQ;
                Console.WriteLine("Undo: Restored ticket #" + uServe.Ticket.TicketId);
            }
            else
            {
                Console.WriteLine("Undo: Unsupported action.");
            }
        }
    }
}