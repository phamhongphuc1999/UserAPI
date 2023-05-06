from configs.global_config import GlobalConfig


class AppConfig:
    def __init__(self):
        self.Global = GlobalConfig()


app_config = AppConfig()
