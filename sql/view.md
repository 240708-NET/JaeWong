# View
https://learn.microsoft.com/en-us/sql/relational-databases/views/views?view=sql-server-ver16

A view is a virtual table whose contents are defined by a query. 

Views do not store any data but DO reflect changes to the table's data.

The main benefit of views is that you can increase data security by employing permission access (like allowing users to query the view but not to query the table(s) itself).

Views can also be used to provide a backward compatible interface to emulate a table that used to exist but whose schema has changed.

### Syntax
```sql
-- Create the view
CREATE VIEW dbo.InvoiceWithCustomerInfo
AS
SELECT i.InvoiceDate, c.FirstName, c.LastName, i.Total
FROM dbo.Invoice AS i
INNER JOIN dbo.Customer AS c
    ON i.CustomerId = c.CustomerId;
GO

-- Query the view
SELECT *
FROM dbo.InvoiceWithCustomerInfo
ORDER BY InvoiceDate;
GO

-- Modify the view
ALTER VIEW dbo.InvoiceWithCustomerInfo
AS
SELECT i.InvoiceId, c.FirstName, c.LastName, c.State, i.Total
FROM dbo.Invoice AS i
INNER JOIN dbo.Customer AS c
    ON i.CustomerId = c.CustomerId;
GO

-- Update date through view
UPDATE dbo.InvoiceWithCustomerInfo
SET State = 'NC'
WHERE InvoiceId = 1;
GO

-- Drop/Delete the view
DROP VIEW dbo.InvoiceWithCustomerInfo;
GO
```

### 
