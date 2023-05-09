from bots import BaseBot
from database.model_getter import ModelGetter
from fetcher.coin_market_cap_fetcher import CoinMarketCapFetcher
from services import app_logger
from services.token_refine_service import TokenRefineService


class CrawlDataBot(BaseBot):
    def __init__(self):
        super().__init__()
        self.fetcher = CoinMarketCapFetcher()

    def _run(self):
        try:
            token_data = self.fetcher.fetch_tokens()
            if token_data["status"]["error_code"] != 0:
                app_logger.error(token_data["status"])
            else:
                standard_data = TokenRefineService.standard_data(token_data["data"])
                bulk_data = TokenRefineService.standard_bulk_data(standard_data["token"])
                bulk_metadata = TokenRefineService.standard_bulk_metadata(standard_data["metadata"])
                _model = ModelGetter.Mongo.get_model()
                # _model.Token.update_bulk_metadata(bulk_data)
                _model.Metadata.update_bulk_metadata(bulk_metadata)
                print("Save done!")
        except Exception as error:
            app_logger.error(error)

    def run(self):
        # self.schedule_module.add_interval_job(
        #     func=self._run,
        #     seconds=10,
        #     args=[],
        #     id="crawl_data_bot",
        # )
        # self.schedule_module.start()
        self._run()
