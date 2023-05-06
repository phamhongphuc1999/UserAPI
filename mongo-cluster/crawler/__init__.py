from bs4 import BeautifulSoup
from selenium.webdriver import Chrome
from selenium.webdriver.chrome.options import Options


class BaseCrawler:
    def __init__(self, base_url: str):
        chrome_options = Options()
        chrome_options.add_argument("--headless")
        with Chrome(options=chrome_options) as browser:
            browser.get(base_url)
            html = browser.page_source
        self.base_soup = BeautifulSoup(html, "html.parser")

    @staticmethod
    def get_soup(content: str):
        return BeautifulSoup(content, "html.parser")
