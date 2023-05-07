TOKEN_DATA = {
    "type": "object",
    "properties": {
        "id": {"type": "number"},
        "name": {"type": "string"},
        "symbol": {"type": "string"},
        "slug": {"type": "string"},
        "cmc_rank": {"type": "number"},
        "num_market_pairs": {"type": "number"},
        "circulating_supply": {"type": "number"},
        "total_supply": {"type": "number"},
        "market_cap_by_total_supply": {"type": "number"},
        "max_supply": {"type": "number"},
        "infinite_supply": {"type": "boolean"},
        "last_updated": {"type": "string"},
        "date_added": {"type": "string"},
        "tags": {"type": "array", "items": {"type": "string"}},
        "self_reported_circulating_supply": {"type": "number"},
        "self_reported_market_cap": {"type": "number"},
        "tvl_ratio": {"type": "number"},
        "platform": {
            "type": "object",
            "properties": {
                "id": {"type": "number"},
                "name": {"type": "string"},
                "symbol": {"type": "string"},
                "slug": {"type": "string"},
                "token_address": {"type": "string"},
            },
        },
        "quote": {
            "type": "object",
            "patternProperties": {
                "^.*$": {
                    "type": "object",
                    "properties": {
                        "price": {"type": "number"},
                        "volume_24h": {"type": "number"},
                        "volume_change_24h": {"type": "number"},
                        "volume_24h_reported": {"type": "number"},
                        "volume_7d": {"type": "number"},
                        "volume_7d_reported": {"type": "number"},
                        "volume_30d": {"type": "number"},
                        "volume_30d_reported": {"type": "number"},
                        "market_cap": {"type": "number"},
                        "market_cap_dominance": {"type": "number"},
                        "fully_diluted_market_cap": {"type": "number"},
                        "tvl": {"type": "number"},
                        "percent_change_1h": {"type": "number"},
                        "percent_change_24h": {"type": "number"},
                        "percent_change_7d": {"type": "number"},
                        "last_updated": {"type": "string"},
                    },
                }
            },
        },
    },
}

TOKEN_METADATA = {
    "type": "object",
    "properties": {
        "id": {"type": "number"},
        "name": {"type": "string"},
        "symbol": {"type": "string"},
        "category": {"type": "string"},
        "slug": {"type": "string"},
        "logo": {"type": "string"},
        "description": {"type": "string"},
        "date_added": {"type": "string"},
        "date_launched": {"type": "string"},
        "notice": {"type": "string"},
        "tags": {"type": "array", "items": {"type": "string"}},
        "platform": {
            "type": "object",
            "properties": {
                "id": {"type": "number"},
                "name": {"type": "string"},
                "symbol": {"type": "string"},
                "slug": {"type": "string"},
                "token_address": {"type": "string"},
            },
        },
        "self_reported_circulating_supply": {"type": "number"},
        "self_reported_market_cap": {"type": "number"},
        "self_reported_tags": {"type": "object"},
        "infinite_supply": {"type": "boolean"},
        "urls": {
            "type": "object",
            "properties": {
                "website": {"type": "array", "items": {"type": "string"}},
                "technical_doc": {"type": "array", "items": {"type": "string"}},
                "explorer": {"type": "array", "items": {"type": "string"}},
                "source_code": {"type": "array", "items": {"type": "string"}},
                "message_board": {"type": "array", "items": {"type": "string"}},
                "chat": {"type": "array", "items": {"type": "string"}},
                "announcement": {"type": "array", "items": {"type": "string"}},
                "reddit": {"type": "array", "items": {"type": "string"}},
                "twitter": {"type": "array", "items": {"type": "string"}},
            },
        },
    },
}


class SchemaConfig:
    TOKEN_DATA_SCHEMA = TOKEN_DATA
    TOKEN_METADATA_SCHEMA = TOKEN_DATA_SCHEMA
