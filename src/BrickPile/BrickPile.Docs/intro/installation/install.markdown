#Installation

BrickPile is installed via [NuGet](http://nuget.org/List/Packages/BrickPile), just run PM> Install-Package BrickPile inside of the package manager console.

## Configuration

As default BrickPile will use the RavenDB server on http://localhost:8080 but this can easily be configured to use an other port or RavenDB embedded with the following example.

    <connectionStrings>
        <add name="RavenDB" connectionString="DataDir = ~\App_Data\Data" />
    </connectionStrings>

**Note:** To run BrickPile with RavenDB server you need to [download](http://ravendb.net/download) it and execute `[RavenDBdir]\Server\Raven.Server.exe`. Then you need to copy the assembly in `[WebRoot]\App_Data\Plugins` and put in '[RavenDBdir]\Server\Plugins`. For more configuration options see [RavenDB documentation](http://ravendb.net/documentation).

You also need to change the loginUrl on the forms tag in web.config to

    <authentication mode="Forms">
      <forms loginUrl="~/dashboard/account" timeout="2880" />
    </authentication>

**Optional:** I usually turn off ClientValidationEnabled and UnobtrusiveJavaScriptEnabled by setting the value to false.

    <appSettings>
        <add key="ClientValidationEnabled" value="false" />
        <add key="UnobtrusiveJavaScriptEnabled" value="false" />
    </appSettings>

## Setup

When the configuration is done just hit `F5` inside Visual Studio, this will hopefully bring up the setup screen.
Fill the form with the correct information and hit "Let's do this" and your done.
