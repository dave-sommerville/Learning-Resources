## Data Types
### Numeric Data Types

Used to store numbers, which can be exact (integers/decimals) or approximate (floating-point).

| **Data Type**                  | **Description**                                                                                                            | **Example**                                 |
| ------------------------------ | -------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------- |
| **`INT` / `INTEGER`**          | A standard integer (whole number).                                                                                         | `42`, `-1005`                               |
| **`SMALLINT`**                 | A smaller integer for saving storage space.                                                                                | `15`                                        |
| **`BIGINT`**                   | A large integer for very big numbers.                                                                                      | `9223372036854775807`                       |
| **`DECIMAL(p,s)` / `NUMERIC`** | Exact fixed-point numbers. `p` is precision (total digits), `s` is scale (digits after decimal). Ideal for financial data. | `DECIMAL(10,2)` $\rightarrow$ `12345678.90` |
| **`FLOAT` / `REAL`**           | Approximate floating-point numbers.                                                                                        | `3.14159`                                   |
### Character and String Data Types
Used to store text, characters, and strings.

| **Data Type**    | **Description**                                                                                        | **Example**                                  |
| ---------------- | ------------------------------------------------------------------------------------------------------ | -------------------------------------------- |
| **`CHAR(n)`**    | Fixed-length character string. If the text is shorter than `n`, it pads it with spaces.                | `CHAR(2)` $\rightarrow$ `'NY'`               |
| **`VARCHAR(n)`** | Variable-length character string. Stores only the characters you input up to the limit `n`.            | `VARCHAR(255)` $\rightarrow$ `'Hello World'` |
| **`TEXT`**       | Stores large amounts of text data (e.g., product descriptions, blog posts). Length varies by database. | `'Once upon a time...'`                      |
### Date and Time Data Types

Used to manage dates, times, and timestamps.

| **Data Type**   | **Description**                                                                                         | **Example Format**          |
| --------------- | ------------------------------------------------------------------------------------------------------- | --------------------------- |
| **`DATE`**      | Stores date values (Year, Month, Day).                                                                  | `'2026-07-10'`              |
| **`TIME`**      | Stores time values (Hour, Minute, Second).                                                              | `'14:30:00'`                |
| **`DATETIME`**  | Stores both date and time values.                                                                       | `'2026-07-10 14:30:00'`     |
| **`TIMESTAMP`** | Similar to `DATETIME`, but often used to track changes (tracks Unix time and can adjust to time zones). | `'2026-07-10 19:30:00 UTC'` |
| **`INTERVAL`**  | Represents a duration or period of time.                                                                | `'3 DAYS'`                  |
## Core SQL Clauses

Here are the standard clauses you will use in almost every query, listed in the order they are typically written:

#### `SELECT`
- **What it does:** Specifies which columns you want to retrieve.
- **Example:** `SELECT first_name, email`
#### `FROM`
- **What it does:** Specifies which table (or tables, using `JOIN`) to pull the data from.
- **Example:** `FROM customers`
### `WHERE`
- **What it does:** Filters rows based on a specific condition _before_ any grouping happens.
- **Example:** `WHERE country = 'Canada'`
### `GROUP BY`
- **What it does:** Groups rows that have the same values into summary rows. It is almost always used with aggregate functions like `COUNT()`, `SUM()`, or `AVG()`.
- **Example:** `GROUP BY country`
### `HAVING`
- **What it does:** Filters groups created by the `GROUP BY` clause. (Note: `WHERE` filters individual rows; `HAVING` filters aggregated groups).
- **Example:** `HAVING COUNT(customer_id) > 100`
### `ORDER BY`
- **What it does:** Sorts the final result set in ascending (`ASC`) or descending (`DESC`) order.
- **Example:** `ORDER BY last_name DESC`
### TOP
- **What it does:** Restricts the total number of rows returned. 
- **Example:** `TOP 10`

Example:
```
SELECT country, COUNT(customer_id) AS total_customers
FROM customers
WHERE status = 'Active'
GROUP BY country
HAVING COUNT(customer_id) > 5
ORDER BY total_customers DESC
TOP 5;
```


## Built In Functions 
### Data Conversion
#### CAST
- Converts an expression or column from one data type to another.
- `CAST(expression AS target_data_type)`
### String Manipulation
#### SUBSTRING
- Extracts a specific section of text from a larger string based on a designated starting position and length.
- `SUBSTRING(string, start_position, length)`
#### LEN
- Returns the total number of characters in a string.
- `LEN(string)`
#### TRIM
- Removes leading and trailing spaces from a string.
- `TRIM(string)`
#### CONCAT
- Joins two or more strings together into a single text value.
- `CONCAT(string1, string2, ...)`
### Aggregation
#### COUNT
- Returns the total number of rows matching the query criteria.
- ``COUNT(column_name)` or `COUNT(*)``
#### SUM
- Calculates the total arithmetic addition of a numeric column.
- `SUM(column_name)`
#### AVG
- Calculates the mathematical average (mean) of a numeric column.
- `AVG(column_name)`
#### MIN/Max
- Evaluates a column and returns its lowest (`MIN`) or highest (`MAX`) value.
### Conditional Null Handling
#### COALESCE
- Evaluates a sequence of arguments from left to right and returns the very first non-null value it encounters.
- `COALESCE(value1, value2, ..., fallback_value)`


## Operators
### Logical
#### AND
- Equivalent to C#'s `&&`
#### OR
- Equivalent to C#'s `||`
#### NOT
- Equivalent to C#'s `!`
### Comparison
#### = 
- Equal To 
- Equivalent to C#'s `==`
- **Not** used for assigning values
#### <> 
- Not  Equal To
- Equivalent to C#'s `!=`
- `!=` does work in SQL, but isn't recommended
#### \>,< 
- Greater Than/ Less Than
#### \>=, <=
- Greater Than/ Less Than OR Equal To
### Range and Complex Comparison
#### BETWEEN	
- Filters values within an inclusive range (numbers, dates, or text).	
- `WHERE age BETWEEN 18 AND 25`
#### IN	
- Checks if a value matches any value in a specified list or subquery.	
- `WHERE country IN ('UK', 'US', 'CA')`
#### LIKE	
- Matches a pattern using wildcards (% and _). 
- `WHERE name LIKE 'J%'`
- %: Wildcard represents any sequence of zero, one, or multiple characters.
- \_: wildcard represents exactly one single character.
- Either % or \_ can be used on either or both sides
#### IS NULL	
- Checks for empty/missing data (NULL). Note: You cannot use = NULL. 	
- `WHERE phone_number IS NULL`
#### EXISTS
- Tests whether a subquery returns any rows, instantly returning `TRUE` and stopping the scan the moment a single match is found. Example:

```
SELECT department_name 
FROM departments d 
WHERE EXISTS ( 
	SELECT 1 
	FROM employees e 
	WHERE e.department_id = d.department_id 
);
```

## Misc.
### Comments
To Add Comments in SQL, start a statement with `--`
```
--This is a SQL Comment
SELECT *
FROM tableName;
```
### Aliases
An alias is a **temporary nickname** given to a table or a column in a query to make the SQL cleaner to read or to rename an output column. It only lasts for the duration of that specific query. It's keyword is `AS`
```
-- Column Alias: Renames the output header to 'Total'
SELECT price * quantity AS total_cost 
FROM orders;

-- Table Alias: Gives 'employees' the nickname 'e' to save typing
SELECT e.first_name, e.last_name 
FROM employees AS e;
```

### Triggers
A **trigger** is an automated database script that automatically runs (fires) in response to a specific event on a table, such as an `INSERT`, `UPDATE`, or `DELETE`.
Think of it like a digital tripwire: when data changes, the trigger automatically fires a predefined action to maintain data integrity, log changes, or enforce business rules without human intervention. Example
```
-- 1. Create a trigger that fires AFTER an UPDATE on the employees table
CREATE TRIGGER audit_salary_change
AFTER UPDATE ON employees
FOR EACH ROW
WHEN (OLD.salary IS DISTINCT FROM NEW.salary)
BEGIN
    -- 2. Insert the tracking data into an audit history table
    INSERT INTO salary_history_log (employee_id, old_salary, new_salary, changed_at)
    VALUES (:OLD.employee_id, :OLD.salary, :NEW.salary, CURRENT_TIMESTAMP);
END;
```
**Timing:** Triggers can be set to run `BEFORE` the data changes (e.g., to validate or fix data before it hits the table) or `AFTER` the data changes (e.g., to log history or clean up related tables).
**OLD and NEW Modifiers:** Triggers have access to special virtual tables. 
- `:OLD` holds the data exactly how it looked _before_ the query ran. 
- `:NEW` holds the proposed change.
### Intellisense Error (red squiggly lines)
The reason your code shows a red squiggly error in SQL Server Management Studio (SSMS) but still executes perfectly is **IntelliSense cache lag**.
The code parser that runs inside your text editor (IntelliSense) runs independently of the actual SQL Server database engine. When you create a new object like the `Departments` table, the database engine knows about it instantly, which is why your script runs fine. However, the IntelliSense cache doesn't update automatically in real-time, so the editor still thinks the `Departments` table doesn't exist.
You can fix this visual error instantly by forcing SSMS to refresh its local cache.
- **Keyboard Shortcut:** Press **`Ctrl + Shift + R`**
- **Menu Path:** Go to **Edit** -> **IntelliSense** -> **Refresh Local Cache**
