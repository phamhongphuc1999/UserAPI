import requests
from bs4 import BeautifulSoup


class BaseCrawler:
    def __init__(self, base_url: str):
        headers = {
            "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36",
        }
        _text = requests.get(base_url, headers).text
        self.base_soup = BeautifulSoup(_text, "html.parser")

    @staticmethod
    def get_soup(content: str):
        return BeautifulSoup(content, "html.parser")
