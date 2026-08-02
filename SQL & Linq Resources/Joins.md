In a relational database, data is split into multiple specialized tables to avoid duplication. The `JOIN` operation allows you to link these tables back together during a query. Tables are connected by matching values—typically where a **Foreign Key** in one table matches the **Primary Key** in another.

### INNER JOIN

Returns only the rows where there is a matching value in **both** tables. If a record in the first table doesn't have a corresponding record in the second table, it will not appear in the results.
![[INNER JOIN.png]]

```
-- Returns only employees who are assigned to an existing department
SELECT 
    Employees.FirstName, 
    Employees.LastName, 
    Departments.DepartmentName
FROM Employees
INNER JOIN Departments 
    ON Employees.DepartmentID = Departments.DepartmentID;
```


### LEFT (OUTER) JOIN

Returns **all** rows from the left table (the one listed first in the `FROM` clause), plus any matching rows from the right table. If there is no match for a row on the right side, the result will still show the left row, but the right table's columns will display as `NULL`.

![[LEFT JOIN.png]]

```
-- Returns ALL employees, even if they haven't been assigned to a department yet
SELECT 
    Employees.FirstName, 
    Employees.LastName, 
    Departments.DepartmentName
FROM Employees
LEFT JOIN Departments 
    ON Employees.DepartmentID = Departments.DepartmentID;
```

Graphics © C.L. Moffat, 2008

### RIGHT (OUTER) JOIN

The exact inverse of a `LEFT JOIN`. It returns **all** rows from the right table (the one listed after the `JOIN` keyword), plus any matching rows from the left table. If a row in the right table has no matching row on the left, the left table's columns will display as `NULL`.

![[RIGHT JOIN.png]]

```
-- Returns ALL departments, even if there are currently no employees assigned to them
SELECT 
    Employees.FirstName, 
    Employees.LastName, 
    Departments.DepartmentName
FROM Employees
RIGHT JOIN Departments 
    ON Employees.DepartmentID = Departments.DepartmentID;
```

### FULL (OUTER) JOIN

Returns **all rows from both tables**, regardless of whether a match exists or not. It essentially combines a `LEFT JOIN` and a `RIGHT JOIN` into a single query.
![[OUTER JOIN.png]]
```
-- Returns ALL employees and ALL departments, highlighting all mismatches on both sides
SELECT 
    Employees.FirstName, 
    Employees.LastName, 
    Departments.DepartmentName
FROM Employees
FULL JOIN Departments 
    ON Employees.DepartmentID = Departments.DepartmentID;
```


## Set Operators (Combining Queries)

While `JOIN` operations combine columns from different tables horizontally, **Set Operators** combine the results of two or more separate `SELECT` statements vertically into a single result set.
### The Golden Rules for Set Operations
To use any set operator, your queries must follow these strict requirements:
1. Both queries must return the **same number of columns**.
2. The columns must have **compatible data types** in the exact same order (e.g., if column 1 in query A is an `INT`, column 1 in query B must also be an `INT`).
3. The column names in the final result set are always determined by the **first** query.

### UNION / UNION ALL
Combines the result sets of two queries into a single list.
- **UNION:** Automatically removes duplicate rows from the final result. If a row appears in both queries, it is only listed once.
- **UNION ALL:** Keeps all duplicates. It simply pastes the results of the second query directly underneath the first, making it significantly faster because SQL Server does not have to spend time scanning for duplicates.

**Structure Template:**
SQL

```
SELECT ColumnName1, ColumnName2 FROM TableA
UNION -- or UNION ALL
SELECT ColumnName1, ColumnName2 FROM TableB;
```

**Example:**

SQL

```
-- Creates a single master mailing list of everyone, removing duplicates
SELECT FirstName, LastName, Email FROM Employees
UNION
SELECT FirstName, LastName, Email FROM Customers;
```

### INTERSECT

Returns only the rows that exist in the result sets of **both** queries. If a record appears in query A but not in query B, it is completely excluded from the final output. Duplicates are automatically removed.

**Structure Template:**

SQL

```
SELECT ColumnName1, ColumnName2 FROM TableA
INTERSECT
SELECT ColumnName1, ColumnName2 FROM TableB;
```

**Example:**

SQL

```
-- Finds people who are currently both an employee and an active customer
SELECT FirstName, LastName, Email FROM Employees
INTERSECT
SELECT FirstName, LastName, Email FROM Customers;
```

### EXCEPT (Difference)

Returns rows from the first query that **do not exist** in the second query. The order of the queries matters entirely here; it effectively subtracts the second result set from the first. Duplicates are automatically removed.

**Structure Template:**

SQL

```
SELECT ColumnName1, ColumnName2 FROM TableA
EXCEPT
SELECT ColumnName1, ColumnName2 FROM TableB;
```

**Example:**

SQL

```
-- Finds employees who have never placed an order as a customer
SELECT FirstName, LastName, Email FROM Employees
EXCEPT
SELECT FirstName, LastName, Email FROM Customers;
```

_Note: If you flipped the order of these tables, the query would instead find customers who are not employees._