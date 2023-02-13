using System; // Namespace for Console output
using System.Configuration; // Namespace for ConfigurationManager
using System.Threading.Tasks; // Namespace for Task
using Azure.Identity;
using Azure.Storage.Queues; // Namespace for Queue storage types
using Azure.Storage.Queues.Models; // Namespace for PeekedMessage

namespace AzureStorageQueue
{
    class Program
    {

        static void Main(string[] args)
        {
            var queueName = "myqueue-" + DateTime.Today.ToString("yyyyMMddHH");
            CreateQueue(queueName);
            for (int n = 0; n < 10; n++)
            {
                var msg = "Hello, World!" + n;
                InsertMessage(queueName, msg);
                Console.WriteLine($"message : '{msg}' inserted.");
            }

            Console.WriteLine("press any key to read message...");
            Console.ReadKey();

            do
            {
                var msg = DequeueMessage(queueName);
                Console.WriteLine($"message : {msg}");

            } while (PeekNextMessage(queueName));

            Console.WriteLine("press any key to exit...");
            Console.ReadKey();
        }

        //-------------------------------------------------
        // Create the queue service client
        //-------------------------------------------------
        public static void CreateQueueClient(string queueName)
        {
            // Get the connection string from app settings
            string connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

            // Instantiate a QueueClient which will be used to create and manipulate the queue
            QueueClient queueClient = new QueueClient(connectionString, queueName);
        }

        //-------------------------------------------------
        // Create a message queue
        //-------------------------------------------------
        public static bool CreateQueue(string queueName)
        {
            try
            {
                // Get the connection string from app settings
                string connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

                // Instantiate a QueueClient which will be used to create and manipulate the queue
                QueueClient queueClient = new QueueClient(connectionString, queueName);

                // Create the queue
                queueClient.CreateIfNotExists();

                if (queueClient.Exists())
                {
                    Console.WriteLine($"Queue created: '{queueClient.Name}'");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Queue {queueName} cannot found.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}\n\n");
                Console.WriteLine($"Make sure the Azurite storage emulator running and try again.");
                return false;
            }
        }
        //-------------------------------------------------
        // Insert a message into a queue
        //-------------------------------------------------
        public static void InsertMessage(string queueName, string message)
        {
            // Get the connection string from app settings
            string connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

            // Instantiate a QueueClient which will be used to create and manipulate the queue
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            // Create the queue if it doesn't already exist
            queueClient.CreateIfNotExists();

            if (queueClient.Exists())
            {
                // Send a message to the queue
                queueClient.SendMessage(message);
            }

        }

        //-------------------------------------------------
        // Peek at a message in the queue
        //-------------------------------------------------
        public static bool PeekNextMessage(string queueName)
        {
            // Get the connection string from app settings
            string connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

            // Instantiate a QueueClient which will be used to manipulate the queue
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            if (queueClient.Exists())
            {
                // Peek at the next message
                PeekedMessage[] peekedMessage = queueClient.PeekMessages();

                if (peekedMessage.Length > 0) return true;
            }
            return false;
        }

        //-------------------------------------------------
        // Process and remove a message from the queue
        //-------------------------------------------------
        public static string DequeueMessage(string queueName)
        {
            // Get the connection string from app settings
            string connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

            // Instantiate a QueueClient which will be used to manipulate the queue
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            if (queueClient.Exists())
            {
                // Get the next message
                QueueMessage[] retrievedMessage = queueClient.ReceiveMessages();

                // Process (i.e. print) the message in less than 30 seconds
                var msg = retrievedMessage[0].Body.ToString();

                // Delete the message
                queueClient.DeleteMessage(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
                return msg;
            }
            return "";
        }
    }
}
