/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
IF ('$(environment)' <> 'test')
BEGIN
:r ".\SeedTlAwardingOrganisations.sql"
:r ".\SeedTlRoutes.sql"
:r ".\SeedTlPathways.sql"
:r ".\SeedTlSpecialisms.sql"
:r ".\SeedTqAwardingOrganisations.sql"
:r ".\SeedNotificationTemplates.sql"
:r ".\SeedTlProviders.sql"
END