from configs import AppConfig
from database.model_getter import ModelGetter
from fetcher.coin_market_cap_fetcher import CoinMarketCapFetcher

if __name__ == "__main__":
    ModelGetter.Mongo.connect()
    print("Done!")
    # AppConfig.Global.init_key("3a12fd86-a0e0-4385-8ff6-e1e88c78dad0")
    # fetcher = CoinMarketCapFetcher()
    # fetcher.fetch_tokens()
