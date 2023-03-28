import asyncio
from datetime import timedelta
from dataclasses import dataclass
from enum import Flag, IntEnum, auto
from temporalio import workflow
from temporalio.client import Client
from temporalio.worker import Worker

TASK_QUEUE = "MessagingTaskQueue"
WORKFLOW_NAME = "MessagingWorkflow"

class MessagingActivityEnum(Flag):
    EmailActivity = auto()
    SmsActivity = auto()
    SlackActivity = auto()



class MediaEnum(IntEnum):
    Email = 1
    SMS = 2
    Slack = 3


@dataclass
class Message:
    body: str
    receivers: list[str]
    subject: str
    media: MediaEnum
    channel: str

# Basic workflow that logs and invokes an activity


@workflow.defn
class MessagingWorkflow:
    @workflow.run
    async def run(self, message: Message) -> str:

        workflow.logger.info("Running workflow with parameter %s" % message)
        if message.media is MediaEnum.Email:
            return await workflow.execute_activity(
                MessagingActivityEnum.EmailActivity.name,
                message,
                schedule_to_close_timeout=timedelta(seconds=5)
            )
        elif message.media is MediaEnum.SMS:
            return await workflow.execute_activity(
                MessagingActivityEnum.SmsActivity.name,
                message,
                schedule_to_close_timeout=timedelta(seconds=5)
            )
            
        elif message.media is MediaEnum.Slack:
            return await workflow.execute_activity(
                MessagingActivityEnum.SlackActivity.name,
                message,
                schedule_to_close_timeout=timedelta(seconds=5)
            )
        else:
             raise ValueError(f"Unrecognized media: {message.media}")


async def main(event: asyncio.Event):
    # Uncomment the line below to see logging
    # logging.basicConfig(level=logging.INFO)
    # Start client
    client = await Client.connect("localhost:7233")

    # Run a worker for the workflow
    async with Worker(
            client=client,
            task_queue=TASK_QUEUE,
            identity="MessagingWorker",
            workflows=[MessagingWorkflow],
            no_remote_activities=True):
        await event.wait()

if __name__ == "__main__":
    # pass an event to the main function
    event = asyncio.Event()
    asyncio.run(main(event))
