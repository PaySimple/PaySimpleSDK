# PaySimple SDK v1.4.2

PaySimple SDK is a fully asynchronous and validatable .NET SDK designed around the PaySimple API v4

## Installation

Use NuGet to get the latest version of the SDK

```
Install-Package PaySimpleSdk
```

Create new instance of the PaySimpleSettings object and pass your Api Key and Username:

```
var settings = new PaySimpleSettings("AoOtRylA63570WmH3eqChyFRqwhTnA2g0dnsV7zzQko4s4yKWdBorA1WiT7dK2H2xz06P562Hqv0heYBdfNamfQyxX50drtpL8s7", "AUserName");
```
Optionally, declare the number of retries on failed requests:
```
var settings = new PaySimpleSettings("AoOtRylA63570WmH3eqChyFRqwhTnA2g0dnsV7zzQko4s4yKWdBorA1WiT7dK2H2xz06P562Hqv0heYBdfNamfQyxX50drtpL8s7", "AUserName", automaticRetryCount: 5);
```

Create a service and start calling the API:

```
var paymentScheduleService = new PaymentScheduleService(settings);
var paymentSchedules = await paymentScheduleService.GetAllPaymentSchedulesAsync();
```

That's it!

## Documentation

Visit the following documentation pages for each of the SDK services:

* [Account Service](https://github.com/PaySimple/PaySimpleSDK/wiki/Account-Service)
* [Customer Service](https://github.com/PaySimple/PaySimpleSDK/wiki/Customer-Service)
* [Payment Service](https://github.com/PaySimple/PaySimpleSDK/wiki/Payment-Service)
* [Payment Schedule Service](https://github.com/PaySimple/PaySimpleSDK/wiki/Payment-Schedule-Service)

Visit the documentation for usage on the PagedResult&lt;T&gt; class and PaySimple Exceptions:

* [PagedResult&lt;T&gt;](https://github.com/PaySimple/PaySimpleSDK/wiki/PagedResult)
* [Exceptions](https://github.com/PaySimple/PaySimpleSDK/wiki/Exceptions)
