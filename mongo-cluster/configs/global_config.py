class GlobalConfig:
    def __init__(self):
        self.coin_market_cap_key = ""
        self.Mongo = self._Mongo()

    def init_key(self, coin_market_cap_key: str):
        self.coin_market_cap_key = coin_market_cap_key

    class _Mongo:
        def __init__(self):
            self.PORT = 27017
            self.USER_NAME = "root"
            self.PASSWORD = "sanic"
            self.DATABASE_NAME = "MyDatabase"
            self.HOST = "127.0.0.1"
            self.CONNECTION_STRING = "mongodb://127.0.0.1:27117,127.0.0.1:27118"

            self.METADATA_COLLECTION = "metadata"
            self.TOKEN_COLLECTION = "token"
