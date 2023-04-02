# Temporal C# Example
This is an example repository that demonstrates how to use the Temporal SDK in a C# project. Unfortunately, current version of C# SDK of Temporal doesn't supports workflows and I to use another language that have a complete SDK, in this project I'm using Python.

## Getting Started
To get started with this repository, you will need to have the following prerequisites installed:
- .NET 6 SDK or later
- [Temporal CLI](https://docs.temporal.io/application-development/foundations#run-a-development-server)
- [Temporal .NET SDK](https://github.com/temporalio/sdk-dotnet/)
- [Temporal Python SDK](https://github.com/temporalio/sdk-python)

Once you have the prerequisites installed, you can clone this repository to your local machine:
`git clone https://github.com/mahdiAkhi/Temporal-Dotnet-Python.git`

Next, you will need to start the Temporal server by running the following command:
`temporal server start-dev`
This command automatically starts the Web UI, creates the default Namespace, and uses an in-memory database. The Temporal Server should be available on localhost:7233 and the Temporal Web UI should be accessible at [http://localhost:8233](http://localhost:8233).

## Project Scenario
We are going to implement a messagin system using Temporal. This system sends the messages through the below media:
- Email
- SMS
- Slack message

The messaging services are writen in C#(.NET). We don't want to implement the complete logic of these services. we just have three functions(service) like below to send email, SMS, or Slack message.

```
 public static string Email(Message message){
            return "This is an email to: {message.receivers.ToString()}.\n"+
                  $"Body: {message.body} \n", 
                  $"Sends from the {message.channel} channel";
        }
        
public static string SMS(Message message){
            return "This is a SMS to: {message.receivers.ToString()}.\n"+
                  $"Body: {message.body} \n", 
                  $"Sends from the {message.channel} channel";
        }
        
public static string Slack(Message message){
            return "This is a Slack message to: {message.receivers.ToString()}.\n"+
                  $"Body: {message.body} \n", 
                  $"Sends from the {message.channel} channel";
        }```
        
We know for sending messages we need to call a third-party service(API call). This can fail due to lots of reasons such as network issues, API token expiration, etc. 
Also, calling third-party services using API always is a time-consuming statement. Imagine you need to send both email and SMS for new users and you have an internal Slack channel that is used to notify the sales team of new user registers. In such a scenario your system sends three messages and needs time to get a successful response from the APIs. What happens if one of these messages fails? How do you try to resend it? How many times do you want to try? if you have more messages to send, how much time is spent to complete a user registration?
To address this issues we can use Temporal.
[Temporal](https://temporal.io) is a developer-first, open-source platform that ensures the successful execution of services and applications. Also, Temporal is an orchestration tool that manages the sequence of services in a workflow. Here is some ability of Temporal that can help us:

- Ability to define the services in the workflows. It helps us with orchestration and lets us run the services in turn.
- It guarantees that the activities run till they ended successfully. Therefore, we can define different policies to handle task failures.
- Run the services asynchronously.

## Design System using Temporal
In this section we are going to design our messaging system using Temporal. First of all, we need to learn the key concepts and components of the Temporal. Here we go!

### Workflow
The core component of the Temporal is Workflow.  A workflow represents a set of coordinated tasks(activities/services) that are executed in a specific order to achieve a particular goal. In simple language, a workflow is a function that manages the activities. It manages the order of the activities, the precondition to run an activity, retry policies, etc. A critical aspect of developing Workflow Definitions is ensuring they exhibit certain deterministic traits – that is, making sure that the same Commands are emitted in the same sequence, whenever a corresponding Workflow Function Execution is re-executed.
A Workflow Definition is the code that defines the constraints of a Workflow Execution. A Workflow Definition is often also referred to as a Workflow Function. [Here you can see how to develop a workflow in different languages](https://docs.temporal.io/application-development/foundations#develop-workflows).
Unfortunately, the current version of the [dotnet SDK](https://github.com/temporalio/sdk-dotnet) doesn't support defining a workflow. To address this challenge we use Python.

### Activity
A workflow is a chain of one or more activities. Activities are individual tasks(services) that are executed by the workflow. They are typically short-lived, stateless, and perform a specific action such as reading from a database or sending an email. Activities are functions that encapsulate logic that can potentially fail, such as network calls, file operations, or random number generation. Activities are invoked in Workflow code and the Server coordinates with the application to execute them. The current version of [dotnet SDK](https://github.com/temporalio/sdk-dotnet) supports the activity.


You can see many different examples in different scenarios in the [Python code samples](https://github.com/temporalio/samples-python) to get more familiar with Temporal components. Also, the [dotnet code samples](https://github.com/temporalio/samples-dotnet) are here.
