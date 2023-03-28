# Temporal C# Example
This is an example repository that demonstrates how to use the Temporal SDK in a C# project. Unfortunately, current version of C# SDK of Temporal doesn't supports workflows and we have to use another language that have a complete SDK, in this project I'm using Python.

## Getting Started
To get started with this repository, you will need to have the following prerequisites installed:
- .NET 6 SDK or later
- [Temporal CLI](https://docs.temporal.io/application-development/foundations#run-a-development-server)
- [Temporal .NET SDK](https://github.com/temporalio/sdk-dotnet/)
- [Temporal Python SDK](https://github.com/temporalio/sdk-python)

Once you have the prerequisites installed, you can clone this repository to your local machine:
`git clone https://github.com/mahdiAkhi/TemporalSample.git`

Next, you will need to start the Temporal server by running the following command:
`temporal server start-dev`
This command automatically starts the Web UI, creates the default Namespace, and uses an in-memory database. The Temporal Server should be available on localhost:7233 and the Temporal Web UI should be accessible at [http://localhost:8233](http://localhost:8233).