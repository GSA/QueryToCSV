# QueryToCSV

## Description: 
Takes a query and saves it as a csv file

## Nuget packages and other dll's utilized
* CsvHelper (https://www.nuget.org/packages/CsvHelper/)
* Ionic.Zip (http://www.codeplex.com/DotNetZip)
* MySQL.data.dll (https://www.nuget.org/packages/MySql.Data/)

## Initial Setup
### Default config setup

The repository contains a app config that points to external config files. These external config files not controlled by version control will need to be created and configured prior to running the application. The files required and the default configuration can be found below. For those on the development team, additional details can be found in the documentation on the google drive in the GIT team drive.


 * **Things to do before your first commit**
   * Make a new branch for development. All pre-existing branches are protected and cannot be pushed to directly.
   * You can publish a new branch and do pull requests to have your changes incorporated into the project.
   * Once you have created a new branch you will need to create the config files. (see below for more info on this)
   * Default version of these files are provided in the repo with the .example extension
   * Copy these files into the project **bin\Debug folder** and change the extension to .config using the previous filename
   * Or create new files that contain the code as seen below and place them in the **bin\Debug folder**
   * Do not push your config files to the repository. Pull requests that include these files will be rejected.
 
 * **Current config files that will need to be added.**
   * ConnectionStrings.config
   * AppSettings.config
 
* **Default settings for these files will follow this line**
 
   * **ConnectionStrings.config file should contain the following lines.** 
    ~~~ xml
   <connectionStrings>
	<add name="CONNSTRING" connectionString="connection string" />
   </connectionStrings>
    ~~~

   * **AppSettings.config should contain the following lines.**
  ~~~ xml
  <appSettings>
  <add key="zipKey" value="MyKeyHere"/>
  <add key="zipLocation" value="C:\temp\"/>    
  </appSettings>
  ~~~
  
  ***
  
## Usage
Run from the command line supplying the following arguments
  * Query string
  * File Location
  * File Name
  * Delimiter
  * Zip Archive (bool)
  * Archive Name

```
[Repo Path]\obj\x86\Debug\QueryToCSV.exe "call hspd.uspGetPIVCardInfo('20505000118000180E0E');" "C:\Temp\Random" XTEC.dat true Sample.zip
```

## Contributing
Fork this repository, make changes in your fork, and then submit a pull-request, remembering not to upload any system specific configuration files, PII, or sensitive data of any type. 

## Credits
GSA
