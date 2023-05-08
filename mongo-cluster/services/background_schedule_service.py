import tzlocal
from apscheduler.schedulers.background import BackgroundScheduler


class BackgroundScheduleService(BackgroundScheduler):
    def __init__(self, **options):
        super().__init__(timezone=str(tzlocal.get_localzone()), **options)
