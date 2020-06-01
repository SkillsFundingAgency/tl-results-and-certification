EXECUTE('DROP USER [' + @Username + ']')
EXECUTE ('CREATE USER [' + @Username + '] WITH PASSWORD = ''' + @Password + ''', DEFAULT_SCHEMA=[dbo]')
EXECUTE('ALTER ROLE db_datareader ADD MEMBER [' + @Username +']')
EXECUTE('ALTER ROLE db_datawriter ADD MEMBER [' + @Username +']')