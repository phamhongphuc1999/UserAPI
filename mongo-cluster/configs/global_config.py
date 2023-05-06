class GlobalConfig:
    def __init__(self):
        self.coin_market_cap_key = ""

    def init_key(self, coin_market_cap_key: str):
        self.coin_market_cap_key = coin_market_cap_key
