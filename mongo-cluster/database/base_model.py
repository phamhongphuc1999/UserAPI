from database.base_connector import MongoBaseConnector, ConnectionOption


class MongoBaseModel:
    def __init__(self, connection: MongoBaseConnector = None, option: ConnectionOption = None):
        if connection:
            self.connection = connection
        elif option:
            self.connection = MongoBaseConnector(option)
        else:
            raise Exception("Missing connection option")
