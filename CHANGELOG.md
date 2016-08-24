#PaySimple SDK Changelog

#v1.0.8 - 2.25.2016

Updated all requests to only use TLS 1.2 

PCI-DSS will require all tramissions to be over TLS 1.1 or greater by June 30, 2018 (see http://blog.pcisecuritystandards.org/migrating-from-ssl-and-early-tls)

#v1.0.9 - 5.23.2016

Updated library to allow for retries on failed web requests:

* The number of retries can be set in the PaySimpleSettings constructor
* The default number of retries is 1

#v1.0.10 - 5.27.2016

Fixed an issue where the new constructor created for retries broke backwards compatability

#v1.1.0 - 8.23.2016

Add support for international payments.  Validators on Postal Codes were removed and a list of all countries was added to the CountryCode enumeration
