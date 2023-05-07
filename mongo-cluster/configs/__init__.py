from configs.global_config import GlobalConfig
from configs.schema_config import SchemaConfig


class _AppConfig:
    def __init__(self):
        self.Global = GlobalConfig()
        self.Schema = SchemaConfig()


AppConfig = _AppConfig()
