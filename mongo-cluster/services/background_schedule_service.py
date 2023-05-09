import tzlocal
from apscheduler.schedulers.background import BackgroundScheduler


class BackgroundScheduleService(BackgroundScheduler):
    def __init__(self, **options):
        super().__init__(timezone=str(tzlocal.get_localzone()), **options)

    def add_interval_job(self, seconds: int, **kwargs):
        self.add_job(trigger="interval", seconds=seconds, max_instances=2, **kwargs)

    def add_cron_job(self, hour: str, minute: str, **kwargs):
        self.add_job(trigger="cron", hour=hour, minute=minute, **kwargs)
