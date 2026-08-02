### Table Creation
Each column follows a strict `[Column Name] [Data Type] [Constraint]` pattern, separated by commas. Every table begins with the `CREATE TABLE` statement.
Structure Template:
```
CREATE TABLE TableName
	ColumnName DataType Constraints, 
	ColumnName DataType Constraints,
	...
```
Example (without Constraints)
```
CREATE TABLE Students
(
    StudentID INT,
    FirstName VARCHAR(50),
    LastName VARCHAR(50)
);
```
At this point there are **no rules** on the data. Every column allows NULL values and there is nothing preventing duplicate StudentIDs.
### Constraints
- `IDENTITY(1,1)`: This is an auto-incrementing property. The first row gets a value of `1`, and each new row adds `1`. You do not manually insert numbers into an identity column.
- `DEFAULT GETDATE()`: If you don't provide a `HireDate` during an insert, SQL Server will automatically grab the current system date and stamp it for you.
- `CHECK (Salary >= 0)`: A validation rule that physically prevents anyone from accidentally inserting a negative salary.
- `NOT NULL`: forces the column to always have data. 
- `UNIQUE`: Guarantees that every single entry in this column is completely distinct from the others, preventing anyone from accidentally entering a duplicate value.
- `PRIMARY KEY`: Combines the rules of `UNIQUE` and `NOT NULL` into a single constraint. A table can only have one primary key, ensuring you always have a foolproof way to find, update, or delete a specific record.
Example:
```
CREATE TABLE Departments(
	DepartmentID INT IDENTITY(1,1) PRIMARY KEY, 
	DepartmentName VARCHAR(100) NOT NULL UNIQUE 
);
```
#### Foreign Key References
`FOREIGN KEY`: This is your link between tables. In the script below, the `DepartmentID` column in the `Employees` table points back to the `DepartmentID` column in the `Departments` table.
- This prevents "orphan" records. You cannot assign an employee to Department #99 if Department #99 doesn't exist in the parent table yet.
```
CREATE TABLE Employees(
	EmployeeId INT IDENTITY(1,1) PRIMARY KEY, 
	FirstName VARCHAR(50) NOT NULL, 
	LastName VARCHAR(50) NOT NULL, 
	Email VARCHAR(100) NULL, 
	HireDate DATE DEFAULT GETDATE(), 
	Salary DECIMAL(10,2) CHECK (Salary >= 0), 
	DepartmentID INT NOT NULL, 
	
	-- Defining the Foreign Key constraint 
	CONSTRAINT FK_Employees_Departments 
	FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID) 
	ON DELETE CASCADE 
);
```

#### Default Constraints
If a user or application inserts a row but leaves a specific column blank, a `DEFAULT` constraint automatically fills that column with a predefined fallback value instead of leaving it as `NULL`.
```
CREATE TABLE UserAccounts ( 
	UserID INT IDENTITY(1,1) PRIMARY KEY, 
	Username VARCHAR(50) NOT NULL, 
	-- Automatic Values / Defaults: 
	IsActive BIT DEFAULT 1, 
	CreatedDate DATETIME DEFAULT GETDATE() 
);
```

## Deletion Behavior
### `CASCADE`
- If a row in the parent table is deleted, all matching rows in the child table are automatically deleted along with it.
```
CREATE TABLE Employees (
	EmployeeID INT IDENTITY(1,1) PRIMARY KEY, 
	FirstName VARCHAR(50) NOT NULL, 
	LastName VARCHAR(50) NOT NULL, 
	DepartmentID INT NOT NULL, 
    
	-- If Department 5 is deleted, all employees in Department 5 are deleted
	CONSTRAINT FK_Employees_Departments 
	FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID) 
	ON DELETE CASCADE 
);
```
### `SET NULL`
- If a row in the parent table is deleted, the foreign key column in the child table is automatically changed to `NULL`. This keeps the child record alive but breaks the link to the deleted parent. The column must allow NULLs for this to work.
```
CREATE TABLE Employees (
	EmployeeID INT IDENTITY(1,1) PRIMARY KEY, 
	FirstName VARCHAR(50) NOT NULL, 
	LastName VARCHAR(50) NOT NULL, 
	DepartmentID INT NULL, -- Must be NULL to use SET NULL
    
	-- If Department 5 is deleted, affected employees stay, but their DepartmentID becomes NULL
	CONSTRAINT FK_Employees_Departments 
	FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID) 
	ON DELETE SET NULL 
);
```
### `SET DEFAULT`
- If a row in the parent table is deleted, the foreign key column in the child table is automatically reset to its pre-defined `DEFAULT` value. The default value must already exist in the parent table.
```
CREATE TABLE Employees (
	EmployeeID INT IDENTITY(1,1) PRIMARY KEY, 
	FirstName VARCHAR(50) NOT NULL, 
	LastName VARCHAR(50) NOT NULL, 
	DepartmentID INT NOT NULL DEFAULT 1, -- Default fallback department
    
	-- If Department 5 is deleted, affected employees are moved to Department 1
	CONSTRAINT FK_Employees_Departments 
	FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID) 
	ON DELETE SET DEFAULT 
);
```
### `NO ACTION` (default behavior)
- If you do not specify an `ON DELETE` rule, SQL Server defaults to `NO ACTION`. If a user tries to delete a row in the parent table that still has matching rows in the child table, SQL Server will physically block the delete and throw an error.
```
CREATE TABLE Employees (
	EmployeeID INT IDENTITY(1,1) PRIMARY KEY, 
	FirstName VARCHAR(50) NOT NULL, 
	LastName VARCHAR(50) NOT NULL, 
	DepartmentID INT NOT NULL, 
    
	-- You cannot delete a Department until you manually remove or reassign its employees first
	CONSTRAINT FK_Employees_Departments 
	FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID) 
	ON DELETE NO ACTION 
);
```
## Table Altering

Once a table has been created, you often need to make changes without deleting it.
The `ALTER TABLE` statement allows you to:
- Add new columns
- Remove columns
- Change data types
- Add constraints
- Remove constraints
### Adding Columns
Suppose we create a table like below.
```
CREATE TABLE Students
(
    StudentID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50)
);
```
Then later we decide to store email addresses for Students. Instead of creating a whole new table, you can use `ALTER TABLE` like so:
```
ALTER TABLE Students
ADD Email VARCHAR(100);
```
Any rows that existed prior to altering the table will be given a NULL value. This means you can't use `NOT NULL` when adding a column. You **can**, however, provide a default value for pre-existing rows in order to allow a `NOT NULL` constraint. Example:
```
ALTER TABLE Students
ADD Age INT NOT NULL
DEFAULT 18;
```
**You can add multiple rows at once as well.**
```
ALTER TABLE Students
ADD
    Phone VARCHAR(20),
    BirthDate DATE,
    GPA DECIMAL(3,2);
```
### Changing Column's Data Type and Constraints
You may need to change your data type as your database evolves. If you currently have an `Email NVARCHAR(10)` and the length needs to increase, you can alter it like this:
```
ALTER TABLE Students
ALTER COLUMN Email NVARCHAR(150);
```
This will only work if the conversion can be made. For example, a `CHAR(10)` attribute with values `{1,2,5,8}` in its columns can be altered to an `INT` data type, but one with `{'apple', 'orange'}` would fail. **You can also change the constraints on the `ALTER COLUMN` line.** 
_If you want to make a column nullable, add the `NULL` constraint._
##### Removing a Constraint
If you want to remove a constraint, you can `DROP` it
```
ALTER TABLE Employees
DROP CONSTRAINT FK_Employees_Departments;
--Similar for primary keys but with PK instead of FK
```
## Removing Data
#### `DELETE` Whole Table - One Row at a Time
- Remove the entire table, one row at a time, with just the table name.
- `DELETE FROM Students;` 
#### `DELETE/WHERE` Specified Rows
- Remove specific rows from the name using the `WHERE` clause.
```
DELETE FROM Students
WHERE StudentID = 5;
```
#### `TRUNCATE` - Whole Table - All at Once
- Delete entire table all at once, much faster
- `TRUNCATE TABLE Students;`
#### `DROP` - Nuclear Option
- Completely delete the entire table structure, all of its data, its constraints, and its permissions from the database permanently.
- `DROP TABLE Students;`
##### **Warning!!** 
Unlike `DELETE` or `TRUNCATE` which leave the empty table structure intact for future use, `DROP` completely erases the table from existence. If you run this, the table no longer exists.
## Insertion and Deletion
### `INSERT INTO`
- The `INSERT INTO` statement allows you to add new records to a table. You specify the table name, the columns you want to populate, and the corresponding values.
- If a column has an `IDENTITY` property, a `DEFAULT` constraint, or allows `NULL` values, you can omit it from your insert statement. SQL Server will handle the values automatically.
- It is best practice to explicitly list your target columns. This ensures that even if the table structure changes later (like adding a new nullable column), your insert statement won't break.
```
INSERT INTO Students (FirstName, LastName, Email)
VALUES ('Jane', 'Doe', 'jane.doe@example.com');
```
- You can insert multiple rows of data at the same time by separating your value lists with commas.
```
INSERT INTO Students (FirstName, LastName)
VALUES 
    ('Alex', 'Smith'),
    ('Maria', 'Garcia'),
    ('Liam', 'Johnson');
```

### `UPDATE`
- The `UPDATE` statement modifies existing data within a table. You specify which columns to change and what the new values should be.
- To modify specific records, you must include a `WHERE` clause. This restricts the update to only the rows that match your criteria.
```
UPDATE Students
SET Email = 'alex.smith.updated@example.com'
WHERE StudentID = 5;
```
- You can update multiple columns at once as well
```
UPDATE Students
SET 
    FirstName = 'Robert', 
    LastName = 'Miller'
WHERE StudentID = 2;
```

#### Warning!!
If you completely omit the `WHERE` clause from an `UPDATE` statement, the change will physically apply to **every single row** in the table.
```
-- DANGER: This changes the age to 21 for ALL students in the database
UPDATE Students
SET Age = 21;
```