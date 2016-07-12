# PaySimple SDK v1.0.11

PaySimple SDK is a fully asynchronous and validatable .NET SDK designed around the PaySimple API v4

## Installation

Use NuGet to get the latest version of the SDK 

```
Install-Package PaySimpleSdk
```

Create new instance of the PaySimpleSettings object and pass your Api Key and Username:

```
#!C#
var settings = new PaySimpleSettings("AoOtRylA63570WmH3eqChyFRqwhTnA2g0dnsV7zzQko4s4yKWdBorA1WiT7dK2H2xz06P562Hqv0heYBdfNamfQyxX50drtpL8s7", "AUserName");
```
Optionally, declare the number of retries on failed requests:
```
#!C#
var settings = new PaySimpleSettings("AoOtRylA63570WmH3eqChyFRqwhTnA2g0dnsV7zzQko4s4yKWdBorA1WiT7dK2H2xz06P562Hqv0heYBdfNamfQyxX50drtpL8s7", "AUserName", automaticRetryCount: 5);
```

Create a service and start calling the API:

```
#!C#
var paymentScheduleService = new PaymentScheduleService(settings);
var paymentSchedules = await paymentScheduleService.GetAllPaymentSchedulesAsync();
```

That's it!

## Documentation

Visit the following documentation pages for each of the SDK services:

* [Account Service](https://bitbucket.org/scottlance/paysimplesdk/wiki/AccountService)
* [Customer Service](https://bitbucket.org/scottlance/paysimplesdk/wiki/CustomerService)
* [Payment Service](https://bitbucket.org/scottlance/paysimplesdk/wiki/PaymentService)
* [Payment Schedule Service](https://bitbucket.org/scottlance/paysimplesdk/wiki/PaymentScheduleService)

Visit the documentation for usage on the PagedResult<T> class and PaySimple Exceptions:

* [PagedResult<T>](https://bitbucket.org/scottlance/paysimplesdk/wiki/PagedResult)
* [Exceptions](https://bitbucket.org/scottlance/paysimplesdk/wiki/Exceptions)

## Documentation

Visit the documentation wiki at: https://bitbucket.org/scottlance/paysimplesdk/wiki/Home