import signal

from bots.crawl_data_bot import CrawlDataBot
from configs import AppConfig
from database.model_getter import ModelGetter
from fetcher.coin_market_cap_fetcher import CoinMarketCapFetcher
from services import get_env
from services.token_refine_service import TokenRefineService

if __name__ == "__main__":
    _env = get_env()
    AppConfig.init_env(_env, "abc")
    ModelGetter.Mongo.connect()
    print("Done!")

    _bot = CrawlDataBot()
    _bot.run()
    # signal.pause()
