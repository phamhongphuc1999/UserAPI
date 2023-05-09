from configs.constant import EnvironmentType


class GlobalConfig:
    def __init__(self):
        self.coin_market_cap_key = ""
        self.Mongo = self._Mongo()

    def init_key(self, coin_market_cap_key: str):
        self.coin_market_cap_key = coin_market_cap_key

    def init_env(self, _env: EnvironmentType):
        self.Mongo.init_env(_env)

    class _Mongo:
        def __init__(self):
            self.PORT = 27017
            self.USER_NAME = "root"
            self.PASSWORD = "root"
            self.DATABASE_NAME = "MyDatabase"
            self.HOST = "127.0.0.1"
            self.CONNECTION_STRING = "mongodb://127.0.0.1:27117,127.0.0.1:27118"

            self.METADATA_COLLECTION = "metadata"
            self.TOKEN_COLLECTION = "token"

        def init_env(self, _env: EnvironmentType):
            if _env == EnvironmentType.DEVELOPMENT:
                self.HOST = "127.0.0.1"
                self.CONNECTION_STRING = "mongodb://127.0.0.1:27117,127.0.0.1:27118"
            elif _env == EnvironmentType.PRODUCTION:
                self.HOST = "127.0.0.1"
                self.CONNECTION_STRING = "123"
