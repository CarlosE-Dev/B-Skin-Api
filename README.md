<div>
  
  ## About B-Skin API
  B-Skin API was developed with the focus on ensuring the functioning of the B-Skin website, a clothing shop focused in T-Shirts which was developed by [@analuizadev](https://github.com/analuizadev) 
  and can be seen [here.](https://github.com/analuizadev/B-skin)
  The implementation is composed of a 3-tier architecture, where the focus was on building as quickly as possible to unlock and enable front-end building, but without losing quality in the process.
  
  Developed with C#, ASP .NET Core, SQL Server, Dapper using a 3-layer architeture with CQRS, UnitOfWork and Repository Pattern.
  
  <div align="center">
    <img src="https://media.discordapp.net/attachments/1050461916474122251/1090459415016124516/image.png"></img>
  </div>
  
  
  ## Provider Endpoints
  
  <div align="center">
    <img src="https://media.discordapp.net/attachments/1050461916474122251/1090462186205368360/image.png"></img>
  </div>
  
  
   ## TShirt Endpoints
  
  <div align="center">
    <img src="https://media.discordapp.net/attachments/1050461916474122251/1090463109631713280/image.png"></img>
  </div>
  
  
  ## Installing
  
  ### Prerequisites
  What you need to run the API

  ```
  - .NET 5
  - SQL Server
  - Any Database Workbench
  - PowerShell or Command Prompt
  - NotePad or Any Code Editor
  ```
  
  ### Step 1: Create Database
  Creating and populating the database
  
  Open your Database Workbench and execute in sequence the following [script.](https://github.com/CarlosE-Dev/B-Skin-Api/blob/master/B-Skin-Api.Data/Scripts/Scripts.txt)
  This will create the database structure and also insert the default records
  
  ### Step 2: Configure the database connection
  Editing the connection string
  
  In your computer file explorer, navigate to the folder B-Skin-Api.Data, and then navigate to the folder called Dapper

  Path Example: C:\Dev\B-Skin-Api\B-Skin-Api.Data\Dapper
  
  Open the file DbSession.cs ( you can open as NotePad, NotePad++ or any code editor you want )
  
  Then modify the connection string to your SQL Server configuration
  
  <div align="center">
    <img src="https://media.discordapp.net/attachments/1050461916474122251/1090483849743585421/image.png"></img>
  </div>
  
  ### Step 3: Run the API
  With PowerShell or Command Prompt navigate to API Folder as the following example:
  
  ```
  cd "C:\Dev\B-Skin-Api\B-Skin-Api"
  ```
  
  In the API folder, you just need to run the following command:
  
  ```
  dotnet watch run
  ```
  
  If all steps have been completed, the swagger interface should open while running
  
</div>





