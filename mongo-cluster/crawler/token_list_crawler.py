from bs4 import PageElement

from crawler import BaseCrawler


class TokenListCrawler(BaseCrawler):
    BASE_URL = "https://coinmarketcap.com/vi/"

    def __init__(self, page: int = 1):
        super().__init__(f"{TokenListCrawler.BASE_URL}?page={page}")
        self.rows = self.base_soup.find("table").find("tbody").find_all("tr")
        self._extract_row(self.rows[0])

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
        return text_data

    def _extract_percent_cell(self, percent_content: PageElement):
        cell_soup = self.get_soup(str(percent_content))
        text_data = cell_soup.find("span").text
        _len = len(text_data)
        return float(text_data[0 : _len - 1]) * 100

    def _extract_volume_cell(self, volume_content: PageElement):
        cell_soup = self.get_soup(str(volume_content))
        text_data = cell_soup.find_all("p")
        fiat_data = text_data[0].text
        real_data = text_data[1].text
        return {"fiat": fiat_data, "real": real_data}

    def _extract_supply_cell(self, supply_content: PageElement):
        cell_soup = self.get_soup(str(supply_content))
        text_data = cell_soup.find("p")
        return text_data.text

    def _extract_row(self, row_content: PageElement):
        row_soup = self.get_soup(str(row_content))
        cells = row_soup.find_all("td")
        _name_data = self._extract_name_cell(cells[2])
        print(_name_data)
        _price_data = self._extract_price_cell(cells[3])
        print(_price_data)
        _one_hour = self._extract_percent_cell(cells[4])
        print(_one_hour)
        _ond_day = self._extract_percent_cell(cells[5])
        print(_ond_day)
        _seven_days = self._extract_percent_cell(cells[6])
        print(_seven_days)
        market_data = self._extract_price_cell(cells[7])
        print(market_data)
        volume_data = self._extract_volume_cell(cells[8])
        print(volume_data)
        supply_data = self._extract_supply_cell(cells[9])
        print(supply_data)
