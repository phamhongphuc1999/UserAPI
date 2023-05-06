from typing import List

from bs4 import PageElement

from crawler import BaseCrawler
from services import convert_comma_to_number
from services.logger_service import app_logger


class TokenRow:
    def __init__(
        self,
        rank: str,
        img_url: str,
        name: str,
        symbol: str,
        price: str,
        one_hour: str,
        one_day: str,
        seven_days: str,
        market_cap: str,
        fiat_volume: str,
        volume: str,
        supply: str,
    ):
        self.rank = rank
        self.img_url = img_url
        self.name = name
        self.symbol = symbol
        self.price = price
        self.one_hour = one_hour
        self.one_day = one_day
        self.seven_days = seven_days
        self.market_cap = market_cap
        self.fiat_volume = fiat_volume
        self.volume = volume
        self.supply = supply


class TokenListCrawler(BaseCrawler):
    BASE_URL = "https://coinmarketcap.com"

    def __init__(self, page: int = 1):
        super().__init__(f"{TokenListCrawler.BASE_URL}?page={page}")
        self.rows = self.base_soup.find("table").find("tbody").find_all("tr")
        self.tokens: List[TokenRow] = []
        for row in self.rows:
            try:
                self.tokens.append(self._extract_row(row))
            except:
                app_logger.error("Something wrong")

    def _extract_rank_cell(self, rank_content: PageElement):
        cell_soup = self.get_soup(str(rank_content))
        print(cell_soup)
        rank = cell_soup.find("p").text
        return rank

    def _extract_name_cell(self, name_content: PageElement):
        cell_soup = self.get_soup(str(name_content))
        img_url = cell_soup.find("img").attrs["src"]
        text_data = cell_soup.find_all("p")
        return {
            "image": img_url,
            "name": text_data[0].text.lower(),
            "symbol": text_data[1].text.lower(),
        }

    def _extract_price_cell(self, price_content: PageElement):
        cell_soup = self.get_soup(str(price_content))
        text_data = cell_soup.find("span").text
        return convert_comma_to_number(text_data[1:])

    def _extract_percent_cell(self, percent_content: PageElement):
        cell_soup = self.get_soup(str(percent_content))
        text_data = cell_soup.find("span").text
        _len = len(text_data)
        return text_data[0 : _len - 1]

    def _extract_volume_cell(self, volume_content: PageElement):
        cell_soup = self.get_soup(str(volume_content))
        text_data = cell_soup.find_all("p")
        fiat_data = text_data[0].text
        real_data = text_data[1].text.split(" ")
        return {
            "fiat": convert_comma_to_number(fiat_data[1:]),
            "real": convert_comma_to_number(real_data[0]),
        }

    def _extract_supply_cell(self, supply_content: PageElement):
        cell_soup = self.get_soup(str(supply_content))
        text_data = cell_soup.find("p").text.split(" ")
        return convert_comma_to_number(text_data[0])

    def _extract_row(self, row_content: PageElement) -> TokenRow:
        row_soup = self.get_soup(str(row_content))
        cells = row_soup.find_all("td")
        rank = self._extract_rank_cell(cells[1])
        name = self._extract_name_cell(cells[2])
        price = self._extract_price_cell(cells[3])
        one_hour = self._extract_percent_cell(cells[4])
        one_day = self._extract_percent_cell(cells[5])
        seven_days = self._extract_percent_cell(cells[6])
        market_cap = self._extract_price_cell(cells[7])
        volume = self._extract_volume_cell(cells[8])
        supply = self._extract_supply_cell(cells[9])
        return TokenRow(
            rank=rank,
            img_url=name["image"],
            name=name["name"],
            symbol=name["symbol"],
            price=price,
            one_hour=one_hour,
            one_day=one_day,
            seven_days=seven_days,
            market_cap=market_cap,
            fiat_volume=volume["fiat"],
            volume=volume["real"],
            supply=supply,
        )
