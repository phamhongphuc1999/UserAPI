import json

from requests import Session
from requests.exceptions import ConnectionError, Timeout, TooManyRedirects

from configs import AppConfig
from services.logger_service import app_logger


class CoinMarketCapFetcher:
    BASE_URL = "https://pro-api.coinmarketcap.com"

    def __init__(self):
        self.headers = {
            "Accepts": "application/json",
            "X-CMC_PRO_API_KEY": AppConfig.Global.coin_market_cap_key,
        }

    def fetch_tokens(self, start=1, limit=5000, convert="USD"):
        session = Session()
        session.headers.update(self.headers)
        try:
            response = session.get(
                f"{CoinMarketCapFetcher.BASE_URL}/v1/cryptocurrency/listings/latest?start={start}&limit={limit}&convert={convert}"
            )
            data = json.loads(response.text)
            return data
        except (ConnectionError, Timeout, TooManyRedirects) as error:
            app_logger.error(error)

    def fetch_metadata(self):
        session = Session()
        session.headers.update(self.headers)
        try:
            response = session.get(f"{CoinMarketCapFetcher}/v2/cryptocurrency/info")
            data = json.loads(response.text)
            return data
        except (ConnectionError, Timeout, TooManyRedirects) as error:
            app_logger.error(error)
