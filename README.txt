WikipediaMaze 2.0

Prerequisites
========================
The following is a list of items that must be installed:

	1. .NET Framework 4.0
	2. Visual Studio 2010
	3. ASP.NET MVC 3
	4. MS SQL Server 2005 or higher


Setup
========================
1. Database Setup
	
	First run the scripts in located in the sql folder in the following order:

	1. CreateDatabse.sql
	2. BaselineData.sql
	
2. Update Settings 
	
	All of the application settings are located in the /source/WikipediaMaze.Web/web.config file. 
	
	Add the connection string to the database previosly setup under the <connectionStrings> section. 
	
	The following application settings are required and will need to be supplied under the <appSettings> section:

	1. RpxApiKey
	2. RpxDomain
	
	Rpx - Wikipedia Maze uses JanRain Engage (formerly Rpx) to handle authentication. You will need to sign up for a free account at http://www.janrain.com/products/engage.
	
	
	
	