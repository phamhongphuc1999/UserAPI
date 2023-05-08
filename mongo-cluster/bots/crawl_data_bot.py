from bots import BaseBot


def _run():
    print("123")


class CrawlDataBot(BaseBot):
    def __init__(self):
        super().__init__()

    def run(self):
        self.schedule_module.add_job(
            func=_run,
            trigger="interval",
            seconds=10,
            args=[],
            max_instances=2,
            id="crawl_data_bot",
        )
        self.schedule_module.start()
