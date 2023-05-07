from pymongo import MongoClient

from services import app_logger


class ConnectionOption:
    def __init__(
        self,
        host: str = None,
        port: str = None,
        username: str = None,
        password: str = None,
        database: str = None,
        connection_string: str = None,
    ):
        self.host = host
        self.port = port
        self.username = username
        self.password = password
        self.database = database
        self.connection_string = connection_string


class BaseConnector:
    def __init__(self, option: ConnectionOption):
        self.option = option


class MongoBaseConnector(BaseConnector):
    def __init__(self, option: ConnectionOption):
        super().__init__(option)
        self._connect()

    def _connect(self):
        try:
            if self.option.connection_string:
                self._connection = MongoClient(self.option.connection_string)
                if self._connection.server_info():
                    app_logger.info(
                        f"Connected to database: {self.option.connection_string}"
                    )
                else:
                    raise Exception("Connection to mongodb fail")
            else:
                host = self.option.host
                port = self.option.port
                username = self.option.username
                password = self.option.password
                self._connection = MongoClient(
                    host=host, port=port, username=username, password=password
                )
                if self._connection.server_info():
                    app_logger.info(f"Connected to database: {host}:{port}")
                else:
                    raise Exception("Connection to mongodb fail")
        except Exception as err:
            raise err

    def reconnect(self):
        self._connect()

    def get_connection(self) -> MongoClient:
        return self._connection
