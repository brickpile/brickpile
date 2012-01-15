#Configuration
As default BrickPile will use the RavenDB server on http://localhost:8080 but this can easily be configured to use an other port or RavenDB embedded with the following example.

{XML @ConnectionString.xml}

**Note:** To run BrickPile with RavenDB server you need to [download](http://ravendb.net/download) it and execute `[RavenDBdir]\Server\Raven.Server.exe`. Then you need to copy the assembly in `[WebRoot]\App_Data\Plugins` and put in '[RavenDBdir]\Server\Plugins`. For more configuration options see [RavenDB documentation](http://ravendb.net/documentation).

You also need to change the loginUrl on the forms tag in web.config to

{XML @Authentication.xml}

**Optional:** I usually turn off ClientValidationEnabled and UnobtrusiveJavaScriptEnabled by setting the value to false.

{XML @AppSettings.xml}
