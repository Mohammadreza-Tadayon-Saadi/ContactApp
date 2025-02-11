# ContactApp
Simple App Using ReactJs, Asp.Net Core, SQL Server (Three Layer)
----------------------------------
In .Net Application We Have 5 Layer:
  1) Presentation   : Handle Http Request and pass it to the Application Layer
    => All Controllers Return A Same Object For Response To The Client (Consistent Response)

  2) WebConfig      : Configures Application => Middlewares, IoC, Mapper, ...
    => Handle CORS, NotFound, Logging, InjectServices and Also Configure AutoMapper For Mapping An Object To Another Object

  3) Application    : Contract With Infrastrure Layer For Get/Set Data
    => Validate Objects, Build Services, Use CQRS For Communicate With Presentation, ...

  4) Infrastructure : Communicate With DataBase And Have Entities On It
    => Build Context and Tables Using EFCore (We Can Also Add Redis and Other Out Source)

  5) Core           : A Layer That Is Utilities Layer => Convertors, Statuses, Securities Component, CustomResult, ...


In ReactJs Layer We Have:
  1) Components For Contact and Group Section
  2) Communicate With Application For Get/Set Data