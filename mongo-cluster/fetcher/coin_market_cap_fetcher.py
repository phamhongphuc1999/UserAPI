import json

from requests import Request, Session
from requests.exceptions import ConnectionError, Timeout, TooManyRedirects

from configs import app_config


class CoinMarketCapFetcher:
    BASE_URL = "https://pro-api.coinmarketcap.com/v1/cryptocurrency"

    def __init__(self):
        self.headers = {
            "Accepts": "application/json",
            "X-CMC_PRO_API_KEY": app_config.Global.coin_market_cap_key,
        }
        self.start = 1
        self.limit = 5000
        self.convert = "USD"

    def fetch_tokens(self):
        session = Session()
        session.headers.update(self.headers)
        try:
            response = session.get(
                f"{CoinMarketCapFetcher.BASE_URL}/listings/latest"
            )
            data = json.loads(response.text)
            for a in data["data"]:
                print(a['name'])
        except (ConnectionError, Timeout, TooManyRedirects) as error:
            print(error)
