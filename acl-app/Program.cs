using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.Security.ConfidentialLedger;
using Azure.Security.ConfidentialLedger.Certificate;
using System;
using System.Text;
using System.Text.Json;

namespace acl_app
{
    class Program
    {
        static Task Main(string[] args)
        {
            //https://ledgerjoseurl2023.confidential-ledger.azure.com
            // Replace with the name of your confidential ledger            
            const string ledgerName = "ledgerjoseurl2023";
            var ledgerUri = $"https://{ledgerName}.confidential-ledger.azure.com";
            Console.WriteLine($"*** LAB Blockchain ***\n\tJosé Vinicio De León - 1072619 \n\t{ledgerUri}\n");

            // Create a confidential ledger client using the ledger URI and DefaultAzureCredential

            var ledgerClient = new ConfidentialLedgerClient(new Uri(ledgerUri), new DefaultAzureCredential());

            // Write to the ledger
            Console.WriteLine($"Writing in ledger...");
            Operation postOperation = ledgerClient.PostLedgerEntry(
                waitUntil: WaitUntil.Completed,
                RequestContent.Create(
                    new { contents = "Hello world!" }));

            // Access the transaction ID of the ledger write

            string transactionId = postOperation.Id;
            Console.WriteLine($"Appended transaction with Id: {transactionId}");


            // Control the status of transaction
            Response ledgerResponse = ledgerClient.GetLedgerEntry(transactionId);
            JsonElement result = JsonDocument.Parse(ledgerResponse.ContentStream).RootElement;
            Console.WriteLine("\t" + result.GetProperty("state").ToString());

            Response ledgerResponse2 = ledgerClient.GetLedgerEntry(transactionId);
            JsonElement result2 = JsonDocument.Parse(ledgerResponse2.ContentStream).RootElement;
            Console.WriteLine("\t" + result2.GetProperty("state").ToString());

            JsonElement result3 = JsonDocument.Parse(postOperation.WaitForCompletionResponse().ContentStream).RootElement;
            Console.WriteLine("\t" + result3.GetProperty("state").ToString());

            Console.ReadLine();
            return default;
        }
    }
}
