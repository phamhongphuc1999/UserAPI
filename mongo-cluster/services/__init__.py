import sys

from jsonschema.exceptions import ValidationError
from jsonschema.validators import validate

from configs.constant import EnvironmentType
from services.logger_service import app_logger


def convert_comma_to_number(num: str):
    result = ""
    for i in num:
        if i != ",":
            result += i
    return result


def validate_json_schema(data: object, json_schema: dict):
    try:
        validate(data, json_schema)
        return True
    except ValidationError as error:
        app_logger.error(error)
        return False


def convert_mongo_id(mongo_doc):
    if mongo_doc is not None:
        if "_id" in mongo_doc:
            mongo_doc.update({"id": str(mongo_doc.pop("_id"))})
    return mongo_doc


def get_env():
    _args = sys.argv
    if len(_args) < 2:
        raise Exception("environment is required")
    elif _args[1] == "development":
        return EnvironmentType.DEVELOPMENT
    elif _args[1] == "production":
        return EnvironmentType.PRODUCTION
    else:
        raise Exception(f"Not found {_args[1]}")
