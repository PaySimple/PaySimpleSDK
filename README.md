# PaySimple SDK v1.0.0

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

Create a service and start calling the API:

```
#!C#
var paymentScheduleService = new PaymentScheduleService(settings);
var paymentSchedules = await paymentScheduleService.GetAllPaymentSchedulesAsync();
```

That's it!

## Documentation

Visit the documentation wiki at: https://bitbucket.org/scottlance/paysimplesdk/wiki/Home