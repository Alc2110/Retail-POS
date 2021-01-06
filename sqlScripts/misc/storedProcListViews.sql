--create stored procedure to list all views
/*
CREATE PROCEDURE ListViewsProc
AS
SELECT v.name
FROM sys.views as v;
*/

--execute it
EXEC ListViewsProc;