# CurrencyAssesmentTask
# Exchange Rate Managing API
This Api are designed for manging exchange rate for various currencies.  

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Testing](#testing)
- [Contributing](#contributing)
- [License](#license)

## Overview

The Exchange Rate Management API allows users to manage and retrieve exchange rates and perform currency conversions. It is designed for financial applications where handling real-time or historical exchange rates is required.

- This API provides endpoints to manage currency exchange rates and perform currency conversions


## Features
This project includes three main API endpoints:

- Get all exchange rates for a base currency: Retrieve a list of exchange rates for various currencies relative to a given base currency.
- Endpoint :https://hosturl/api/basecurrency
- example https://localhost:44393/api/rates/EUR
- Convert between two currencies: Calculate the conversion rate between two currencies based on the latest exchange rates.
- Endpoint: https://hosturl/api/rates/convert/amount/fromcurrency/tocurrency
- example https://localhost:44393/api/rates/convert/1/EUR/GBP
- Get historical exchange rates: Retrieve the exchange rate history between two dates for a specific base currency. Pagination is supported, and up to a maximum of 100 days of exchange rate data can be extracted at a time.
- Endpoint: https://hosturl/api/rates/rates/history/basecurrency/fromdate>..todate/noofdaysinpage/startingpage
- example" https://localhost:44393/api/rates/history/EUR/2024-01-01/2024-08-31/20/1

## Installation

### Prerequisites
- Software dependencies (.net core 6.0).
