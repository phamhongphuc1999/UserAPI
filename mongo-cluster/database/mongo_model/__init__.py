from database.base_connector import MongoBaseConnector, ConnectionOption
from database.mongo_model.metadata_model import MetadataModel
from database.mongo_model.token_model import TokenModel


class MongoModel:
    def __init__(self, connection: MongoBaseConnector = None, option: ConnectionOption = None):
        self.Metadata = MetadataModel(connection=connection, option=option)
        self.Token = TokenModel(connection=connection, option=option)
