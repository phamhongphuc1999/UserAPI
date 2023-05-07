from configs import AppConfig
from database.base_connector import MongoBaseConnector, ConnectionOption
from database.mongo_model import MongoModel


class _MongoGetter:
    def __init__(self):
        self._connection = None
        self._model = None

    def connect(self):
        if self._connection is None:
            if AppConfig.Global.Mongo.CONNECTION_STRING:
                self._connection = MongoBaseConnector(
                    ConnectionOption(
                        connection_string=AppConfig.Global.Mongo.CONNECTION_STRING,
                        database=AppConfig.Global.Mongo.DATABASE_NAME,
                    )
                )
            else:
                host = AppConfig.Global.Mongo.HOST
                port = AppConfig.Global.Mongo.PORT
                username = AppConfig.Global.Mongo.USER_NAME
                password = AppConfig.Global.Mongo.PASSWORD
                database = AppConfig.Global.Mongo.DATABASE_NAME
                self._connection = MongoBaseConnector(
                    ConnectionOption(
                        host=host,
                        port=port,
                        username=username,
                        password=password,
                        database=database,
                    )
                )

    def get_model(self):
        if self._connection is None:
            raise Exception("Database is disconnected")
        if self._model is None:
            self._model = MongoModel(connection=self._connection)
        return self._model


class _ModelGetter:
    Mongo = _MongoGetter()


ModelGetter = _ModelGetter()
