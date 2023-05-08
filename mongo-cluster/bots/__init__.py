from services.background_schedule_service import BackgroundScheduleService


class BaseBot:
    def __init__(self):
        self.schedule_module = BackgroundScheduleService()

    def run(self):
        pass
