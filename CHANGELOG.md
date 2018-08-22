# PaySimple SDK Changelog

## v1.4.2 - 8.22.2018

Fixed a property name for serialization

## v1.4.1 - 2.9.2018

Credit card validation now allows for 19 digit carts and new MasterCard bin ranges.

## v1.4.0 - 2.5.2018

The PaymentScheduleService.GetAllPaymentSchedulesAsync() will now return List<PaymentSchedule>.
Renamed PaymentScheduleService.GetRecurringPaymentsAsync to PaymentScheduleService.GetRecurringSchedulePaymentsAsync

## v1.3.1 - 12.19.2017

Added paging to PaymentScheduleService.GetAllPaymentSchedulesAsync() call.

## v1.3 - 9.08.2017

Added ability to get a customer token so users of PaySimpleJS can add credit card accounts to existing authenticated customers.

## v1.2.2 - 7.17.2017

Fixed a null reference exception when no errors were sent back from the server

Deserialization no longer closes the stream

## v1.2.1 - 6.29.2017

Fixed PaymentSubType serialization issue with Payment Schedules.

## v1.2.0 - 3.21.2017

Added new API calls

We now use a shared instance of HttpClient

Response deserialization now happens from the response stream


## v1.1.1 - 2.01.2017

Fixed an issue where a deserialization exception would get obscured.

Updated dependent libraries

* FluentValidation 5.6.2.0 > 6.2.1
* Newtonsoft.Json 6.0.8 > 9.0.1

## v1.1.0 - 8.23.2016

Add support for international payments. Validators on Postal Codes were removed and a list of all countries was added to the CountryCode enumeration

## v1.0.10 - 5.27.2016

Fixed an issue where the new constructor created for retries broke backwards compatability

## v1.0.9 - 5.23.2016

Updated library to allow for retries on failed web requests:

* The number of retries can be set in the PaySimpleSettings constructor
* The default number of retries is 1


## v1.0.8 - 2.25.2016

Updated all requests to only use TLS 1.2 

PCI-DSS will require all tramissions to be over TLS 1.1 or greater by June 30, 2018 (see http://blog.pcisecuritystandards.org/migrating-from-ssl-and-early-tls)


