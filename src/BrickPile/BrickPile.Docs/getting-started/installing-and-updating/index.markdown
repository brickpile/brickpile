#Installing and Updating
##Getting Installed
Installing BrickPile is simply the act of writing a single PowerShell command inside the package manager console.

	PM> Install-Package BrickPile

**And that's basically it.** Of course you need an empty ASP.NET MVC 3 web application.

##Configuration
As default BrickPile will use the RavenDB server on http://localhost:8080 but this can easily be configured to use an other port or RavenDB embedded with the following example.

	<connectionStrings>
		<add name="RavenDB" connectionString="DataDir = ~\App_Data\Data" />
	</connectionStrings>

**Note:** To run BrickPile with RavenDB server you need to [download](http://ravendb.net/download) it and execute `[RavenDBdir]\Server\Raven.Server.exe`. For more configuration options see [RavenDB documentation](http://ravendb.net/documentation).

You also need to change the loginUrl on the forms tag in web.config to

	<authentication mode="Forms">
		<forms loginUrl="~/ui/account" timeout="2880" />
	</authentication>

##Setup
When the configuration is done just hit `F5` inside Visual Studio, this will hopefully bring up the setup screen.
Fill the form with the correct information and hit "Let's do this" and your done.

##Updating
Updating BrickPile just as simple as installing, use the following command to check if there is a new version of BrickPile

	PM> Get-Package -updates

If there is one use the following command to update the package to the latest version

	PM> Update-Package BrickPile