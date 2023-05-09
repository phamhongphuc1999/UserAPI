from configs import AppConfig
from services import validate_json_schema


class TokenRefineService:
    @staticmethod
    def convert_quote_to_string(quote):
        result = {}
        for key in quote:
            data = quote[key]
            result[key] = {
                "price": str(data.get("price")),
                "volume_24h": str(data.get("volume_24h")),
                "volume_change_24h": str(data.get("volume_change_24h")),
                "volume_24h_reported": str(data.get("volume_24h_reported")),
                "volume_7d": str(data.get("volume_7d")),
                "volume_7d_reported": str(data.get("volume_7d_reported")),
                "volume_30d": str(data.get("volume_30d")),
                "volume_30d_reported": str(data.get("volume_30d_reported")),
                "market_cap": str(data.get("market_cap")),
                "market_cap_dominance": str(data.get("market_cap_dominance")),
                "fully_diluted_market_cap": str(data.get("fully_diluted_market_cap")),
                "tvl": str(data.get("tvl")),
                "percent_change_1h": str(data.get("percent_change_1h")),
                "percent_change_24h": str(data.get("percent_change_24h")),
                "percent_change_7d": str(data.get("percent_change_7d")),
                "last_updated": str(data.get("last_updated")),
            }
        return result

    @staticmethod
    def standard_data(raw_data: list):
        token_data = []
        token_metadata = []
        for token in raw_data:
            if validate_json_schema(token, AppConfig.Schema.TOKEN_DATA_SCHEMA):
                token_data.append(
                    {
                        "id": str(token.get("id")),
                        "cmc_rank": str(token.get("cmc_rank")),
                        "num_market_pairs": str(token.get("num_market_pairs")),
                        "circulating_supply": str(token.get("circulating_supply")),
                        "total_supply": str(token.get("total_supply")),
                        "market_cap_by_total_supply": str(token.get("market_cap_by_total_supply")),
                        "max_supply": str(token.get("max_supply")),
                        "infinite_supply": str(token.get("infinite_supply")),
                        "last_updated": str(token.get("last_updated")),
                        "self_reported_circulating_supply": str(token.get("self_reported_circulating_supply")),
                        "self_reported_market_cap": str(token.get("self_reported_market_cap")),
                        "tvl_ratio": str(token.get("tvl_ratio")),
                        "quote": TokenRefineService.convert_quote_to_string(token.get("quote")),
                    }
                )
                token_metadata.append(
                    {
                        "id": str(token.get("id")),
                        "name": str(token.get("name")),
                        "symbol": str(token.get("symbol")),
                        "slug": str(token.get("slug")),
                        "date_added": str(token.get("date_added")),
                        "tags": token.get("tags"),
                        "platform": token.get("platform"),
                    }
                )
        return {"token": token_data, "metadata": token_metadata}

    @staticmethod
    def standard_bulk_data(token_data: list):
        bulk_data = []
        for token in token_data:
            bulk_data.append(
                {
                    "query": {
                        "id": token["id"],
                        "last_updated": token["last_updated"],
                    },
                    "data": token,
                }
            )
        return bulk_data

    @staticmethod
    def standard_bulk_metadata(token_data: list):
        bulk_data = []
        for token in token_data:
            bulk_data.append({"query": {"id": token["id"]}, "data": token})
        return bulk_data
