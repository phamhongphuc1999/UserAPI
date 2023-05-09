from configs.constant import EnvironmentType
from configs.global_config import GlobalConfig
from configs.schema_config import SchemaConfig


class _AppConfig:
    def __init__(self):
        self.env = None
        self.Global = GlobalConfig()
        self.Schema = SchemaConfig()

    def init_env(self, _env: EnvironmentType, _key: str):
        self.env = _env
        self.Global.init_env(_env)
        self.Global.init_key(_key)


AppConfig = _AppConfig()
