from bson import ObjectId
from pymongo import UpdateOne
from pymongo.errors import BulkWriteError

from configs import AppConfig
from database.base_connector import MongoBaseConnector, ConnectionOption
from database.base_model import MongoBaseModel
from services import app_logger, convert_mongo_id


class TokenModel(MongoBaseModel):
    def __init__(self, connection: MongoBaseConnector = None, option: ConnectionOption = None):
        super().__init__(connection, option)
        _client = connection.get_connection()
        self.database = _client[AppConfig.Global.Mongo.DATABASE_NAME]
        self._token_collection = self.database[AppConfig.Global.Mongo.TOKEN_COLLECTION]

    def insert_metadata(self, insert_data):
        try:
            _id = self._token_collection.insert_one(insert_data).inserted_id
            _user_transaction = self._token_collection.find_one({"_id": ObjectId(_id)})
            return convert_mongo_id(_user_transaction)
        except Exception as error:
            app_logger.error(error)
        return None

    def upsert_metadata(self, query, upsert_data):
        try:
            upsert_result = self._token_collection.update_one(query, upsert_data, upsert=True)
            return {
                "id": upsert_result.upserted_id,
                "modified": upsert_result.modified_count,
                "matched": upsert_result.matched_count,
            }
        except Exception as error:
            app_logger.error(error)
        return None

    def update_bulk_metadata(self, bulk_data: list):
        try:
            _requests = []
            for item in bulk_data:
                _requests.append(UpdateOne(item["query"], {"$set": item["data"]}, upsert=True))
            _result = self._token_collection.bulk_write(_requests, ordered=False)
            return _result.bulk_api_result
        except BulkWriteError as bulk_error:
            app_logger.error(bulk_error.details)
        except Exception as error:
            app_logger.error(str(error))
        return None
