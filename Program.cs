using Temporalio.Activity;
using Temporalio.Client;
using Temporalio.Samples.ActivityWorker;
using Temporalio.Worker;
using Temporalio.Workflow;

// Create a client to localhost on default namespace
var client = await TemporalClient.ConnectAsync(new("localhost:7233"));

// Create worker
using var worker = new TemporalWorker(
    client,
    new() { TaskQueue = "MessagingTaskQueue", Activities = { Activities.Slack } });

// Run worker until our code finishes
await worker.ExecuteAsync(async () =>
{
    // Run the workflow from Python. Since this is just a sample we will run the worker and the workflow
    // client here in the same process, but usually these are done separately.
    var result = await client.ExecuteWorkflowAsync(
        IMessagingWorkflow.Ref.RunAsync, 
        new Message{
            body="Hi", 
            media= MediaEnum.Slack, 
            receivers=new string [] {"Mahdi"}, 
            channel="Test Channel", 
            subject="This is the test slack notification"
            },
            // Note that the ID for each wokflow execution should be unique if you want to run multiple workflow
            // You can use receiver+Media as the workflow execution ID
        new() { ID = "MessagingWorkflow", TaskQueue = "MessagingTaskQueue" });

    Console.WriteLine("Workflow result: {0}", result);
});

namespace Temporalio.Samples.ActivityWorker
{
    public static class Activities
    {
        // Our activity implementation
        [Activity("SlackActivity")]
        public static string Slack(Message message){
                return  $"This is a Slack message to: {message.receivers.ToString()}.\n"+
                        $"Body: {message.body} \n", 
                        $"Sends from the {message.channel} channel";
                        }

        [Activity("EmailActivity")]
        public static string Email(Message message){
                return  $"This is an Email to: {message.receivers.ToString()}.\n"+
                        $"Body: {message.body} \n", 
                        $"Sends from the {message.channel} channel";
                        }

        [Activity("SmsActivity")]
        public static string Sms(Message message){
                return  $"This is a SMS to: {message.receivers.ToString()}.\n"+
                        $"Body: {message.body} \n", 
                        $"Sends from the {message.channel} channel";
                        }
    }

    // Workflow definition of the workflow implementation in Go
    [Workflow("MessagingWorkflow")]
    public interface IMessagingWorkflow
    {
        static readonly IMessagingWorkflow Ref = Refs.Create<IMessagingWorkflow>();

        [WorkflowRun]
        Task<string> RunAsync(Message message);
    }

public enum MediaEnum{
    Email = 1,
    SMS = 2,
    Slack = 3
}
    public class Message {
        public string body {get;set;}
        public string [] receivers{get;set;}
        public string subject {get;set;}
        public  MediaEnum media {get;set;}
        public string channel {get;set;}
    }
}
