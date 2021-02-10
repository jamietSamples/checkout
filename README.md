# Checkout.PaymentGateway

I have built a solution that meets the deliverables specified. There is a payment API that allows a user (merchant) to submit and retrieve a payment, as well as that i have included a payment methods controller that allows a user to view which payment methods (visa, amex, mastercard) are available for their given amount and currency.

The solution has been built with DDD in mind, i have also used a mediaor pattern combined with Command Query Seperation which could be useful for scaling.

The solution uses EF Core as an in memory database and seeds `PaymentMethod` objects on initialization.

I have implemented authentication in the form of an API Key, all requests will be required to use the key as part of a request headers. The key is fixed and just demonstrates authentication rather than applying it properly. The key is `specialKey123`

Solution wide the use of minor units is used, so for example if you were to set `"Amount":123` and `"CurrencyCode":"GBP"` this would equate to £1.23

Swagger is in place as an API client and comes with supporting comments on the responses and requests. The API itself used model attributes to validate an initial request inbound, that combined with the acquirer forms most of the validation for this solution.

To simulate the Acquirer i created a seperate project in the solution called `Checkout.PaymentGateway.Acquirer` which does some simple validation and either accepts or rejects the payment request.

The `Payment` object itself does not contain that much information in comparison to what you would find on a normal gateway request, its stripped back and achieves what it needs to.

Logging is written to console by serilog.

There is also docker support for the API and a dockerfile has been included to allow for an image to be built

## Payment Methods

This is an optional endpoint and allows for the retrieval ofa list of options available to a merchant. For example you may be submitting a large amount and a given payment type such as VISA would not support it. You can call this endpoint to verify the min / max amount for a given type.

Calling `/api/PaymentMethod` returns all payment methods

Calling `/api/PaymentMethod/{currencyCode}/{amount}` returns a filtered list

From experience with other gateways its not enforced but is prefered to check ahead and ensure youre able to make a payment

## Payments

Payments is the core functionality of this API, you create a payment request and are provided with a response containing the status, reason (if refused by acquirer) and a generated Id. With that Id you can retrieve all the details of the previously submitted payment.

I have also added a very crude implementation of Idempotency in the form of a `IdempotencyKey`. This key is optional and sent as a request header by the user, the solution will check for an existing key by the same name and ensure that the payment is not processed again. If the key is a match a conflict result is returned along with the previously created id. If the key doesn't exist a response is generated as expected.

## Improvements

Given time there are a number of things i would improve.

- Complete the check on payment type and validate against request values

- A purer DDD model, at the moment i have a lose implementation of AggergateRoot, BaseEntity and ValueObject. Given payment is fairly simple it doesnt contain much domain logic.

- Improved quality of testing around functionality

- Used some form of encryption or tokenisation between the gateway and the acquirer, or from the merchant to the gatewat but it seemed overkill for this.