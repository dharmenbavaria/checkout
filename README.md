
# Checkout Test

## Problem

Building a Payment Gateway
### Background
E-Commerce is experiencing exponential growth and merchants who sell their goods or
services online need a way to easily collect money from their customers.
We would like to build a payment gateway, an API based application that will allow a
merchant to offer away for their shoppers to pay for their product.
Processing a payment online involves multiple steps and entities:
- Shopper: Individual who is buying the product online.
- Merchant: The seller of the product. For example, Apple or Amazon.
- Payment Gateway: Responsible for validating requests, storing card information and forwarding payment requests and accepting payment responses to and from theacquiring bank.
- Acquiring Bank: Allows us to do the actual retrieval of money from the shopper’s card and payout to the merchant. It also performs some validation of the card information and then sends the payment details to the appropriate 3rd party organization for processing. We will be building the payment gateway only and simulating the acquiring bank component in order to allow us to fully test the payment flow. 
### Deliverables
- Build an API that allows a merchant
  - To process a payment through your payment gateway.
  - To retrieve details of a previously made payment.
- Build a simulator to mock the responses from the bank to test the API from your first deliverable.

## Solution

- Have used asp.net core 6.0 to create the APIs with C# and EF as ORM and Inmemory data base to store data
- Have used very simple onion architecture to keep things simple, and used Medaitor for simple commands and request handling.
- I have two main types of projects here
    - PaymentTransaction : Mostly used to maintain all the merchants payment transaction for payment gateway, it also validates card details etc.
    - Mock Bank: Which authenticates if the card is valid or not
- You can use swagger to looks for the APIs have implemented 3 of them 
  - Execute: Executing the payment with the all the details
  - Transaction by Id: Getting only one transaction by its Id
  - Merchants Transaction : Getting all the transactions of merchants.
- For successfull transaction enter valid card number and for invalid you can put cards which ends with 05, 12, 14 etc it will provide different reason of failing.
- SInce it already took more than 7 hours for me to implement i havent considered much of the bonus points although i did some logging, security, contanarization but nothing much fancy about it, I have tried to keep it simple
- If I would have more time
  - Would like to create api for mock bank instead of calling directly its services
  - Also add some more unit tests for data layer and api integration tests. 
  - Tried to create its pipeline for build and deploy
  - Could have added New Relic with docker so that we could do application performance monitoring with it.
- Execute API example
```
curl -X 'POST' \
  'https://localhost:44305/api/v1/PaymentTransaction/execute' \
  -H 'accept: text/plain; x-api-version=1.0' \
  -H 'Content-Type: application/json; x-api-version=1.0' \
  -d '{
  "merchantId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "cardDetails": {
    "cardHolderName": "dhamen",
    "cardNumber": "5241810400081568",
    "expirationMonth": "03",
    "expirationYear": "2025",
    "cvv": "123"
  },
  "amount": 40,
  "currency": "GBP"
}'
```   

- Merchants transaction api example calling
```
curl -X 'GET' \
  'https://localhost:44305/api/v1/PaymentTransaction/merchants/3fa85f64-5717-4562-b3fc-2c963f66afa6' \
  -H 'accept: text/plain; x-api-version=1.0'

```