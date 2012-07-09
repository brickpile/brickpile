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

###Optional configuration
**Important:** If you choose to use this configuration you need to change the **loginUrl** back to the original value `~/Account/LogOn`

You can configure BrickPile to use a sub domain for the back office, this can be done by adding the following app setting

	<appSettings>
		<add key="brickpile/uisubdomain" value="ui." />
	</appSettings>

then you need to edit the bindings for your web site

![Edit bindings](images/edit-bindings.png)

when this is done you also need to add two entrys in your hosts.config file

![Edit hosts.config](images/add-bindings.png)

**Note:** This is the prefered way of running BrickPile because we can be absolutely certain that there can't be any page url that collides with any of the built in controllers.

##Setup
When the configuration is done just hit `F5` inside Visual Studio, this will hopefully bring up the setup screen.
Fill the form with the correct information and hit "Let's do this" and your done.

**Note:** If you are using the **Optional configuration** mode you can't hit `F5`, instead you just navigate to `http://ui.brickpile.local`. 

##Updating
Updating BrickPile just as simple as installing, use the following command to check if there is a new version of BrickPile

	PM> Get-Package -updates

If there is one use the following command to update the package to the latest version

	PM> Update-Package BrickPile